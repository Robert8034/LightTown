using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using LightTown.Core.Data;
using LightTown.Core.Domain.Users;
using LightTown.Server.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using Npgsql.Logging;

namespace LightTown.Server.Tests
{
    public static class MockExtensions
    {
        public static void SetupRepositoryMock<T>(this Mock<Repository<T>> repositoryMock, Action<IRepository<T>> options) where T : BaseEntity
        {
            var context = new LightTownTestContext();
            context.Database.EnsureDeleted();

            var constructorArguments = repositoryMock.GetType().GetField("constructorArguments", 
                BindingFlags.NonPublic | BindingFlags.Instance);
            constructorArguments?.SetValue(repositoryMock, new object[]{ context });

            repositoryMock.Protected().SetupGet<DbSet<T>>("Entities").Returns(context.Set<T>());
            repositoryMock.Setup(repository => repository.Table).Returns(context.Set<T>());
            repositoryMock.Setup(repository => repository.TableNoTracking).Returns(context.Set<T>().AsNoTracking);

            options.Invoke(repositoryMock.Object);

            context.SaveChanges();
        }

        public static void SetupRepositoryMock<T>(this Mock<Repository<T>> repositoryMock, DbContext context, Action<IRepository<T>> options) where T : BaseEntity
        {
            var constructorArguments = repositoryMock.GetType().GetField("constructorArguments",
                BindingFlags.NonPublic | BindingFlags.Instance);
            constructorArguments?.SetValue(repositoryMock, new object[] { context });

            repositoryMock.Protected().SetupGet<DbSet<T>>("Entities").Returns(context.Set<T>());
            repositoryMock.Setup(repository => repository.Table).Returns(context.Set<T>());
            repositoryMock.Setup(repository => repository.TableNoTracking).Returns(context.Set<T>().AsNoTracking);

            options.Invoke(repositoryMock.Object);

            context.SaveChanges();
        }

        public static void SetupUserManagerMock<TUser, TRole, TContext>(this Mock<UserManager<TUser>> userManagerMock, Action<UserManager<TUser>> options) where TUser : IdentityUser<int> where TRole : IdentityRole<int> where TContext : DbContext
        {
            TContext context = Activator.CreateInstance<TContext>();
            context.Database.EnsureDeleted();

            var storeMock = new Mock<UserStore<TUser, TRole, TContext, int>>(new object[]{ context });

            var storeObj = storeMock.Object;


            var use = new UserStore<TUser, TRole, TContext, int>(context);

            storeMock.Setup(store => store.CreateAsync(It.IsAny<TUser>(), It.IsAny<CancellationToken>()))
                .CallBase();

            var constructorArguments = userManagerMock.GetType().GetField("constructorArguments",
                BindingFlags.NonPublic | BindingFlags.Instance);
            constructorArguments?.SetValue(userManagerMock, new object[] { storeMock.Object, null, null, null, null, null, null, null, null });

            userManagerMock.Object.UserValidators.Add(new UserValidator<TUser>());
            userManagerMock.Object.PasswordValidators.Add(new PasswordValidator<TUser>());

            userManagerMock.Setup(userManager => userManager.CreateAsync(It.IsAny<TUser>(), It.IsAny<string>()))
                .CallBase();
            //userManagerMock.Setup(x => x.UpdateAsync(It.IsAny<T>())).ReturnsAsync(IdentityResult.Success);

            options.Invoke(userManagerMock.Object);
        }
    }
}
