using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPFBase.Api.Context.Model;
using WPFBase.Entities.BM;
using WPFBase.Api.Context.UnitOfWork;

namespace WPFBase.Api.Context.Repository
{
    public class TbWeighOperatorRepository : Repository<tb_weigh_operator>, IRepository<tb_weigh_operator>
    {
        public TbWeighOperatorRepository(BaseContext dbContext) : base(dbContext)
        {
        }
    }
}
