using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FSAstorePwned;
using FSAstorePwned.Controllers;
using FSAstorePwned.Models;

namespace FSAstorePwned.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            //Arrange
            HomeController controller = new HomeController();

            // Setup
            PasswordPnwed model = new PasswordPnwed();
            model.Password = "P@ssw0rd";

            // Act
            ViewResult result = controller.PasswordCheck(model) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }


    }
}
