using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhpaAll.Controllers
{
    public abstract partial class PhpaBaseController : Controller
    {

        public List<string> StatusMessages
        {
            get
            {
                return TempData["StatusMessages"] as List<string>;
            }
            private set
            {
                TempData["StatusMessages"] = value;
            }
        }

        protected void AddStatusMessage(string msg)
        {
            if (StatusMessages == null)
            {
                StatusMessages = new List<string>();
            }
            StatusMessages.Add(msg);
        }


    }
}