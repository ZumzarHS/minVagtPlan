using minVagtPlan.Areas.Identity.Data;

namespace minVagtPlan.Models.Entities
{
    public class Employee
    {
        public Guid EmployeeId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Role { get; set; }
        public string FullName
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }

        public ICollection<ShiftEmployee> ShiftEmployees { get; set; } = new List<ShiftEmployee>();

        // Foreign key for VagtPlanUser
        public string UserId { get; set; }
        public VagtPlanUser User { get; set; }
    }
}
