using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPFBase.Entities.BM;
using WPFBase.Api.Context.UnitOfWork;

namespace WPFBase.Api.Context.Repository
{
    public class TbWeighGroupauthorityuserRepository : Repository<tb_weigh_groupauthorityusers>, IRepository<tb_weigh_groupauthorityusers>
    {
        public TbWeighGroupauthorityuserRepository(BaseContext dbContext) : base(dbContext)
        {
        }
    }
}