﻿using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using WPFBase.Api.Context.Model;
using WPFBase.Api.Context.Model.BM;
using WPFBase.Api.Context.Model.SM;

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

        public DbSet<TbWeighOperator> TbWeighOperator { get; set; }

        public DbSet<TbWeighUsergroup> TbWeighUsergroup { get; set; } 
        public DbSet<TbWeighGroupauthorityuser> TbWeighGroupauthorityuser { get; set; } 
        public DbSet<TbWeighMenu> TbWeighMenu { get; set; }

        public DbSet<TbWeighGroupauthority> TbWeighGroupauthority { get; set; }

        public DbSet<TbWeighDatalineinfo> TbWeighDatalineinfo { get; set; }

        public DbSet<TbWeighLittleplan> TbWeighLittleplan { get; set; }

        public DbSet<TbWeighPlan> TbWeighPlan { get; set; }

        public DbSet<TbWeighWeighbridgeoffice> TbWeighOffice { get; set; }

        public DbSet<TbWeighVideo> TbWeighVideo { get; set; }

        public DbSet<TbWeighDevicestatus> TbWeighDevicestatus { get; set; }
    }
}