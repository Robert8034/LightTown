using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using LightTown.Core.Domain.Projects;
using LightTown.Server.Data;
using LightTown.Server.Services.Projects;
using Moq;
using Xunit;

namespace LightTown.Server.Tests.Services.Projects
{
    public class ProjectMemberServiceTests
    {
        private readonly Mock<Repository<ProjectMember>> _projectMemberRepositoryMock;
        
        public ProjectMemberServiceTests()
        {
            _projectMemberRepositoryMock = new Mock<Repository<ProjectMember>>();
        }

        /// <summary>
        /// Test the GetProjectMember method and see if it returns the right ProjectMember object.
        /// </summary>
        [Fact]
        public void GetProjectMemberTest()
        {
            //Arrange
            ProjectMember projectMember1 = new ProjectMember { Id = 1, MemberId = 1, ProjectId = 1 };
            ProjectMember projectMember2 = new ProjectMember { Id = 2, MemberId = 2, ProjectId = 1 };

            _projectMemberRepositoryMock.SetupRepositoryMock(options =>
            {
                options.Insert(projectMember1); 
                options.Insert(projectMember2);
            });

            var projectMemberService = new ProjectMemberService(_projectMemberRepositoryMock.Object);

            //Act
            var actualProjectMember1 = projectMemberService.GetProjectMember(1, 1);

            //Assert
            Assert.Equal(projectMember1.Id, actualProjectMember1.Id);
            Assert.Equal(projectMember1.ProjectId, actualProjectMember1.ProjectId);
            Assert.Equal(projectMember1.MemberId, actualProjectMember1.MemberId);
        }

        /// <summary>
        /// Test the RemoveProjectMember method and see if it removes the right ProjectMember object.
        /// </summary>
        [Fact]
        public void RemoveProjectMemberTest()
        {
            //Arrange
            ProjectMember projectMember1 = new ProjectMember {Id = 1, MemberId = 1, ProjectId = 1};
            ProjectMember projectMember2 = new ProjectMember { Id = 2, MemberId = 2, ProjectId = 1 };

            _projectMemberRepositoryMock.SetupRepositoryMock(options =>
            {
                options.Insert(projectMember1);
                options.Insert(projectMember2);
            });

            var projectMemberService = new ProjectMemberService(_projectMemberRepositoryMock.Object);

            //Act
            projectMemberService.RemoveProjectMember(projectMember1);

            var actualProjectMember1 = projectMemberService.GetProjectMember(1, 1);
            var actualProjectMember2 = projectMemberService.GetProjectMember(1, 2);

            //Assert
            Assert.Null(actualProjectMember1);
            Assert.Equal(projectMember2.MemberId, actualProjectMember2.MemberId);
            Assert.Equal(projectMember2.ProjectId, actualProjectMember2.ProjectId);
            Assert.Equal(projectMember2.Id, actualProjectMember2.Id);
        }

        /// <summary>
        /// Test the CreateProjectMember method and see if it successfully inserts the right ProjectMember object.
        /// </summary>
        [Fact]
        public void CreateProjectMemberTest()
        {
            //Arrange
            ProjectMember projectMember1 = new ProjectMember {Id = 1, ProjectId = 1, MemberId = 1};

            _projectMemberRepositoryMock.SetupRepositoryMock(options => { });

            var projectMemberService = new ProjectMemberService(_projectMemberRepositoryMock.Object);

            //Act
            projectMemberService.CreateProjectMember(1, 1);

            var actualProjectMember = projectMemberService.GetProjectMember(1, 1);

            //Assert
            Assert.Equal(projectMember1.Id, actualProjectMember.Id);
            Assert.Equal(projectMember1.ProjectId, actualProjectMember.ProjectId);
            Assert.Equal(projectMember1.MemberId, actualProjectMember.MemberId);

        }
    }
}
