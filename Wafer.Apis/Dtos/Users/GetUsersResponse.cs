using System.Collections.Generic;

namespace Wafer.Apis.Dtos.Users
{
    public class GetUsersResponse
    {
        public bool Success { get; set; }
        public List<UserInfoDto> Users { get; set; }
        public int TotalPageCount { get; set; }
    }
}
