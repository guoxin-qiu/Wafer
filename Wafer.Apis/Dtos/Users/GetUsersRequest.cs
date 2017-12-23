namespace Wafer.Apis.Dtos.Users
{
    public class GetUsersRequest
    {
        public string SearchText { get; set; }
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
        public string SortCol { get; set; }
        public string SortOrder { get; set; }
    }
}
