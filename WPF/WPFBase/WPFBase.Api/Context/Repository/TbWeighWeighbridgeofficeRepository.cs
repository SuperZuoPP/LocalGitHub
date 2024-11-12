using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPFBase.Entities.BM;
using WPFBase.Api.Context.UnitOfWork;

namespace WPFBase.Api.Context.Repository
{
    public class TbWeighWeighbridgeofficeRepository : Repository<tb_weigh_weighbridgeoffice>, IRepository<tb_weigh_weighbridgeoffice>
    {
        public TbWeighWeighbridgeofficeRepository(BaseContext dbContext) : base(dbContext)
        {
        }
    }
}