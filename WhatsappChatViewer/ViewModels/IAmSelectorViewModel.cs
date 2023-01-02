using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsappChatViewer.ViewModels;

public class IAmSelectorViewModel
{
    public List<string> PossibleNames { get; }
    public string? IAmName { get; private set; }

    public Command SetIAmCommand { get;}

    public IAmSelectorViewModel(IEnumerable<string> possibleNames)
    {
        PossibleNames = new(possibleNames);

        SetIAmCommand = new Command(SetIAm);
    }

    private void SetIAm(object iAmString)
    {
        IAmName = (string)iAmString;
    }
}
