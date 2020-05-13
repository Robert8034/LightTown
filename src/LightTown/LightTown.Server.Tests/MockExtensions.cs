using System;
using System.Reflection;
using LightTown.Core.Data;
using LightTown.Server.Data;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.Protected;

namespace LightTown.Server.Tests
{
    public static class MockExtensions
    {
        public static void SetupRepositoryMock<T>(this Mock<Repository<T>> repositoryMock, Action<IRepository<T>> options) where T : BaseEntity
        {
            var context = new LightTownTestContext();

            var constructorArguments = repositoryMock.GetType().GetField("constructorArguments", 
                BindingFlags.NonPublic | BindingFlags.Instance);
            constructorArguments?.SetValue(repositoryMock, new object[]{ context });

            repositoryMock.Protected().SetupGet<DbSet<T>>("Entities").Returns(context.Set<T>());
            repositoryMock.Setup(repository => repository.Table).Returns(context.Set<T>());
            repositoryMock.Setup(repository => repository.TableNoTracking).Returns(context.Set<T>().AsNoTracking);

            options.Invoke(repositoryMock.Object);

            context.SaveChanges();
        }
    }
}
