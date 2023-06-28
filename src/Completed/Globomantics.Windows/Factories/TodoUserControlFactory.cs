using Globomantics.Windows.UserControls;
using Globomantics.Windows.ViewModels;
using System;
using System.Windows.Controls;

namespace Globomantics.Windows.Factories;

public class TodoUserControlFactory
{
    public static UserControl CreateUserControl(ITodoViewModel viewModel)
    {
        UserControl control = viewModel switch
        {
            BugViewModel => new BugControl(viewModel),
            FeatureViewModel => new FeatureControl(viewModel),
            _ => throw new NotImplementedException()
        };

        return control;
    }
}
