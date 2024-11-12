using WPFBase.Api.Context.Model;
using WPFBase.Entities.BM;
using WPFBase.Api.Context.UnitOfWork;

namespace WPFBase.Api.Context.Repository
{
    
    public class TbWeighMenuRepository : Repository<tb_weigh_menu>, IRepository<tb_weigh_menu>
    {
        public TbWeighMenuRepository(BaseContext dbContext) : base(dbContext)
        {
        }
    }
}
