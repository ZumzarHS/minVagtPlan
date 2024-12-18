﻿namespace minVagtPlan.Models.Entities
{
    public class Employee
    {
        public Guid EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
        public string FullName
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }

        public ICollection<ShiftEmployee> ShiftEmployees { get; set; } = new List<ShiftEmployee>();

    }
}
