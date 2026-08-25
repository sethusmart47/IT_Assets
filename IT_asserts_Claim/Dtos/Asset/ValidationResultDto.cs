namespace IT_asserts.Dtos.Asset
{
    public class ValidationResultDto
    {
        public List<string> ValidSerials { get; set; } = new();
        public List<ValidationError> Errors { get; set; } = new();
        public bool IsValid => Errors.Count == 0;
    }
    public class ValidationError
    {
        public int Index { get; set; }
        public string SerialNumber { get; set; } = string.Empty;
        public string Error { get; set; } = string.Empty;
    }
}
