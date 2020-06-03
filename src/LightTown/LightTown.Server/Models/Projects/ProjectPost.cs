using System.ComponentModel.DataAnnotations;

namespace LightTown.Server.Models.Projects
{
    public class ProjectPost
    {
        [Required]
        public string ProjectName { get; set; }
        public string ProjectDescription { get; set; }
    }
}
