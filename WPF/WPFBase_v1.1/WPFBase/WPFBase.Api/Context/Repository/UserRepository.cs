using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPFBase.Api.Context.Model;
using WPFBase.Api.Context.Model.SM;
using WPFBase.Api.Context.UnitOfWork;

namespace WPFBase.Api.Context.Repository
{
    public class OperatorRepository : Repository<Operator>, IRepository<Operator>
    {
        public OperatorRepository(BaseContext dbContext) : base(dbContext)
        {
        }
    }
}
