using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTrackerApp.Client.Models
{
    public class UserModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public decimal HourRate { get; set; }
    }
}
