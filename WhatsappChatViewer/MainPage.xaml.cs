using WhatsappChatViewer.ViewModels;

namespace WhatsappChatViewer;

public partial class MainPage : ContentPage
{
	public MainPage(MainPageViewModel mainPageViewModel)
	{
		InitializeComponent();

        BindingContext = mainPageViewModel;
    }

}

