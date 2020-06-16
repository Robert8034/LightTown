using System.ComponentModel.DataAnnotations;

namespace LightTown.Server.Models.Invites
{
    public class InvitePost
    {
        [Required]
        public string Password { get; set; }
    }
}