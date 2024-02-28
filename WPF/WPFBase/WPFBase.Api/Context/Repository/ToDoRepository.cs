using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPFBase.Api.Context.Model;
using WPFBase.Api.Context.UnitOfWork;

namespace WPFBase.Api.Context.Repository
{
    public class ToDoRepository : Repository<ToDo>, IRepository<ToDo>
    {
        public ToDoRepository(BaseContext dbContext) : base(dbContext)
        {
        }
    }
}
