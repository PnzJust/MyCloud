using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace MyCloud.Controllers
{
    public class MyStorageController : Controller
    {
        // GET: /MyStorage/Storage
        [HttpGet]
        public ActionResult Storage()
        {
            string path = @"C:\ALL\" + User.Identity.GetUserId() + @"\";
            ViewData["allFiles"] = Directory.EnumerateFiles(path).ToList();
            return View();
        }

        // GET: /MyStorage/Delete/{id}
        public ActionResult Delete(string id)
        {
            string path = @"C:\ALL\" + User.Identity.GetUserId() + @"\" + id;
            System.IO.File.Delete(path);
            return RedirectToAction("Storage", "MyStorage");
        }


        // GET: /MyStorage/Open/{id}
        public ActionResult Open(string id)
        {
            string path = @"C:\ALL\" + User.Identity.GetUserId() + @"\" + id;
            if (!System.IO.File.Exists(path)) // does not exists
                return View("~/Shared/Error");

            string text = "";
            // Open the stream and read it back.
            using (StreamReader sr = System.IO.File.OpenText(path))
            {
                string s = "";
                while ((s = sr.ReadLine()) != null)
                {
                    text = text + s;
                }
            }
            ViewData["text"] = text;

            return View();
        }


        // GET: /MyStorage/Encrypt/{id}
        public ActionResult Encrypt(string id)
        {
            string path = @"C:\ALL\" + User.Identity.GetUserId() + @"\" + id;
            System.IO.File.Encrypt(path);
            return RedirectToAction("Storage", "MyStorage");
        }

        // GET: /MyStorage/Decrypt/{id}
        public ActionResult Decrypt(string id)
        {
            string path = @"C:\ALL\" + User.Identity.GetUserId() + @"\" + id;
            System.IO.File.Decrypt(path);
            return RedirectToAction("Storage", "MyStorage");
        }

        // GET: /MyStorage/Rename/{id}
        public ActionResult Rename(string id)
        {

            string path = @"C:\ALL\" + User.Identity.GetUserId() + @"\" + id;
            return RedirectToAction("Storage", "MyStorage");
        }
    }
}