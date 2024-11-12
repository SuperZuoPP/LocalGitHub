using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using WPFBase.Api.Context.Model;
using WPFBase.Entities.BM;
using WPFBase.Entities.SM;

namespace WPFBase.Api.Context
{
    public class BaseContext : DbContext
    {
        public BaseContext(DbContextOptions<BaseContext> options) : base(options)
        {

        }
        public DbSet<ToDo> ToDo { get; set; }
        public DbSet<Memo> Memo { get; set; }
        public DbSet<User> User { get; set; } 
        public DbSet<Operator> Operator { get; set; }

        public DbSet<tb_weigh_operator> TbWeighOperator { get; set; }

        public DbSet<tb_weigh_usergroup> TbWeighUsergroup { get; set; } 
        public DbSet<tb_weigh_groupauthorityusers> TbWeighGroupauthorityuser { get; set; } 
        public DbSet<tb_weigh_menu> TbWeighMenu { get; set; }

        public DbSet<tb_weigh_groupauthority> TbWeighGroupauthority { get; set; }

        public DbSet<tb_weigh_datalineinfo> tb_weigh_datalineinfo { get; set; }

        public DbSet<tb_weigh_littleplan> TbWeighLittleplan { get; set; }

        public DbSet<tb_weigh_plan> TbWeighPlan { get; set; }

        public DbSet<tb_weigh_weighbridgeoffice> TbWeighOffice { get; set; }

        public DbSet<tb_weigh_video> TbWeighVideo { get; set; }

        public DbSet<tb_weigh_devicestatus> TbWeighDevicestatus { get; set; }
    }
}