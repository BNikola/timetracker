using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TimeTrackerApp.Data;
using TimeTrackerApp.Models;

namespace TimeTrackerApp.Controllers
{
    [ApiController]         // pravimo api kontrolere
    [Route("/api/users")]   // rutiranje
    public class UsersController : Controller
    {
        private readonly TimeTrackerDbContext _dbContext;
        private readonly ILogger<UsersController> _logger;

        // using dependency injection
        public UsersController(TimeTrackerDbContext dbContext, ILogger<UsersController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        [HttpGet("{id}")]       // definisemo da koristi osnovni rout users i doda {id}
                                // return a strongly typed model, asinhrono
        public async Task<ActionResult<UserModel>> GetById(long id)
        {
            _logger.LogInformation($"Getting user by id: {id}");
            var user = await _dbContext.Users.FindAsync(id);

            if (user == null)
            {
                _logger.LogWarning($"User with id: {id} not found!");
                return NotFound();
            }

            return UserModel.FromUser(user);
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<UserModel>>> GetPage(int page = 1, int size = 5)
        {
            _logger.LogInformation($"Getting a page {page} of users with page size {size}");

            var users = await _dbContext.Users
                .Skip((page - 1) * size) // preskacemo neke stranice
                .Take(size)
                .ToListAsync();

            var totalUsers = await _dbContext.Users.CountAsync();

            // ne vraca null nego praznu listu
            return new PagedList<UserModel>
            {
                Items = users.Select(UserModel.FromUser),
                Page = page,
                PageSize = size,
                TotalCount = totalUsers
            };
        }
    }
}