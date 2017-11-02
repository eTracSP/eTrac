using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorkOrderEMS.Helper;

namespace WorkOrderEMS.Controllers.Login
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/
        public ActionResult Index()
        {
            { return RedirectToAction("index", "Login"); }
        }
        public ActionResult Login(string ReturnUrl)
        {
            TempData["logout"] = CommonMessage.SessionExpired();
            return RedirectToAction("index", "Login", ReturnUrl);
        }

    }
}