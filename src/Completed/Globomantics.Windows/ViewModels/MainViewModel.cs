using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Globomantics.Domain;
using Globomantics.Infrastructure.Data.Repositories;
using Globomantics.Windows.Json;
using Globomantics.Windows.Messages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Globomantics.Windows.ViewModels;

public class MainViewModel : ObservableObject,
    IViewModel
{
    private string searchText = "";
    private string statusText = "Everything is OK!";
    private bool isLoading;
    private bool isInitialized;
    private readonly IRepository<User> userRepository;
    private readonly IRepository<TodoTask> todoRepository;

    public string SearchText
    {
        get => searchText;
        set
        {
            searchText = value;
            OnPropertyChanged(nameof(SearchText));
        }
    }
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
    public ICommand SearchCommand { get; set; }

    public Action<string>? ShowAlert { get; set; }
    public Action<string>? ShowError { get; set; }
    public Func<IEnumerable<string>>? ShowOpenFileDialog { get; set; }
    public Func<string>? ShowSaveFileDialog { get; set; }
    public Func<string, bool>? AskForConfirmation { get; set; }

    public ObservableCollection<Todo> Completed { get; set; } = new();
    public ObservableCollection<Todo> Unfinished { get; set; } = new();

    public MainViewModel(IRepository<User> userRepository,
        IRepository<TodoTask> todoRepository)
    {
        WeakReferenceMessenger.Default.Register<TodoSavedMessage>(this,
            (sender, message) => {
                var item = message.Value;

                if (item.IsCompleted)
                {
                    var existing = Unfinished.FirstOrDefault(i => i.Id == item.Id);

                    if (existing is not null)
                    {
                        Unfinished.Remove(existing);
                    }

                    ReplaceOrAdd(Completed, item);
                }
                else
                {
                    var existing = Completed.FirstOrDefault(i => i.Id == item.Id);

                    if (existing is not null)
                    {
                        Completed.Remove(existing);
                    }

                    ReplaceOrAdd(Unfinished, item);
                }
            });

        WeakReferenceMessenger.Default.Register<TodoDeletedMessage>(this,
            (sender, message) => {
                var item = message.Value;

                var unfinishedItem = Unfinished.FirstOrDefault(i => i.Id == item.Id);

                if (unfinishedItem is not null)
                {
                    Unfinished.Remove(unfinishedItem);
                }
            });

        this.userRepository = userRepository;
        this.todoRepository = todoRepository;

        ExportCommand = new RelayCommand(async () => {
            await ExportAsync();
        });

        ImportCommand = new RelayCommand(async () => {
            await ImportAsync();
        });

        SearchCommand = new RelayCommand(async () => { 
            Unfinished.Clear();

            var items = await todoRepository.AllAsync();

            var query = items.AsQueryable()
                    .Where(item => !item.IsCompleted && !item.IsDeleted);

            if (!string.IsNullOrWhiteSpace(SearchText) &&
                !SearchText.Equals("*", StringComparison.OrdinalIgnoreCase))
            {
                query = query.Where(item => item.Title.Contains(SearchText,
                                        StringComparison.OrdinalIgnoreCase));
            }
            foreach(var item in query)
            {
                Unfinished.Add(item);
            }
        });
    }

    private async Task ExportAsync()
    {
        var filename = ShowSaveFileDialog?.Invoke();

        IsLoading = true;

        var items = await todoRepository.AllAsync();

        var json = JsonConvert.SerializeObject(items,
            new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                SerializationBinder = new SerializationBinder()
            });

        await File.WriteAllTextAsync(filename, json);

        ShowAlert?.Invoke("Data exported");

        IsLoading = false;
    }

    private async Task ImportAsync()
    {
        var filenames = ShowOpenFileDialog?.Invoke();

        if (filenames is null || !filenames.Any())
        {
            return;
        }

        var filename = filenames.First();

        if(string.IsNullOrWhiteSpace(filename))
        {
            ShowError?.Invoke("No filename specified");
        }

        IsLoading = true;

        var json = await File.ReadAllTextAsync(filename);

        var items = JsonConvert.DeserializeObject<IEnumerable<TodoTask>>(
            json, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                SerializationBinder = new SerializationBinder()
            }
        );

        if (items is null)
        {
            return;
        }

        foreach(var item in items)
        {
            await todoRepository.AddAsync(item);

            if (item.IsCompleted)
            {
                Completed.Add(item);
            }
            else if(!item.IsDeleted)
            {
                Unfinished.Add(item);
            }
        }

        await todoRepository.SaveChangesAsync();

        IsLoading = false;
    }

    private void ReplaceOrAdd(ObservableCollection<Todo> collection, Todo item)
    {
        var existingItem = collection.FirstOrDefault(x => x.Id == item.Id);

        if (existingItem is not null)
        {
            var index = collection.IndexOf(existingItem);
            collection[index] = item;
        }
        else
        {
            collection.Add(item);
        }
    }

    public async Task InitializeAsync()
    {
        if (isInitialized) return;

        App.CurrentUser = await userRepository.FindByAsync("Filip");

        var items = await todoRepository.AllAsync();

        var itemsDue = items.Count(i => i.DueDate.ToLocalTime() > DateTimeOffset.Now);

        StatusText = $"Welcome {App.CurrentUser.Name}! " +
            $"You have {itemsDue} items passed due date.";

        foreach (var item in items.Where(item => !item.IsDeleted))
        {
            if (item.IsCompleted)
            {
                Completed.Add(item);
            }
            else
            {
                Unfinished.Add(item);
            }
        }

        isInitialized = true;
    }
}