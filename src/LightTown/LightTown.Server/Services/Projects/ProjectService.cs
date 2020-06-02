using System;
using LightTown.Core.Domain.Projects;
using System.Collections.Generic;
using System.Linq;
using LightTown.Core.Data;
using LightTown.Core.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace LightTown.Server.Services.Projects
{
    public class ProjectService : IProjectService
    {
        private readonly IRepository<Project> _projectRepository;
        private readonly IRepository<ProjectMember> _projectMemberRepository;

        public ProjectService(IRepository<Project> projectRepository, IRepository<ProjectMember> projectMemberRepository)
        {
            _projectRepository = projectRepository;
            _projectMemberRepository = projectMemberRepository;
        }

        public IEnumerable<Project> GetProjects()
        {
            var projects = _projectRepository.TableNoTracking.ToList();

            return projects;
        }

        public IEnumerable<(Project, int, IEnumerable<int>)> GetProjectsWithTagIdsAndMemberCount(int? userIdFilter)
        {
            IQueryable<Project> queryable;

            if (userIdFilter.HasValue)
            {
                queryable = _projectRepository.TableNoTracking
                    .Where(project => project.ProjectMembers.Any(projectMember => 
                        projectMember.ProjectId == project.Id && projectMember.MemberId == userIdFilter));
            }
            else
            {
                queryable = _projectRepository.TableNoTracking;
            }

            var projects = queryable
                .Select(project => new Tuple<Project, int, IEnumerable<int>>(project, 
                    project.ProjectMembers.Count(projectMember => projectMember.ProjectId == project.Id), 
                    project.ProjectTags.Select(projectTag => projectTag.TagId)).ToValueTuple())
                .ToList();

            return projects;
        }

        public Project CreateProject(string projectName, string projectDescription, int creatorId)
        {
            Project project = new Project
            {
                ProjectName = projectName,
                ProjectDescription = projectDescription ?? "",
                CreationDateTime = DateTime.Now,
                CreatorId = creatorId,
                ProjectMembers = new List<ProjectMember>(new []
                {
                    new ProjectMember
                    {
                        MemberId = creatorId
                    }
                })
            };

            return _projectRepository.Insert(project);
        }

        public Project GetProject(int projectId)
        {
            var project = _projectRepository.TableNoTracking.SingleOrDefault(e => e.Id == projectId);

            return project;
        }

        public IEnumerable<ProjectMember> GetProjectMembers(int projectId)
        {
            return _projectMemberRepository.Table.Where(e => e.ProjectId == projectId).Include(e => e.Member).ToList();
        }

        public bool PutProject(Project project)
        {
            _projectRepository.Update(project);

            return true;
        }

        public bool ProjectExists(int projectId)
        {
            return _projectRepository.Table.Any(e => e.Id == projectId);
        }

        public IEnumerable<User> GetMembers(int projectId)
        {
            return _projectMemberRepository.TableNoTracking
                .Where(projectMember => projectMember.ProjectId == projectId)
                .Select(projectMember => projectMember.Member)
                .ToList();
        }

        public bool UserIsMember(int projectId, int userId)
        {
            return _projectMemberRepository.TableNoTracking.Any(e => e.MemberId == userId && e.ProjectId == projectId);
        }

        public List<Project> SearchProjects(string searchValue)
        {
            if (_projectRepository.TableNoTracking.Any(e => EF.Functions.Like(e.ProjectName, $"%{searchValue}%")))
            {
                return _projectRepository.TableNoTracking.Where(e => EF.Functions.Like(e.ProjectName, $"%{searchValue}%")).ToList();
            }
            return new List<Project>();
        }
    }
}
