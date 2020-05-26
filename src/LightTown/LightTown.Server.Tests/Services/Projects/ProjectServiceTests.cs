using LightTown.Core.Domain.Projects;
using LightTown.Server.Data;
using LightTown.Server.Services.Projects;
using Microsoft.Extensions.Options;
using Moq;
using System.Collections.Generic;
using System.Linq;
using LightTown.Core.Domain.Tags;
using Xunit;

namespace LightTown.Server.Tests.Services.Projects
{
    //TODO add other tests here
    public class ProjectServiceTests
    {
        private readonly Mock<Repository<Project>> _projectRepositoryMock;
        private readonly Mock<Repository<ProjectMember>> _projectMemberRepositoryMock;
        private readonly Mock<Repository<Tag>> _tagRepositoryMock;
        private readonly Mock<Repository<ProjectTag>> _projectTagRepositoryMock;

        public ProjectServiceTests()
        {
            _projectRepositoryMock = new Mock<Repository<Project>>();
            _projectMemberRepositoryMock = new Mock<Repository<ProjectMember>>();
            _tagRepositoryMock = new Mock<Repository<Tag>>();
            _projectTagRepositoryMock = new Mock<Repository<ProjectTag>>();
        }

        /// <summary>
        /// Test the GetProject method and see if it returns the right Project object.
        /// </summary>
        [Fact]
        public void GetProjectTest()
        {
            //Arrange
            Project project1 = new Project {Id = 1, ProjectName = "Project1"};
            Project project2 = new Project {Id = 2, ProjectName = "Project2"};
            Project project3 = new Project {Id = 3, ProjectName = "Project3"};

            _projectRepositoryMock.SetupRepositoryMock(options =>
            {
                options.Insert(project1);
                options.Insert(project2);
                options.Insert(project3);
            });
            
            var projectService = new ProjectService(_projectRepositoryMock.Object, null);

            //Act
            var actualProject = projectService.GetProject(2);

            //Assert
            Assert.Equal(project2.Id, actualProject.Id);
            Assert.Equal(project2.ProjectName, actualProject.ProjectName);
        }

        /// <summary>
        /// Test the GetProjects method and see if it returns all the projects in the right order.
        /// </summary>
        [Fact]
        public void GetProjectsTest()
        {
            //Arrange
            Project project1 = new Project { Id = 1, ProjectName = "Project1" };
            Project project2 = new Project { Id = 2, ProjectName = "Project2" };
            Project project3 = new Project { Id = 3, ProjectName = "Project3" };

            _projectRepositoryMock.SetupRepositoryMock(options =>
            {
                options.Insert(project1);
                options.Insert(project2);
                options.Insert(project3);
            });

            List<Project> projects = new List<Project>
            {
                project1,
                project2,
                project3
            };

            var projectService = new ProjectService(_projectRepositoryMock.Object, null);

            //Act
            List<Project> actualProjects = projectService.GetProjects().ToList();

            //Assert
            for (int i = 0; i < projects.Count; i++)
            {
                Assert.Equal(projects[i].Id, actualProjects[i].Id);
                Assert.Equal(projects[i].ProjectName, actualProjects[i].ProjectName);
            }
        }

        /// <summary>
        /// Test the GetProjects method and see if it returns all the projects in the right order.
        /// </summary>
        [Fact]
        public void CreateProjectTest()
        {
            //Arrange
            Project project = new Project {Id = 1, ProjectName = "Project1", ProjectDescription = "ProjectDescription", CreatorId = 1};

            _projectRepositoryMock.SetupRepositoryMock(options =>
            {

            });

            var projectService = new ProjectService(_projectRepositoryMock.Object, null);

            //Act
            var actualProject = projectService.CreateProject("Project1", "ProjectDescription", 1);

            //Assert
            Assert.Equal(project.ProjectName, actualProject.ProjectName);
            Assert.Equal(project.ProjectDescription, actualProject.ProjectDescription);
            Assert.Equal(project.CreatorId, actualProject.CreatorId);

        }

        /// <summary>
        /// Test the GetProjectsWithTagIdsAndMemberCount method and see if it returns the correct Projects with its Tag Ids and the correct MemberCount
        /// </summary>
        /*[Fact]
        public void GetProjectTagIdsAndMemberCountTest()
        {
            //Arrange
            Project project = new Project { Id = 1, ProjectName = "Project1", ProjectDescription = "ProjectDescription", CreatorId = 1 };
            ProjectMember projectMember1 = new ProjectMember { Id = 1, MemberId = 1, ProjectId = 1 };
            ProjectMember projectMember2 = new ProjectMember { Id = 2, MemberId = 2, ProjectId = 1 };
            Tag tag1 = new Tag { Id = 1, Name = "Tag1" };
            Tag tag2 = new Tag { Id = 2, Name = "Tag2" };
            Tag tag3 = new Tag { Id = 3, Name = "Tag3" };
            ProjectTag projectTag1 = new ProjectTag {Id = 1, ProjectId = 1, TagId = 1};
            ProjectTag projectTag2 = new ProjectTag { Id = 2, ProjectId = 1, TagId = 2 };
            ProjectTag projectTag3 = new ProjectTag { Id = 3, ProjectId = 1, TagId = 3 };

            _projectRepositoryMock.SetupRepositoryMock(options =>
            {
                options.Insert(project);
            });

            _projectMemberRepositoryMock.SetupRepositoryMock(options =>
            {
                options.Insert(projectMember1);
                options.Insert(projectMember2);
            });

            _tagRepositoryMock.SetupRepositoryMock(options =>
            {
                options.Insert(tag1);
                options.Insert(tag2);
                options.Insert(tag3);
            });

            _projectTagRepositoryMock.SetupRepositoryMock(options =>
            {
                options.Insert(projectTag1);
                options.Insert(projectTag2);
                options.Insert(projectTag3);
            });

            var projectService = new ProjectService(_projectRepositoryMock.Object, _projectMemberRepositoryMock.Object);

            //Act
            var actualProjects = projectService.GetProjectsWithTagIdsAndMemberCount(1);

            //Assert

        }*/
    }
}
