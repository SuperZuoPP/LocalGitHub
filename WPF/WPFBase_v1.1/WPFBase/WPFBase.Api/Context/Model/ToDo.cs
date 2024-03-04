using WPFBase.Api.Context.Model.SM;

namespace WPFBase.Api.Context.Model
{
    public class ToDo : EntityBase
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public int? Status { get; set; }

    }
}
