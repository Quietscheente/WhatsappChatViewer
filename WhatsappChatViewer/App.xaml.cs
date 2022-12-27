namespace WhatsappChatViewer;

public partial class App : Application
{
	public App(Lazy<MainPage> mainPage)
	{
		InitializeComponent();

		MainPage = mainPage.Value;
	}
}
