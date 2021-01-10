using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyCloud.Controllers
{
    public class MyInstancesController : Controller
    {
        // GET: MyInstances
        public ActionResult Instances()
        {
            return View();
        }
    }
}