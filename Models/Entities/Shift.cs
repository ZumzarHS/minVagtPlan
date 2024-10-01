namespace minVagtPlan.Models.Entities
{
    public class Shift
    {
        public Guid ShiftId { get; set; }
        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
        public Guid EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
