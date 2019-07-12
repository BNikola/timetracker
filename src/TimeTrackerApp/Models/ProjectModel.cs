using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTrackerApp.Models
{
    public class ProjectModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public long ClientId { get; set; }

        public string ClientName { get; set; }
    }
}
