namespace ITAssetManagement.Dtos.Asset
{
    public class AssetValidationResult
    {
        public List<string> ValidSerials { get; set; } = new();
        public List<AssetValidationError> Errors { get; set; } = new();
        public bool IsValid => Errors.Count == 0;
    }
    public class AssetValidationError
    {
        public int Index { get; set; }
        public string SerialNumber { get; set; } = string.Empty;
        public string Error { get; set; } = string.Empty;
    }
}
