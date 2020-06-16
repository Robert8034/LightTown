using System.ComponentModel.DataAnnotations;

namespace LightTown.Server.Models.Users
{
    public class UserPost
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public int RoleId { get; set; }
    }
}