using WPFBase.Entities.SM;

namespace WPFBase.Api.Context.Model
{
    public class User : EntityBase
    {
        public string Account { get; set; }

        public string UserName { get; set; }

        public string PassWord { get; set; }
    }
}
