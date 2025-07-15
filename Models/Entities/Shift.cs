namespace minVagtPlan.Models.Entities
{
    public class Shift
    {
        public Guid ShiftId { get; set; }
        public required DateTime StartTime { get; set; }

        public required DateTime EndTime { get; set; }

        public string Title
        {
            get
            {
                return $"{StartTime} {EndTime}";
            }
        }

        // Foreign key for Employee

        public ICollection<ShiftEmployee> ShiftEmployees { get; set; } = new List<ShiftEmployee>();
    }
}
