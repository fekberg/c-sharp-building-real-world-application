using Globomantics.Windows.ViewModels;
using System.Windows.Controls;

namespace Globomantics.Windows.UserControls;

/// <summary>
/// Interaction logic for FeatureControl.xaml
/// </summary>
public partial class FeatureControl : UserControl
{
    public FeatureControl(ITodoViewModel viewModel)
    {
        InitializeComponent();

        DataContext = viewModel;

        ParentTodo.SelectedIndex = -1;
    }
}
