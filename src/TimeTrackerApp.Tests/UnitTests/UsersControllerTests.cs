using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTrackerApp.Controllers;
using TimeTrackerApp.Data;
using TimeTrackerApp.Domain;
using TimeTrackerApp.Models;
using Xunit;

namespace TimeTrackerApp.Tests.UnitTests
{
    public class UsersControllerTests
    {
        private readonly UsersController _controller;

        public UsersControllerTests()
        {
            var options = new DbContextOptionsBuilder<TimeTrackerDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())     // baza u memoriji sa random imenom
                .Options;       // opcije za db kontekst

            var dbContext = new TimeTrackerDbContext(options);      // kreiranje db kontext-a sa opcijama

            // dodavanje korisnika pjeske
            dbContext.Users.Add(new User { Id = 1, Name = "User 1", HourRate = 15 });
            dbContext.Users.Add(new User { Id = 2, Name = "User 2", HourRate = 20 });
            dbContext.Users.Add(new User { Id = 3, Name = "User 3", HourRate = 30 });
            dbContext.SaveChanges();

            var logger = new FakeLogger<UsersController>();

            _controller = new UsersController(dbContext, logger);
        }

        [Fact(Skip = "Does not work with EF Core 3 preview 6")]
        // testovi su uglavnom void
        public void GetById_IdDoesNotExist_ReturnsNotFoundResult()
        {
        //    Vec je inicijalizovan
        //    Arrange - podesavanje (SUT = System under test)
        //    var controller = new UsersController(null, new FakeLogger<UsersController>());

            // Act
            var result = _controller.GetById(0);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);   // jer GetById vraca task, pa moramo njegov result proslijediti
                                                            // ne radi u verziji 6 - vraca NullReference
        }

        [Fact]
        public async Task GetById_IdExists_ReturnCorrectResult()
        {
            // Arrange
            const string expectedName = "User 1";

            // Act
            var result = await _controller.GetById(1);

            // Assert
            Assert.IsType<ActionResult<UserModel>>(result);
            Assert.NotNull(result.Value);
            Assert.Equal(expectedName, result.Value.Name);
        }

        [Fact]
        public async Task GetPage_FirstPage_ReturnsExpectedResult()
        {
            // Arrange
            const int expectedCount = 3;
            const int expectedTotalCount = 3;

            // Act
            var result = await _controller.GetPage(page: 1, size: 3);

            // Assert
            Assert.IsType<ActionResult<PagedList<UserModel>>>(result);
            Assert.NotNull(result.Value);
            Assert.Equal(expectedCount, result.Value.Items.Count());
            Assert.Equal(expectedTotalCount, result.Value.TotalCount);
        }

        [Fact]
        public async Task GetPage_SecondtPage_ReturnsExpectedResult()
        {
            // Arrange
            const int expectedTotalCount = 3;

            // Act
            var result = await _controller.GetPage(page: 2, size: 3);

            // Assert
            Assert.IsType<ActionResult<PagedList<UserModel>>>(result);
            Assert.NotNull(result.Value);
            Assert.Empty(result.Value.Items);       // items bi trebali biti prazni (na drugoj strani nema nista)
            Assert.Equal(expectedTotalCount, result.Value.TotalCount);
        }

        // delete metod
        [Fact]
        public async Task Delete_IdExists_ReturnsOkResult()
        {
            // Arrange

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsType<OkResult>(result);
        }
    }
}
