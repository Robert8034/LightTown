using System;
using System.Collections.Generic;
using System.Linq;
using LightTown.Core.Data;
using LightTown.Core.Domain.Projects;
using LightTown.Server.Data;
using LightTown.Server.Services.Projects;
using Moq;
using Xunit;

namespace LightTown.Server.Tests
{
    public class ProjectServiceTests
    {
        private Mock<Repository<Project>> projectRepositoryMock;
        public ProjectServiceTests()
        {
            projectRepositoryMock = new Mock<Repository<Project>>();
        }

        [Fact]
        public void Test1()
        {
            //Arrange

            var queryable = new List<Project>
            {
                new Project {Id = 1, ProjectName = "Project1"}, new Project {Id = 2, ProjectName = "Project2"}
            };

            var repoMock = new Mock<IRepository<Project>>();

            //repoMock.Setup(x => x.TableNoTracking.SingleOrDefault(e => e.Id == 1)).Returns(mockProjects[0]);
            //repoMock.Setup(x => x.TableNoTracking.Where(e => e.Id == 1)).Returns(mockProjects[0]);

            var projectService = new ProjectService(repoMock.Object, null, null);

            //Act
            var actualProject = projectService.GetProject(1);

            //Assert
            Assert.Equal(queryable.ToList()[1].Id, actualProject.Id);
        }
    }
}
