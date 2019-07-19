using System.ComponentModel.DataAnnotations;

namespace TimeTrackerApp.Client.Models
{
    public class ClientInputModel
    {
        [Required]
        public string Name { get; set; }
    }

}
