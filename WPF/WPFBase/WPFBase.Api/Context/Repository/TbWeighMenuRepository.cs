using WPFBase.Api.Context.Model;
using WPFBase.Api.Context.Model.BM;
using WPFBase.Api.Context.UnitOfWork;

namespace WPFBase.Api.Context.Repository
{
    
    public class TbWeighMenuRepository : Repository<TbWeighMenu>, IRepository<TbWeighMenu>
    {
        public TbWeighMenuRepository(BaseContext dbContext) : base(dbContext)
        {
        }
    }
}
