using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeTrackerApp.Domain;

namespace TimeTrackerApp.Models
{
    public class UserModel
    {
        private UserModel()         // Singleton
        {
        }
        public long Id { get; set; }
        public string Name { get; set; }
        public decimal HourRate { get; set; }

        public static UserModel FromUser(User user)         // factory metod, moze i constructor (mapiranje)
        {
            return new UserModel
            {
                Id = user.Id,
                Name = user.Name,
                HourRate = user.HourRate
            };
        }
    }
}
