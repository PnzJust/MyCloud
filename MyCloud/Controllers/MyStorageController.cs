using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.VisualBasic.Devices;

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
            string key = @"secret12";
            byte[] plainContent = System.IO.File.ReadAllBytes(path);
            using (var DES = new DESCryptoServiceProvider())
            {
                DES.IV = Encoding.UTF8.GetBytes(key);
                DES.Key = Encoding.UTF8.GetBytes(key);
                DES.Mode = CipherMode.CBC;
                DES.Padding = PaddingMode.PKCS7;
                using (var memStream = new MemoryStream())
                {
                    CryptoStream cryptoStream = new CryptoStream(memStream, DES.CreateEncryptor(), CryptoStreamMode.Write);
                    cryptoStream.Write(plainContent, 0, plainContent.Length);
                    cryptoStream.FlushFinalBlock();
                    System.IO.File.WriteAllBytes(path, memStream.ToArray());                       
                }
            }
            return RedirectToAction("Storage", "MyStorage");
        }

        // GET: /MyStorage/Decrypt/{id}
        public ActionResult Decrypt(string id)
        {
            string path = @"C:\ALL\" + User.Identity.GetUserId() + @"\" + id;
            string key = @"secret12";
            byte[] encrypted = System.IO.File.ReadAllBytes(path);

            using (var DES = new DESCryptoServiceProvider())
            {
                DES.IV = Encoding.UTF8.GetBytes(key);
                DES.Key = Encoding.UTF8.GetBytes(key);
                DES.Mode = CipherMode.CBC;
                DES.Padding = PaddingMode.PKCS7;

                using (var memStream = new MemoryStream())
                {
                    CryptoStream cryptoStream = new CryptoStream(memStream, DES.CreateDecryptor(), CryptoStreamMode.Write);
                    cryptoStream.Write(encrypted, 0, encrypted.Length);
                    cryptoStream.FlushFinalBlock();
                    System.IO.File.WriteAllBytes(path, memStream.ToArray());
                }
            }
            return RedirectToAction("Storage", "MyStorage");
        }

        // GET: /MyStorage/Download/{id}
        public ActionResult Download(string id)
        {

            string path = @"C:\ALL\" + User.Identity.GetUserId() + @"\" + id;
            var net = new System.Net.WebClient();
            var data = net.DownloadData(path);
            var content = new System.IO.MemoryStream(data);
            return File(content, @"Application/octet-stream", id);
        }

        // POST: /MyStorage/Upload
        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            string path = @"C:\ALL\" + User.Identity.GetUserId() + @"\" + file.FileName.Split('\\').Last();
            file.SaveAs(path);
            return RedirectToAction("Storage", "MyStorage");
        }
    }
}