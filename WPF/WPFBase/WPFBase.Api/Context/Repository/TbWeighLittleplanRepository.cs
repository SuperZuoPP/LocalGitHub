using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPFBase.Entities.BM;
using WPFBase.Api.Context.UnitOfWork;

namespace WPFBase.Api.Context.Repository
{
    public class TbWeighLittleplanRepository : Repository<tb_weigh_littleplan>, IRepository<tb_weigh_littleplan>
    {
        public TbWeighLittleplanRepository(BaseContext dbContext) : base(dbContext)
        {
        }
    }
}