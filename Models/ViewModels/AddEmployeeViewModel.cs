using System.ComponentModel.DataAnnotations;

namespace minVagtPlan.Models.ViewModels
{
    public class AddEmployeeViewModel
    {
        [Required]
        public required string FirstName { get; set; }
        [Required]
        public required string LastName { get; set; }
        [Required]
        public required string Role { get; set; }
    }
}
