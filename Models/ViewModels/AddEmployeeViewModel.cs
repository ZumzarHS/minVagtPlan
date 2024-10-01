namespace minVagtPlan.Models.ViewModels
{
    public class AddEmployeeViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
        public string FullName => $"{FirstName} {LastName}";

    }
}
