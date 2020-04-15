using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LightTown.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        public ProjectsController()
        {

        }

        [HttpGet]
        [Route("/{projectId}/members")]
        public async Task<ApiResult> GetMembers(int projectId)
        {
            return ApiResult.NoContent();
        }

        [HttpPut]
        [Route("/{projectId}/members/{userId}")]
        public async Task<ApiResult> GetMember(int projectId, int userId)
        {
            return ApiResult.NoContent();
        }
    }
}
