namespace DataValidator.DTOs
{
    public class ValidationResponse
    {
        public IDictionary<string, string> Errors { get; set; } = new Dictionary<string, string>();
        public CardType CardType { get; set; }
    }
}
