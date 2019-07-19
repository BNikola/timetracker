using System;
using System.ComponentModel.DataAnnotations;

namespace TimeTrackerApp.Client.Models
{
    public class TimeEntryInputModel
    {
        [Required]
        public long ProjectId { get; set; }

        public long UserId { get; set; }

        public DateTime EntryDate { get; set; }

        [Range(1,24)]
        public int Hours { get; set; }

        public string Description { get; set; }
    }
}
