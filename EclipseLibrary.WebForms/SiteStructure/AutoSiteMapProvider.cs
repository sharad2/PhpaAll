using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;

namespace EclipseLibrary.Web.SiteStructure
{
    /// <summary>
    /// Automatically creates a sitemap by parsing all the *.aspx files within a root folder.
    /// You specify the name of the default page, and all other pages are presumed to be children of the
    /// default page. The SiteMapNode is populates using the information gleaned by parsing the page.
    /// </summary>
    /// <include file='AutoSiteMapProvider.xml' path='AutoSiteMapProvider/doc[@name="class"]/*'/>
    public class AutoSiteMapProvider : StaticSiteMapProvider
    {
        #region Initialization
        private string _homePageUrl;
        private string _siteMapRoot;

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        public override void Initialize(string name, NameValueCollection attributes)
        {
            _homePageUrl = attributes["homePageUrl"];
            _siteMapRoot = attributes["siteMapRoot"];

            string str = attributes["customAttributes"];
            if (!string.IsNullOrEmpty(str))
            {
                char[] seperators = new char[] { ',' };
                string[] customAttributes = attributes["customAttributes"].Split(seperators, StringSplitOptions.RemoveEmptyEntries);

                _customRegexList = new Dictionary<string, Regex>(customAttributes.Length);
                foreach (string customAttribute in customAttributes)
                {
                    str = string.Format(@"\<meta.*\sname=\""{0}\"".*?\scontent=\""(?<{0}>[^\""]*).*?/>", customAttribute);
                    Regex customRegex = new Regex(str, RegexOptions.Singleline | RegexOptions.Compiled);
                    _customRegexList.Add(customAttribute, customRegex);
                }
            }
            base.Initialize(name, attributes);
        }
        #endregion

        #region Sitemap building
        /// <summary>
        /// <inheritdoc />
        /// </summary>
        /// <returns>The root node</returns>
        /// <remarks>
        /// If the sitemap has not already been constructed, parse all the files to construct it now
        /// </remarks>
        public override SiteMapNode BuildSiteMap()
        {
            lock (this)
            {
                SiteMapNode rootNode = GetRootNodeFromCache();
                if (rootNode == null)
                {
                    this.Clear();
                    rootNode = DoBuildSiteMap();
                }
                return rootNode;
            }
        }

        /// <summary>
        /// This must be protected with a call to lock
        /// </summary>
        private SiteMapNode DoBuildSiteMap()
        {
            SiteMapNode rootNode = new SiteMapNode(this, "root");
            rootNode.Url = _homePageUrl;
            string rootdir = HttpContext.Current.Request.PhysicalApplicationPath;
            if (!string.IsNullOrEmpty(_siteMapRoot))
            {
                rootdir += _siteMapRoot;
            }

            DirectoryInfo dirinfo = new DirectoryInfo(rootdir);
            FileInfo[] files = dirinfo.GetFiles("*.aspx", SearchOption.AllDirectories);
            List<string> directories = new List<string>(files.Length);
            //directories.Add(rootdir);
            foreach (DirectoryInfo d in dirinfo.GetDirectories())
            {
                directories.Add(d.FullName);
            }
            foreach (FileInfo file in files)
            {
                SiteMapNode node = ParseFile(file);
                if (node != null)
                {
                    string siteMapRoot = string.Format("~/{0}", _siteMapRoot);
                    node.Url = file.FullName.Replace(rootdir, siteMapRoot).Replace('\\', '/');
                    if (file.Name == _homePageUrl)
                    {
                        this.RemoveNode(rootNode);
                    }
                    else
                    {
                        this.AddNode(node, rootNode);
                    }
                }
            }

            this.AddNode(rootNode);
            SetRootNodeInCache(rootNode, directories.ToArray());
            return rootNode;
        }

        private static Regex _titleRegex = new Regex(@"\<%@\s*?Page.*\sTitle=\""(?<title>[^\""]*).*?%>", RegexOptions.Singleline | RegexOptions.Compiled);
        private Regex _descriptionRegex = new Regex(@"\<meta.*\sname=\""Description\"".*?\scontent=\""(?<description>[^\""]*).*?/>", RegexOptions.Singleline | RegexOptions.Compiled);
        private Dictionary<string, Regex> _customRegexList;

        private SiteMapNode ParseFile(FileInfo file)
        {
            SiteMapNode node = new SiteMapNode(this, file.Name);
            StringBuilder sb = new StringBuilder();
            MatchCollection matches;

            using (StreamReader stream = file.OpenText())
            {
                string s;
                bool bWaitingForMetaEnd = false;
                while ((s = stream.ReadLine()) != null)
                {
                    if (string.IsNullOrEmpty(node.Title))
                    {
                        int index;
                        while ((index = s.IndexOf("%>")) < 0)
                        {
                            // Keep reading until closing %> is found
                            sb.Append(s);
                            s = stream.ReadLine();
                        }

                        // Set the title and get rid of everything before the <%@Page ... %> tag
                        sb.Append(s, 0, index + 2);
                        matches = _titleRegex.Matches(sb.ToString());
                        if (matches.Count > 0)
                        {
                            node.Title = matches[0].Groups["title"].Value;
                        }
                        else
                        {
                            node.Title = "Untitled";
                        }
                        sb.Length = 0;
                        if (s.Length > index + 2)
                        {
                            sb.Append(s, index + 2, s.Length - index - 2);
                        }
                    }
                    if (bWaitingForMetaEnd)
                    {
                        bWaitingForMetaEnd = !HandleMetaEnd(sb, s, node);
                    }
                    else
                    {
                        // Check for Meta begin
                        int indexMetaBegin = s.IndexOf("<meta ");
                        if (indexMetaBegin >= 0)
                        {
                            sb.Append(s, indexMetaBegin, 6);
                            s = s.Substring(indexMetaBegin + 6);
                            bWaitingForMetaEnd = !HandleMetaEnd(sb, s, node);
                        }
                        else
                        {
                            // Ignore this line
                        }
                    }
                }
            }


            //if (_customRegex != null)
            //{
            //    foreach (var item in _customRegex)
            //    {
            //        matches = item.Value.Matches(sb.ToString());
            //        if (matches.Count > 0)
            //        {
            //            node[item.Key] = matches[0].Groups[item.Key].Value;
            //        }
            //    }
            //}

            //matches = _titleRegex.Matches(sb.ToString());
            //if (matches.Count > 0)
            //{
            //    node.Title = matches[0].Groups["title"].Value;
            //}
            //else
            //{
            //    node.Title = string.Empty;
            //}

            //matches = _descriptionRegex.Matches(sb.ToString());
            //if (matches.Count > 0)
            //{
            //    node.Description = matches[0].Groups["description"].Value;
            //}
            return node;
        }

        /// <summary>
        /// Returns true if the end tag was found.
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="s"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        /// <remarks>
        /// When the end tag is found, it is parsed and the property is set in the site map node
        /// </remarks>
        private bool HandleMetaEnd(StringBuilder sb, string s, SiteMapNode node)
        {
            int indexMetaEnd = s.IndexOf("/>");
            if (indexMetaEnd < 0)
            {
                sb.Append(s);
                return false;
            }

            sb.Append(s, 0, indexMetaEnd + 2);
            MatchCollection matches;
            if (string.IsNullOrEmpty(node.Description))
            {
                matches = _descriptionRegex.Matches(sb.ToString());
                if (matches.Count > 0)
                {
                    node.Description = matches[0].Groups["description"].Value;
                }
            }
            if (_customRegexList != null)
            {
                foreach (var item in _customRegexList.Where(p => string.IsNullOrEmpty(node[p.Key])))
                {
                    matches = item.Value.Matches(sb.ToString());
                    if (matches.Count > 0)
                    {
                        node[item.Key] = matches[0].Groups[item.Key].Value;
                    }
                }
            }
            sb.Length = 0;
            if (s.Length > indexMetaEnd + 2)
            {
                sb.Append(s, indexMetaEnd + 2, s.Length - (indexMetaEnd + 2));
                if (sb.ToString().Contains("<meta "))
                {
                    throw new NotSupportedException("Current Limitation: Each meta tag must begin on a seperate line");
                }
            }
            return true;
        }
        #endregion

        #region Boilerplate overrides
        /// <summary>
        /// <inheritdoc />
        /// </summary>
        /// <returns></returns>
        protected override SiteMapNode GetRootNodeCore()
        {
            return this.RootNode;
        }

        /// <summary>
        /// Constructs the sitemap if necessary and returns the root node
        /// </summary>
        /// <remarks>
        /// The DebuggerBrowsableAttribute ensures that the sitemap does not get built simply because
        /// you tried to look at <c>RootNode</c> in the debugger
        /// </remarks>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public override SiteMapNode RootNode
        {
            get
            {
                SiteMapNode temp = null;
                temp = BuildSiteMap();
                return temp;
            }
        }
        #endregion

        #region Application Cache
        private SiteMapNode GetRootNodeFromCache()
        {
            return (SiteMapNode)HttpContext.Current.Cache["AutoSiteMap"];
        }

        /// <summary>
        /// Sets dependency on the directories passed.
        /// Whenever any file in the list of directories changes, the sitemap is reconstructed.
        /// </summary>
        private void SetRootNodeInCache(SiteMapNode node, string[] directories)
        {
            string[] cacheKeys = new string[1];
            CacheDependency dep = new CacheDependency(directories);
            HttpContext.Current.Cache.Insert("AutoSiteMap", node, dep, Cache.NoAbsoluteExpiration,
                Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        /// <remarks>
        /// Remove the sitemap provider entries from the application cache
        /// </remarks>
        protected override void Clear()
        {
            lock (this)
            {
                HttpContext.Current.Cache.Remove("AutoSiteMap");
                base.Clear();
            }
        }
        #endregion
    }
}
