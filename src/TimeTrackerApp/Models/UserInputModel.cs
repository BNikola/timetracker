using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeTrackerApp.Domain;

namespace TimeTrackerApp.Models
{
    public class UserInputModel
    {
        public string Name { get; set; }
        public decimal HourRate { get; set; }

        public void MapTo(User user)
        {
            user.Name = Name;
            user.HourRate = HourRate;
        }
    }
}
