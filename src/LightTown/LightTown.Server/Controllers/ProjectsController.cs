using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LightTown.Core.Domain.Roles;
using LightTown.Server.Services.Projects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace LightTown.Server.Controllers
{
    [Controller]
    [Route("[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet]
        [Route("images/{imageFilename}")]
        [Authorization(Permissions.NONE)]
        public IActionResult GetProjectImage(string imageFilename)
        {
            if (_projectService.TryGetProjectImage(imageFilename, out byte[] imageBytes))
            {
                if (!new FileExtensionContentTypeProvider().TryGetContentType(imageFilename, out string contentType))
                    contentType = "application/octet-stream";

                return new FileContentResult(imageBytes, contentType);
            }

            return new NotFoundResult();
        }
    }
}
