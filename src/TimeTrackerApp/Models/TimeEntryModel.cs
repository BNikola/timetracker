using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeTrackerApp.Domain;

namespace TimeTrackerApp.Models
{
    public class TimeEntryModel
    {
        private TimeEntryModel()
        {
        }
        public long Id { get; set; }

        public long ProjectId { get; set; }

        public string ProjectName { get; set; }

        public string ClientName { get; set; }

        public long UserId { get; set; }

        public string UserName { get; set; }

        public int Hours { get; set; }

        public DateTime EntryDate { get; set; }

        public string Description { get; set; }

        public decimal HourRate { get; set; }

        public decimal Total => Hours * HourRate;

        public static TimeEntryModel FromTimeEntry(TimeEntry timeEntry)
        {
            return new TimeEntryModel
            {
                Id = timeEntry.Id,
                ProjectId = timeEntry.Project.Id,
                ProjectName = timeEntry.Project.Name,
                ClientName = timeEntry.Project.Client.Name,
                UserId = timeEntry.User.Id,
                UserName = timeEntry.User.Name,
                EntryDate = timeEntry.EntryDate,
                Hours = timeEntry.Hours,
                Description = timeEntry.Description,
                HourRate = timeEntry.HourRate
            };
        }
    }
}
