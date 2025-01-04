using System.ComponentModel.DataAnnotations;

namespace AspNetcoreWebApp.Models
{
    public class Container
    {
        [Required]
        public string Name { get; set; }
    }
}
