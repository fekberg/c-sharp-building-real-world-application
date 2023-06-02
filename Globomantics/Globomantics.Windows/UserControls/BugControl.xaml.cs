using Globomantics.Windows.ViewModels;
using System.Windows.Controls;

namespace Globomantics.Windows.UserControls;

/// <summary>
/// Interaction logic for BugControl.xaml
/// </summary>
public partial class BugControl : UserControl
{
    public BugControl(ITodoViewModel viewModel)
    {
        InitializeComponent();

        DataContext = viewModel;

        ParentTodo.SelectedIndex = -1;
    }
}