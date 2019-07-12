using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTrackerApp.Models
{
    public class TimeEntryModel
    {
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
    }
}
