namespace IT_asserts.Dtos.Asset
{
    public class ValidateSerialNumbersDto
    {
        public Guid PurchaseId { get; set; }
        public List<string> SerialNumbers { get; set; } = new();
    }
}
