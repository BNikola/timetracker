using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeTrackerApp.Domain;

namespace TimeTrackerApp.Models
{
    public class ClientInputModel
    {
        public string Name { get; set; }

        public void MapTo(Client client)
        {
            client.Name = Name;     // zbog konvencije
        }
    }
}
