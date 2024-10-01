namespace minVagtPlan.Models.Entities
{
    public class Employee
    {
        public Guid EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
        public string FullName => $"{FirstName} {LastName}";

        //// Navigation property to link to User
        //public string UserId { get; set; } // Foreign key to User
        //public virtual User User { get; set; } // Navigation property

        //public virtual ICollection<Shift> Shift { get; set; }
    }
}
