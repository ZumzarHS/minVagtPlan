using System.ComponentModel.DataAnnotations;


namespace minVagtPlan.Models.ViewModels
{
    public class EditEmployeeViewModel
    {
        public Guid EmployeeId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Role { get; set; }
    }
}
