using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPFBase.Entities.BM;
using WPFBase.Api.Context.UnitOfWork;

namespace WPFBase.Api.Context.Repository
{
    public class TbWeighUsergroupRepository : Repository<tb_weigh_usergroup>, IRepository<tb_weigh_usergroup>
    {
        public TbWeighUsergroupRepository(BaseContext dbContext) : base(dbContext)
        {
        }
    }
}