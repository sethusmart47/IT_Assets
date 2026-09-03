namespace ITAssetManagement.Common;

/// <summary>
/// Resolves the current user. Defaults to "System" until JWT auth is wired in.
/// </summary>
public class CurrentUserService : ICurrentUserService
{
    public string UserId => "System";
    public string UserName => "System";
}
