using LightTown.Core.Domain.Projects;
using LightTown.Server.Data;
using LightTown.Server.Services.Projects;
using Moq;
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
    }
}
