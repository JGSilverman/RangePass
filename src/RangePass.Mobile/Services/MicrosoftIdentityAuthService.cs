using Microsoft.Identity.Client;

namespace RangePass.Mobile.Services;

public sealed class MicrosoftIdentityAuthService
{
    private readonly MicrosoftIdentityOptions _options;
    private readonly IPublicClientApplication _publicClientApplication;
    private IAccount? _account;

    public MicrosoftIdentityAuthService(MicrosoftIdentityOptions options)
    {
        _options = options;
        _publicClientApplication = PublicClientApplicationBuilder
            .Create(options.ClientId)
            .WithAuthority(options.Authority)
            .WithRedirectUri(options.RedirectUri)
            .Build();
    }

    public bool IsConfigured => _options.IsConfigured;
    public bool IsSignedIn => _account is not null;
    public string? DisplayName { get; private set; }
    public string? Email { get; private set; }

    public event Action? AuthenticationChanged;

    public async Task InitializeAsync()
    {
        if (!IsConfigured)
            return;

        var accounts = await _publicClientApplication.GetAccountsAsync();
        _account = accounts.FirstOrDefault();
        if (_account is null)
            return;

        var result = await AcquireTokenSilentAsync();
        UpdateAccount(result?.Account, result?.Account?.Username);
    }

    public async Task<string?> SignInAsync()
    {
        if (!IsConfigured)
            return "Microsoft identity is not configured. Add the Azure app registration values to microsoft-identity.json.";

        try
        {
            var result = await _publicClientApplication
                .AcquireTokenInteractive(_options.Scopes)
                .WithPrompt(Prompt.SelectAccount)
                .ExecuteAsync();

            UpdateAccount(result.Account, result.Account.Username);
            return null;
        }
        catch (MsalException exception)
        {
            return exception.Message;
        }
    }

    public async Task<string?> SignOutAsync()
    {
        if (!IsSignedIn)
            return null;

        try
        {
            foreach (var account in await _publicClientApplication.GetAccountsAsync())
                await _publicClientApplication.RemoveAsync(account);

            _account = null;
            DisplayName = null;
            Email = null;
            AuthenticationChanged?.Invoke();
            return null;
        }
        catch (MsalException exception)
        {
            return exception.Message;
        }
    }

    public async Task<AuthenticationResult?> AcquireTokenSilentAsync()
    {
        if (!IsConfigured || _account is null)
            return null;

        try
        {
            return await _publicClientApplication
                .AcquireTokenSilent(_options.Scopes, _account)
                .ExecuteAsync();
        }
        catch (MsalUiRequiredException)
        {
            return null;
        }
    }

    private void UpdateAccount(IAccount? account, string? email)
    {
        _account = account;
        DisplayName = account?.Username ?? email;
        Email = email;
        AuthenticationChanged?.Invoke();
    }
}
