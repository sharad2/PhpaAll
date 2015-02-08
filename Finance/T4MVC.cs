﻿// <auto-generated />
// This file was generated by a T4 template.
// Don't change it directly as your change would get overwritten.  Instead, make changes
// to the .tt file (i.e. the T4 template) and save it to regenerate this file.

// Make sure the compiler doesn't complain about missing Xml comments and CLS compliance
// 0108: suppress "Foo hides inherited member Foo. Use the new keyword if hiding was intended." when a controller and its abstract parent are both processed
#pragma warning disable 1591, 3008, 3009, 0108
#region T4MVC

using System;
using System.Diagnostics;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Html;
using System.Web.Routing;
using T4MVC;

[GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
public static partial class MVC
{
    public static PhpaAll.Controllers.BillsController Bills = new PhpaAll.Controllers.T4MVC_BillsController();
    public static PhpaAll.Controllers.BillsHomeController BillsHome = new PhpaAll.Controllers.T4MVC_BillsHomeController();
    public static PhpaAll.Controllers.ManageBillsController ManageBills = new PhpaAll.Controllers.T4MVC_ManageBillsController();
    public static T4MVC.SharedController Shared = new T4MVC.SharedController();
}

namespace T4MVC
{
}

namespace T4MVC
{
    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public class Dummy
    {
        private Dummy() { }
        public static Dummy Instance = new Dummy();
    }
}

[GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
internal partial class T4MVC_System_Web_Mvc_ActionResult : System.Web.Mvc.ActionResult, IT4MVCActionResult
{
    public T4MVC_System_Web_Mvc_ActionResult(string area, string controller, string action, string protocol = null): base()
    {
        this.InitMVCT4Result(area, controller, action, protocol);
    }
     
    public override void ExecuteResult(System.Web.Mvc.ControllerContext context) { }
    
    public string Controller { get; set; }
    public string Action { get; set; }
    public string Protocol { get; set; }
    public RouteValueDictionary RouteValueDictionary { get; set; }
}
[GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
internal partial class T4MVC_System_Web_Mvc_JsonResult : System.Web.Mvc.JsonResult, IT4MVCActionResult
{
    public T4MVC_System_Web_Mvc_JsonResult(string area, string controller, string action, string protocol = null): base()
    {
        this.InitMVCT4Result(area, controller, action, protocol);
    }
    
    public string Controller { get; set; }
    public string Action { get; set; }
    public string Protocol { get; set; }
    public RouteValueDictionary RouteValueDictionary { get; set; }
}



namespace Links
{
    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public static class Views {
        private const string URLPATH = "~/Views";
        public static string Url() { return T4MVCHelpers.ProcessVirtualPath(URLPATH); }
        public static string Url(string fileName) { return T4MVCHelpers.ProcessVirtualPath(URLPATH + "/" + fileName); }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public static class Bills {
            private const string URLPATH = "~/Views/Bills";
            public static string Url() { return T4MVCHelpers.ProcessVirtualPath(URLPATH); }
            public static string Url(string fileName) { return T4MVCHelpers.ProcessVirtualPath(URLPATH + "/" + fileName); }
        }
    
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public static class BillsHome {
            private const string URLPATH = "~/Views/BillsHome";
            public static string Url() { return T4MVCHelpers.ProcessVirtualPath(URLPATH); }
            public static string Url(string fileName) { return T4MVCHelpers.ProcessVirtualPath(URLPATH + "/" + fileName); }
        }
    
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public static class ManageBills {
            private const string URLPATH = "~/Views/ManageBills";
            public static string Url() { return T4MVCHelpers.ProcessVirtualPath(URLPATH); }
            public static string Url(string fileName) { return T4MVCHelpers.ProcessVirtualPath(URLPATH + "/" + fileName); }
            public static readonly string Bill_css = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/Bill.min.css") ? Url("Bill.min.css")+"?"+T4MVCHelpers.TimestampString(URLPATH + "/Bill.min.css") : Url("Bill.css")+"?"+T4MVCHelpers.TimestampString(URLPATH + "/Bill.css");
                 
            public static readonly string Bill_js = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/Bill.min.js") ? Url("Bill.min.js")+"?"+T4MVCHelpers.TimestampString(URLPATH + "/Bill.min.js") : Url("Bill.js")+"?"+T4MVCHelpers.TimestampString(URLPATH + "/Bill.js");
                    }
    
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public static class Shared {
            private const string URLPATH = "~/Views/Shared";
            public static string Url() { return T4MVCHelpers.ProcessVirtualPath(URLPATH); }
            public static string Url(string fileName) { return T4MVCHelpers.ProcessVirtualPath(URLPATH + "/" + fileName); }
            public static readonly string jqueryval_js = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/jqueryval.min.js") ? Url("jqueryval.min.js")+"?"+T4MVCHelpers.TimestampString(URLPATH + "/jqueryval.min.js") : Url("jqueryval.js")+"?"+T4MVCHelpers.TimestampString(URLPATH + "/jqueryval.js");
                    public static readonly string layout_css = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/layout.min.css") ? Url("layout.min.css")+"?"+T4MVCHelpers.TimestampString(URLPATH + "/layout.min.css") : Url("layout.css")+"?"+T4MVCHelpers.TimestampString(URLPATH + "/layout.css");
                 
            public static readonly string layout_js = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/layout.min.js") ? Url("layout.min.js")+"?"+T4MVCHelpers.TimestampString(URLPATH + "/layout.min.js") : Url("layout.js")+"?"+T4MVCHelpers.TimestampString(URLPATH + "/layout.js");
                    public static readonly string layout_partial_css = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/layout.partial.min.css") ? Url("layout.partial.min.css")+"?"+T4MVCHelpers.TimestampString(URLPATH + "/layout.partial.min.css") : Url("layout.partial.css")+"?"+T4MVCHelpers.TimestampString(URLPATH + "/layout.partial.css");
                 
            public static readonly string layout_partial_js = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/layout.partial.min.js") ? Url("layout.partial.min.js")+"?"+T4MVCHelpers.TimestampString(URLPATH + "/layout.partial.min.js") : Url("layout.partial.js")+"?"+T4MVCHelpers.TimestampString(URLPATH + "/layout.partial.js");
                    }
    
        public static readonly string web_config = Url("web.config")+"?"+T4MVCHelpers.TimestampString(URLPATH + "/web.config");
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public static partial class Bundles
    {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public static partial class Scripts {}
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public static partial class Styles {}
    }
}

[GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
internal static class T4MVCHelpers {
    // You can change the ProcessVirtualPath method to modify the path that gets returned to the client.
    // e.g. you can prepend a domain, or append a query string:
    //      return "http://localhost" + path + "?foo=bar";
    private static string ProcessVirtualPathDefault(string virtualPath) {
        // The path that comes in starts with ~/ and must first be made absolute
        string path = VirtualPathUtility.ToAbsolute(virtualPath);
        
        // Add your own modifications here before returning the path
        return path;
    }

    // Calling ProcessVirtualPath through delegate to allow it to be replaced for unit testing
    public static Func<string, string> ProcessVirtualPath = ProcessVirtualPathDefault;

    // Calling T4Extension.TimestampString through delegate to allow it to be replaced for unit testing and other purposes
    public static Func<string, string> TimestampString = System.Web.Mvc.T4Extensions.TimestampString;

    // Logic to determine if the app is running in production or dev environment
    public static bool IsProduction() { 
        return (HttpContext.Current != null && !HttpContext.Current.IsDebuggingEnabled); 
    }
}





#endregion T4MVC
#pragma warning restore 1591, 3008, 3009, 0108


