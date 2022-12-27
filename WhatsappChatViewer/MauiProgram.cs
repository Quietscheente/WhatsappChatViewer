using CommunityToolkit.Maui;
using WhatsappChatViewer.Services;
using WhatsappChatViewer.ViewModels;

namespace WhatsappChatViewer;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				fonts.AddFont("Font Awesome 6 Free-Solid-900.otf", "FontAwesome");
			})
			.Services
				// Start
				.AddSingleton<MainPage>()
                .AddSingleton<Lazy<MainPage>>(sp => new(() => sp.GetRequiredService<MainPage>()))

				// Viewmodels
				.AddSingleton<MainPageViewModel>()
				// Services
				.AddSingleton<ChatImporter>()
				.AddSingleton<UiMessageLogger>()
				;

		return builder.Build();
	}
}
