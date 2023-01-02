using CommunityToolkit.Maui.Views;
using WhatsappChatViewer.ViewModels;

namespace WhatsappChatViewer.Views;

public partial class IAmSelectorPopup : Popup
{
	public IAmSelectorPopup()
	{
		InitializeComponent();
	}

    private void RadioButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
		var selectedRadioButton = (RadioButton)sender;

		if (selectedRadioButton.IsChecked)
			((IAmSelectorViewModel)BindingContext).SetIAmCommand.Execute(selectedRadioButton.Value);
    }

    private void ButtonFinished_Clicked(object sender, EventArgs e)
    {
		this.Close();
    }
}