using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTrackerApp.Domain
{
    public class User
    {
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        public decimal HourRate { get; set; }
    }
}
