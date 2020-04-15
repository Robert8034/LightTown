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
        public async Task<ApiResult> PutMember(int projectId, int userId)
        {
            return ApiResult.NoContent();
        }

        [HttpGet]
        [Route("")]
        public async Task<ApiResult> GetProjects()
        {
            return ApiResult.NoContent();
        }

        [HttpGet]
        [Route("{projectId}")]
        public async Task<ApiResult> GetProject(int projectId)
        {
            return ApiResult.NoContent();
        }

        [HttpPost]
        [Route("{projectId}")]
        public async Task<ApiResult> PostProject(int projectId)
        {
            return ApiResult.NoContent();
        }
    }
}
