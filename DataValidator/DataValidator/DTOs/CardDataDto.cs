namespace DataValidator.DTOs
{
    public class CardDataDto
    {
        public string? Owner { get; set; }
        public long? Number { get; set; }
        public string? IssueDate { get; set; }
        public int? CVC { get; set; }
    }
}
