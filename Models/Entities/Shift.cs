namespace minVagtPlan.Models.Entities
{
    public class Shift
    {
        public Guid ShiftId { get; set; }
        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
        // Foreign key for Employee

        public ICollection<ShiftEmployee> ShiftEmployees { get; set; } = new List<ShiftEmployee>();
    }
}
