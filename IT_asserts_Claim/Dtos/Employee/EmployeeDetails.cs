namespace ITAssetManagement.Dtos.Employee
{
    public class EmployeeDetails
    {
        public Guid Id { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string Designation { get; set; } = string.Empty;
        public int ActiveAssetCount { get; set; }
    }
}
