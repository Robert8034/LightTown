using LightTown.Core.Domain.Projects;
using LightTown.Server.Data;
using LightTown.Server.Services.Projects;
using Microsoft.Extensions.Options;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace LightTown.Server.Tests.Services.Projects
{
    //TODO add other tests here
    public class ProjectServiceTests
    {
        private readonly Mock<Repository<Project>> _projectRepositoryMock;

        public ProjectServiceTests()
        {
            _projectRepositoryMock = new Mock<Repository<Project>>();
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
    }
}
