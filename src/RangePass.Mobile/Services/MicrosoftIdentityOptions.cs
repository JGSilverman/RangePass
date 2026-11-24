using System.Text.Json;

namespace RangePass.Mobile.Services;

public sealed class MicrosoftIdentityOptions
{
    public string Authority { get; init; } = "https://login.microsoftonline.com/common";
    public string ClientId { get; init; } = "YOUR_CLIENT_ID";
    public string RedirectUri { get; init; } = "rangepass://auth";
    public string[] Scopes { get; init; } = ["openid", "profile", "offline_access"];

    public bool IsConfigured =>
        !string.IsNullOrWhiteSpace(ClientId) &&
        !ClientId.StartsWith("YOUR_", StringComparison.OrdinalIgnoreCase);

    public static MicrosoftIdentityOptions Load()
    {
        using var stream = FileSystem.OpenAppPackageFileAsync("microsoft-identity.json")
            .GetAwaiter()
            .GetResult();

        return JsonSerializer.Deserialize<MicrosoftIdentityOptions>(stream, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? new MicrosoftIdentityOptions();
    }
}
