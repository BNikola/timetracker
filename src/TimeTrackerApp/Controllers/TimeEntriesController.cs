using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeTrackerApp.Data;
using TimeTrackerApp.Domain;
using TimeTrackerApp.Models;

namespace TimeTrackerApp.Controllers
{
    [ApiController]
    [Authorize]
    [Route("/api/time-entries/")]   // lowercase & snake case (sa crticom)
    public class TimeEntriesController : Controller
    {
        private readonly TimeTrackerDbContext _dbContext;
        private readonly ILogger _logger;

        public TimeEntriesController(TimeTrackerDbContext dbContext, ILogger<TimeEntriesController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        // get by id
        [HttpGet("{id}")]
        public async Task<ActionResult<TimeEntryModel>> GetById(long id)
        {
            _logger.LogInformation($"Getting a time entry with id: {id}");

            // ne puca, vraca null ako nema 
            var timeEntry = await _dbContext.TimeEntries
                .Include(x => x.Project)
                .Include(x => x.Project.Client)
                .Include(x => x.User)
                .SingleOrDefaultAsync(x => x.Id == id);

            if (timeEntry == null)
            {
                return NotFound();
            }

            return TimeEntryModel.FromTimeEntry(timeEntry);
        }

        [HttpGet]
        [Route("user/{userId}/{year}/{month}")]    // /time-entries/user/2/2019/7 - izgled url-a
        public async Task<ActionResult<TimeEntryModel[]>> GetByUserAndMonth(long userId, int year, int month)
        {
            _logger.LogInformation($"Getting all time entries for user with id: {userId} for month {year}-{month}");

            // filtriranje time-entries-a
            var startDate = new DateTime(year, month, 1);       // pocetak mjeseca
            var endDate = startDate.Date.AddMonths(1);          // dodati mjesec dana na start date -> end date je prvi u narednom mjesecu

            var timeEntries = await _dbContext.TimeEntries
                .Include(x => x.User)
                .Include(x => x.Project)
                .Include(x => x.Project.Client)
                .Where(x => x.User.Id == userId && x.EntryDate >= startDate && x.EntryDate < endDate)
                .OrderBy(x => x.EntryDate)
                .ToListAsync();

            return timeEntries
                .Select(TimeEntryModel.FromTimeEntry)
                .ToArray();

        }


        [HttpGet]
        public async Task<ActionResult<PagedList<TimeEntryModel>>> GetPage(int page = 1, int size = 5)
        {
            _logger.LogDebug($"Getting a page {page} of time entries with page size {size}");

            var timeEntries = await _dbContext.TimeEntries
                .Include(x => x.User)
                .Include(x => x.Project)
                .Include(x => x.Project.Client)
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync();

            return new PagedList<TimeEntryModel>
            {
                Items = timeEntries.Select(TimeEntryModel.FromTimeEntry),
                Page = page,
                PageSize = size,
                TotalCount = await _dbContext.TimeEntries.CountAsync()
            };
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            _logger.LogDebug($"Deleting time entries with id {id}");

            var timeEntry = await _dbContext.TimeEntries.FindAsync(id);

            if (timeEntry == null)
            {
                return NotFound();
            }

            _dbContext.TimeEntries.Remove(timeEntry);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult<TimeEntryModel>> Create(TimeEntryInputModel model)
        {
            _logger.LogDebug(
                $"Creating a new time entry for user {model.UserId}, project {model.ProjectId} and date {model.EntryDate}");

            var user = await _dbContext.Users.FindAsync(model.UserId);
            var project = await _dbContext.Projects
                .Include(x => x.Client) // Necessary for mapping to TimeEntryModel later
                .SingleOrDefaultAsync(x => x.Id == model.ProjectId);

            if (user == null || project == null)
            {
                return NotFound();
            }

            var timeEntry = new TimeEntry { User = user, Project = project, HourRate = user.HourRate };
            model.MapTo(timeEntry);

            await _dbContext.TimeEntries.AddAsync(timeEntry);
            await _dbContext.SaveChangesAsync();

            var resultModel = TimeEntryModel.FromTimeEntry(timeEntry);

            return CreatedAtAction(nameof(GetById), "TimeEntries", new { id = timeEntry.Id }, resultModel);
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<TimeEntryModel>> Update(long id, TimeEntryInputModel model)
        {
            _logger.LogDebug($"Updating time entry with id {id}");

            var timeEntry = await _dbContext.TimeEntries
                .Include(x => x.User)
                .Include(x => x.Project)
                .Include(x => x.Project.Client)
                .SingleOrDefaultAsync(x => x.Id == id);

            if (timeEntry == null)
            {
                return NotFound();
            }

            model.MapTo(timeEntry);     // ne mijenjamo klijenta i projekat

            _dbContext.TimeEntries.Update(timeEntry);
            await _dbContext.SaveChangesAsync();

            return TimeEntryModel.FromTimeEntry(timeEntry);
        }
    }
}