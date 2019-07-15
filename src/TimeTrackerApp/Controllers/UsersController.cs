using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TimeTrackerApp.Data;
using TimeTrackerApp.Models;

namespace TimeTrackerApp.Controllers
{
    [ApiController]         // pravimo api kontrolere
    [Authorize]
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

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        // delete, not returning anything (ne treba preko get)
        public async Task<IActionResult> Delete(long id)
        {
            _logger.LogInformation($"Deleting user with id: {id}");

            var user = await _dbContext.Users.FindAsync(id);
            
            if (user == null)
            {
                _logger.LogWarning($"No user found with id: {id}");
                return NotFound();
            }

            _dbContext.Users.Remove(user);          // samo markira user-a za brisanje
            await _dbContext.SaveChangesAsync();    // unit of work 

            return Ok();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]      // post radi insert, put radi update
        public async Task<ActionResult<UserModel>> Create(UserInputModel model)
        {
            _logger.LogInformation($"Creating a new user with name: {model.Name}");

            var user = new Domain.User();
            model.MapTo(user);

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            var resultModel = UserModel.FromUser(user);

            return CreatedAtAction(nameof(GetById), "users", new { id = user.Id }, resultModel);    // prva tri arg. definisu putanju a posljednji cijelog user-a (mora se vratiti i url)
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<UserModel>> Update(long id, UserInputModel model)
        {
            _logger.LogInformation($"Updating a user with id: {id}");

            var user = await _dbContext.Users.FindAsync(id);

            if (user == null)
            {
                _logger.LogWarning($"No user found with id: {id}");
                return NotFound();
            }

            model.MapTo(user);

            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();

            return UserModel.FromUser(user);
        }
    }
}