namespace expensetrackertry.DTO
{
    public class ExpenseDTO
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Category { get; set; }

        public string? Description { get; set; }

        public float? Price { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
