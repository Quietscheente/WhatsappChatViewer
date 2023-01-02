using CommunityToolkit.Maui;
using WhatsappChatViewer.Services;
using WhatsappChatViewer.ViewModels;
using WhatsappChatViewer.Views;

namespace WhatsappChatViewer;

public static class MauiProgram
{
	public delegate RawMessageReader RawMessageReaderFactory(string chatPath);


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
				.AddSingleton<ChatsHandler>()
				.AddSingleton<UiMessageLogger>()
				.AddTransient<IAmSelector>()
				.AddSingleton<ChatMetadataHandler>()
				.AddSingleton<RawMessageReaderFactory>(chatPath => new RawMessageReader(chatPath))
				;

		return builder.Build();
	}
}
