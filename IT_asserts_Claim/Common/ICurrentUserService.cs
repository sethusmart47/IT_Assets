namespace ITAssetManagement.Common;

public interface ICurrentUserService
{
    string UserId { get; }
    string UserName { get; }
}
