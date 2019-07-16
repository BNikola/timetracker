using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeTrackerApp.Domain;

namespace TimeTrackerApp.Models
{
    /// <summary>
    /// Represents one time tracker user.
    /// </summary>
    public class UserModel
    {
        private UserModel()         // Singleton
        {
        }

        /// <summary>
        /// Gets or sets user id.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets user name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets how much the user will be payed per hour.
        /// </summary>
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
