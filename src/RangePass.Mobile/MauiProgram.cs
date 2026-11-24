using Microsoft.Extensions.Logging;
using RangePass.Mobile.Services;

namespace RangePass.Mobile;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			});

		builder.Services.AddMauiBlazorWebView();

		var identityOptions = MicrosoftIdentityOptions.Load();
		builder.Services.AddSingleton(identityOptions);
		builder.Services.AddSingleton<MicrosoftIdentityAuthService>();

#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
