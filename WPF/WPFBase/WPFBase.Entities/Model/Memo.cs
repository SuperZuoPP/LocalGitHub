using WPFBase.Entities.SM;

namespace WPFBase.Api.Context.Model
{
    public class Memo : EntityBase
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public int Status { get; set; }

    }
}
