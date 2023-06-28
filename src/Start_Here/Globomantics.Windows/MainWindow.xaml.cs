using Globomantics.Windows.ViewModels;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Globomantics.Windows;

public partial class MainWindow : Window
{
    private readonly MainViewModel mainViewModel;

    public MainWindow(MainViewModel mainViewModel)
    { 
        InitializeComponent();

        this.mainViewModel = mainViewModel;

        DataContext = mainViewModel;

        mainViewModel.ShowSaveFileDialog = () => OpenCreateFileDialog();
        mainViewModel.ShowOpenFileDialog = () => OpenFileDialog(".json", "JSON (.json)|*.json", true);
        mainViewModel.ShowError = (message) => {
            MessageBox.Show(message);
        };
        mainViewModel.ShowAlert = (message) => {
            MessageBox.Show(message);
        };
    }

    protected override async void OnActivated(EventArgs e)
    {
        base.OnActivated(e);

        try
        {
            await mainViewModel.InitializeAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }

    private UserControl CreateUserControl(string type, 
        // TODO: Change object to domain object type
        object? model = default)
    {
        throw new NotImplementedException();
    }

    private void Search_OnClick(object sender, RoutedEventArgs e)
    {
    }

    #region Boilerplate - Will not change during the course
    private void TodoType_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (TodoType.SelectedIndex == -1) return;

        CreateTodoControlContainer.Children.Clear();

        var control = CreateUserControl(TodoType.SelectedValue.ToString() ?? "");

        CreateTodoControlContainer.Children.Add(control);
    }
    private void TodoItem_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var list = sender as ListView;

        if (list?.SelectedValue is null) return;

        CreateTodoControlContainer.Children.Clear();

        var control = CreateUserControl(
            list.SelectedValue.GetType().Name,
            list.SelectedValue);

        CreateTodoControlContainer.Children.Add(control);

        CompletedItems.UnselectAll();

        UnfinishedItems.UnselectAll();
    }
    private IEnumerable<string> OpenFileDialog(string extension, string filter, bool multiselect)
    {
        var dialog = new OpenFileDialog
        {
            DefaultExt = extension,
            Filter = filter,
            Multiselect = multiselect
        };

        _ = dialog.ShowDialog();

        return dialog.FileNames;
    }
    private string OpenCreateFileDialog()
    {
        var dialog = new SaveFileDialog
        {
            DefaultExt = ".json",
            Filter = "JSON (.json)|*.json"
        };

        _ = dialog.ShowDialog();

        return dialog.FileName;
    }
    private void Hyperlink_OnRequestNavigate(object sender, RequestNavigateEventArgs e)
    {
        Process.Start(new ProcessStartInfo { FileName = e.Uri.AbsoluteUri, UseShellExecute = true });

        e.Handled = true;
    }
    private void Close_OnClick(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }
    #endregion
}
