using System;
using LightTown.Core.Domain.Projects;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LightTown.Core.Data;
using LightTown.Core.Domain.Messages;
using LightTown.Core.Domain.Tags;
using LightTown.Core.Domain.Users;
using LightTown.Server.Services.Tags;
using Microsoft.EntityFrameworkCore;

namespace LightTown.Server.Services.Projects
{
    public class ProjectService : IProjectService
    {
        private readonly IRepository<Project> _projectRepository;
        private readonly IRepository<ProjectMember> _projectMemberRepository;
        private readonly IRepository<ProjectTag> _projectTagRepository;
        private readonly IRepository<Tag> _tagRepository;
        private readonly IRepository<Message> _messageRepository;
        private readonly ITagService _tagService;

        public ProjectService(IRepository<Project> projectRepository, IRepository<ProjectMember> projectMemberRepository, IRepository<ProjectTag> projectTagRepository, IRepository<Tag> tagRepository, ITagService tagService, IRepository<Message> messageRepository)
        {
            _projectRepository = projectRepository;
            _projectMemberRepository = projectMemberRepository;
            _projectTagRepository = projectTagRepository;
            _tagRepository = tagRepository;
            _tagService = tagService;
            _messageRepository = messageRepository;
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
                CreationDateTime = DateTime.Now.Date,
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

        public async Task<bool> TryModifyProjectImage(int projectId, Stream fileStream, long? contentLength, string contentType)
        {
            if (contentLength < 1 || contentLength > 8000000)
                return false;

            if (contentType != "image/jpeg" && contentType != "image/png")
                return false;

            string extension = contentType == "image/jpeg" ? ".jpg" :
                contentType == "image/png" ? ".png" : null;

            if (extension == null)
                return false;

            var project = GetProject(projectId);

            if (project.HasImage)
                File.Delete(Path.Combine(Config.ProjectImagePath, $"{project.ImageFilename}"));

            using (var stream = File.Create(Path.Combine(Config.ProjectImagePath, $"{project.Id}{extension}")))
            {
                await fileStream.CopyToAsync(stream);
            }

            project.HasImage = true;
            project.ImageFilename = $"{project.Id}{extension}";

            PutProject(project);

            return true;
        }

        public bool TryGetProjectImage(string imageFilename, out byte[] imageBytes)
        {
            imageBytes = new byte[0];

            if (File.Exists(Path.Combine(Config.ProjectImagePath, imageFilename)))
            {
                imageBytes = File.ReadAllBytes(Path.Combine(Config.ProjectImagePath, imageFilename));

                return true;
            }

            return false;
        }

        public List<Tag> ModifyProjectTags(int projectId, List<Core.Models.Tags.Tag> tags)
        {
            var projectTags = _projectTagRepository.Table.Where(projectTag => projectTag.ProjectId == projectId)
                .Include(projectTag => projectTag.Tag).ToList();

            var removedTags = projectTags.Where(projectTag => tags.All(tag => tag.Id != projectTag.TagId)).ToList();

            var addedTags = tags.Where(tag => projectTags.All(projectTag => tag.Id != projectTag.TagId)).ToList();

            foreach (Core.Models.Tags.Tag tag in addedTags)
            {
                if (tag.Id == 0 || _tagRepository.GetById(tag.Id) == null)
                {
                    tag.Id = 0;
                    tag.Id = _tagService.InsertTag(tag).Id;
                }
            }

            _projectTagRepository.Delete(removedTags);

            var addedProjectTagEntities = addedTags.Select(tag => new ProjectTag
            {
                ProjectId = projectId,
                TagId = tag.Id
            });

            projectTags.RemoveAll(projectTag => removedTags.Contains(projectTag));

            var addedProjectTags = addedProjectTagEntities.Select(projectTag => _projectTagRepository.Insert(projectTag)).ToList();

            projectTags.AddRange(addedProjectTags);

            return projectTags.Select(projectTag => projectTag.Tag).ToList();
        }

        public IEnumerable<Message> GetMessages(int projectId)
        {
            return _messageRepository.TableNoTracking
                .Where(e => e.ProjectId == projectId).Include(e => e.MessageLikes);
        }
    }
}
