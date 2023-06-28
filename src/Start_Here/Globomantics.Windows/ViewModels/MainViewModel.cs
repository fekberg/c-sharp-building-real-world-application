using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Globomantics.Windows.ViewModels;

public class MainViewModel : ObservableObject, 
    IViewModel
{
    private string statusText = "Everything is OK!";
    private bool isLoading;
    private bool isInitialized;

    public string StatusText 
    {
        get => statusText;
        set
        {
            statusText = value;

            OnPropertyChanged(nameof(StatusText));
        }
    }
    public bool IsLoading
    {
        get => isLoading;
        set
        {
            isLoading = value;

            OnPropertyChanged(nameof(IsLoading));
        }
    }

    public ICommand ExportCommand { get; set; }
    public ICommand ImportCommand { get; set; }

    public Action<string>? ShowAlert { get; set; }
    public Action<string>? ShowError { get; set; }
    public Func<IEnumerable<string>>? ShowOpenFileDialog { get; set; }
    public Func<string>? ShowSaveFileDialog { get; set; }
    public Func<string, bool>? AskForConfirmation { get; set; }

    public MainViewModel()
    {
    }

    public async Task InitializeAsync()
    {
        if (isInitialized) return;

        isInitialized = true;
    }
}