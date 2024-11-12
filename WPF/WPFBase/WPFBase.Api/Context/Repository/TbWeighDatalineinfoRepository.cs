using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPFBase.Entities.BM;
using WPFBase.Api.Context.UnitOfWork;

namespace WPFBase.Api.Context.Repository
{
    public class TbWeighDatalineinfoRepository : Repository<tb_weigh_datalineinfo>, IRepository<tb_weigh_datalineinfo>
    {
        public TbWeighDatalineinfoRepository(BaseContext dbContext) : base(dbContext)
        {
        }
    }
}