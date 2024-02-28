using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPFBase.Api.Context.Model;
using WPFBase.Api.Context.UnitOfWork;

namespace WPFBase.Api.Context.Repository
{
    public class UserRepository : Repository<User>, IRepository<User>
    {
        public UserRepository(BaseContext dbContext) : base(dbContext)
        {
        }
    }
}
