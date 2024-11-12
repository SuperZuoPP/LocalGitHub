using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPFBase.Entities.BM;
using WPFBase.Api.Context.UnitOfWork;

namespace WPFBase.Api.Context.Repository
{
    public class TbWeighPlanRepository : Repository<tb_weigh_plan>, IRepository<tb_weigh_plan>
    {
        public TbWeighPlanRepository(BaseContext dbContext) : base(dbContext)
        {
        }
    }
}