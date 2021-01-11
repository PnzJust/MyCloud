using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyCloud.Models;
using Renci.SshNet;

namespace MyCloud.Controllers
{
    public class InstanceController : Controller
    {
        private DbCtx db = new DbCtx();
        public ActionResult Index()
        {
            List<Instance> instances = db.Instances.ToList();
            ViewBag.Instances = instances;

            return View();
        }

        public ActionResult Details(string id)
        {

            Instance instance = db.Instances.Find(id);
            if (instance != null)
            {
                return View(instance);
            }
            return HttpNotFound(@"Could not find instance with this id " + id);
        }

        [HttpGet]
        public ActionResult New()
        {
            Instance instance = new Instance();
            return View(instance);
        }

        [HttpPost]
        public ActionResult New(Instance instanceRequest)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    int remotePort = 6500;
                    int test_port = db.Instances.Max(x => x.InstancePort);
                    if (test_port > remotePort)
                        remotePort = test_port;

                    SshClient sshclient = new SshClient("192.168.56.104", "test1234", "test1234");
                    sshclient.Connect();
                    SshCommand sc = sshclient.CreateCommand(@"sudo docker run -d -p " + remotePort.ToString() + @":22 -it 2df997a84a17");
                    sc.Execute();
                    string answer = sc.Result;
                    string docker_id = answer.Substring(0, 12);

                    sc = sshclient.CreateCommand(@"sudo docker exec " + docker_id.ToString() + @" sh -c /usr/sbin/sshd");
                    sc.Execute();
                    sshclient.Disconnect();

                    instanceRequest.InstanceId = docker_id;
                    instanceRequest.InstancePort = remotePort;
                    instanceRequest.InstanceIsOn = true;

                    db.Instances.Add(instanceRequest);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(instanceRequest);
            }
            catch (Exception e)
            {
                return View(instanceRequest);
            }
        }
        
        [HttpGet]
        public ActionResult Edit(string id)
        {
            
            Instance instance = db.Instances.Find(id);
            if (instance == null)
                return HttpNotFound("Couldn't find the instance with id " + id.ToString());
            return View(instance);
        }

        [HttpPut]
        public ActionResult Edit(string id, Instance instanceRequest)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    Instance instance = db.Instances.SingleOrDefault(i => i.InstanceName.Equals(id));
                    
                    if(TryUpdateModel(instance))
                    {
                        instance.InstanceDescription = instanceRequest.InstanceDescription;
                        instance.InstanceName = instanceRequest.InstanceName;
                        db.SaveChanges();
                    }
                    return RedirectToAction("Index");
                }
                return View(instanceRequest);
            }
            catch (Exception e)
            {
                return View(instanceRequest);
            }
        }

        [HttpDelete]
        public ActionResult Delete(string id)
        {
            Instance instance = db.Instances.Find(id);
            if (instance != null)
            {
                SshClient sshclient = new SshClient("192.168.56.104", "test1234", "test1234");
                sshclient.Connect();
                SshCommand sc = sshclient.CreateCommand(@"sudo docker stop " + instance.InstanceId.ToString());
                sc.Execute();
                sc = sshclient.CreateCommand(@"sudo docker rm  " + instance.InstanceId.ToString());
                sc.Execute();
                sshclient.Disconnect();

                db.Instances.Remove(instance);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return HttpNotFound("Couldn't find the instance with id " + id.ToString());
        }
    }
}