using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FSAstorePwned.Models;
using FSAstorePwned.Code;


namespace FSAstorePwned.Controllers
{
    public class HomeController : Controller
    {
        PasswordPnwed emptyModel = new PasswordPnwed();
        public ActionResult Index()
        {
            emptyModel.Password = "";
            emptyModel.retHex = "";
            emptyModel.retCount = "";

            return View(emptyModel);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult PasswordCheck(PasswordPnwed model)
        {
            if (ModelState.IsValid)
            {
                PasswordPnwed matchingHex = new PasswordPnwed();
                try
                {
                    matchingHex = Globals.passwordCheck(model);
                    model.retCount = matchingHex.retCount;
                    model.retHex = matchingHex.retHex;
                    return View("Index", model);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            else
            {
                emptyModel.Password = "";
                emptyModel.retHex = "";
                emptyModel.retCount = "";
                return View("Index", emptyModel);
            }
          
        }
    }
}