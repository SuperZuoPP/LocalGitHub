using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPFBase.Api.Context.Model;
using WPFBase.Api.Context.Model.BM;
using WPFBase.Api.Context.UnitOfWork;

namespace WPFBase.Api.Context.Repository
{
    public class TbWeighOperatorRepository : Repository<TbWeighOperator>, IRepository<TbWeighOperator>
    {
        public TbWeighOperatorRepository(BaseContext dbContext) : base(dbContext)
        {
        }
    }
}
