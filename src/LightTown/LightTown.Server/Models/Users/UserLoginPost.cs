namespace LightTown.Server.Models.Users
{
    public class UserLoginPost
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}