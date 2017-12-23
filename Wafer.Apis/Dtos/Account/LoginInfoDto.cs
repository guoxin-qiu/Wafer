namespace Wafer.Apis.Dtos.Account
{
    public class LoginInfoDto
    {
        public bool LoginSuccess { get; set; }
        public string Token { get; set; }
        public LoginUserInfoDto UserInfo { get; set; }
    }

    public class LoginUserInfoDto
    {
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
    }
}
