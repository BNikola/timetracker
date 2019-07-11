using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTrackerApp.Domain
{
    public class Project
    {
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public Client Client { get; set; }
    }
}
