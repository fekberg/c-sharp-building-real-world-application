using System;
using System.Collections.Generic;

namespace Globomantics.Windows.ViewModels;

public interface IViewModel
{
    Action<string>? ShowAlert { get; set; }
    Action<string>? ShowError { get; set; }
    public Func<IEnumerable<string>>? ShowOpenFileDialog { get; set; }
    public Func<string>? ShowSaveFileDialog { get; set; }
    Func<string, bool>? AskForConfirmation { get; set; }
}