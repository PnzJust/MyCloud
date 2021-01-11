using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.SqlServer.Server;
using System.Web.Mvc;

namespace MyCloud.Models
{
    public class Instance
    {
        [Key]
        [Column("instance_id")]
        public string InstanceId { get; set; }

        [Column("instance_name")]
        [Required]
        [RegularExpression(@"^([A-Z0-9])*$", ErrorMessage = "Name should be uppercase and letters only")]
        [MinLength(3, ErrorMessage = "Name could not be less than 3 characters")]
        public string InstanceName { get; set; }
        [Column("instance_passwd")]
        public string InstancePasswd { get; set; }
        [Required]
        [Column("instance_ip")]
        public string InstanceIp { get; set; }
        [Required]
        [Column("instance_port")]
        public int InstancePort { get; set; }
        [Column("instance_ison")]
        public bool InstanceIsOn { get; set; }
        [Column("instance_description")]
        [Required]
        //[]
        public string InstanceDescription { get; set; }
        public Instance()
        {
            this.InstanceId = "";
            this.InstanceIp = "192.168.56.104";
            this.InstanceIsOn = false;
            this.InstancePasswd = "cloudpass";
            this.InstancePort = 0;
        }
    }

    public class DbCtx: DbContext
    {
        public DbCtx() : base("DbConnectionString")
        {
            Database.SetInitializer<DbCtx>(new Initp());
        }
        public DbSet<Instance> Instances { get; set; }
       
    }
    public class Initp : DropCreateDatabaseAlways<DbCtx>
    {
        protected override void Seed (DbCtx ctx)
        {
            //ctx.Instances.Add(new Instance { InstanceId = @"0000", InstanceName = @"NNNN", InstancePasswd = @"passwd",InstanceIp = @"IP", InstancePort = 1025, InstanceIsOn = true, InstanceDescription=@"Descrierea vietii" });
            ctx.SaveChanges();
            base.Seed(ctx);
        }
    }
}