namespace minVagtPlan.Models.Entities
{
    public class ShiftEmployee
    {
        public Guid ShiftId { get; set; }
        public Shift? Shift { get; set; }

        public Guid EmployeeId { get; set; }
        public Employee? Employee { get; set; }
    }
}