using CommunityToolkit.Maui.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappChatViewer.ViewModels;
using WhatsappChatViewer.Views;

namespace WhatsappChatViewer.Services;

public class IAmSelector
{
    private readonly Lazy<MainPage> mainPage;

    public IAmSelector(Lazy<MainPage> mainPage)
	{
        this.mainPage = mainPage;
    }

    public async Task<string?> GetIAmName(IEnumerable<string> possibleNames)
    {
        var iAmSelectorViewModel = new IAmSelectorViewModel(possibleNames);

        var iAmSelectorPopup = new IAmSelectorPopup
        {
            BindingContext = iAmSelectorViewModel
        };

        await mainPage.Value.ShowPopupAsync(iAmSelectorPopup);

        return iAmSelectorViewModel.IAmName;
    }
}
