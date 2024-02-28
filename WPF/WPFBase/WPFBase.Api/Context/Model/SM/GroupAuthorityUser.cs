namespace WPFBase.Api.Context.Model.SM
{
    public class GroupAuthorityUser:EntityBase
    {
        public string UserGroupCode { get; set; }

        public string UserGroupName { get; set; }

        public string UserCode { get; set; }

        public int Status { get; set; }  
    }
}
