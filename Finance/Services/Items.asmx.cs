using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Eclipse.PhpaLibrary.Database.Store;
using Eclipse.PhpaLibrary.Reporting;
using EclipseLibrary.Web.UI;
using EclipseLibrary.Web.JQuery.Input;

namespace PhpaAll.Services
{
    /// <summary>
    /// Summary description for Items
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class Items : System.Web.Services.WebService
    {

        [WebMethod]
        public AutoCompleteItem[] GetItems(string term)
        {
            //int itemId = string.IsNullOrEmpty(id) ? 0 : Convert.ToInt32(id);

            using (StoreDataContext db = new StoreDataContext(ReportingUtilities.DefaultConnectString))
            {
                db.ObjectTrackingEnabled = false;
                return (from item in db.Items
                        where item.ItemCode.Contains(term) || item.ItemCode.StartsWith(term)
                        || item.Description.Contains(term) || item.Description.StartsWith(term)
                        || item.Brand.Contains(term) || item.Brand.StartsWith(term)
                        || item.Color.Contains(term) || item.Color.StartsWith(term)
                        || item.Dimension.Contains(term) || item.Dimension.StartsWith(term)
                        || item.Identifier.Contains(term) || item.Identifier.StartsWith(term)
                        || item.Size.Contains(term) || item.Size.StartsWith(term)
                        orderby item.Description
                        select item).Take(30).ToArray()
                            .Select(item => new AutoCompleteItem()
                            {
                                Value = item.ItemId.ToString(),
                                Text = string.Join(",", new string[] { item.ItemCode, item.Description,
                                            item.Color, item.Brand, item.Dimension, item.Identifier, item.Size}.Where(p => !string.IsNullOrWhiteSpace(p))),
                                Detail = item.ItemCode
                            }).ToArray();
            }
        }

        [WebMethod]
        public AutoCompleteItem ValidateItem(string term)
        {
            int ItemId;
            try
            {
                ItemId = string.IsNullOrEmpty(term) ? 0 : Convert.ToInt32(term);
            }
            catch (OverflowException)
            {
                // Id is either too large or too small for an Int32. Ignore it.
                return null;
            }
            catch (FormatException)
            {
                // Id is not numeric. Ignore it.
                return null;
            }
            using (StoreDataContext db = new StoreDataContext(ReportingUtilities.DefaultConnectString))
            {
                db.ObjectTrackingEnabled = false;
                var query = (from item in db.Items
                             where item.ItemId == ItemId
                             select item).SingleOrDefault();
                if (query == null)
                {
                    return null;
                }
                else
                {
                    return new AutoCompleteItem()
                            {
                                Value = query.ItemId.ToString(),
                                Text = string.Join(",", new string[] { query.ItemCode, query.Description,
                                            query.Color, query.Brand, query.Dimension, query.Identifier, query.Size}
                                            .Where(p => !string.IsNullOrWhiteSpace(p))),
                                Detail = query.ItemCode
                            };
                }
            }
        }
    }
}
