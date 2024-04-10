using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPFBase.Api.Context.Model.BM;
using WPFBase.Api.Context.UnitOfWork;

namespace WPFBase.Api.Context.Repository
{
    public class TbWeighDatalineinfoRepository : Repository<TbWeighDatalineinfo>, IRepository<TbWeighDatalineinfo>
    {
        public TbWeighDatalineinfoRepository(BaseContext dbContext) : base(dbContext)
        {
        }
    }
}