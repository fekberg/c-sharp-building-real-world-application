using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Globomantics.Domain;
using Globomantics.Infrastructure.Data.Repositories;
using Globomantics.Windows.Messages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globomantics.Windows.ViewModels;

public class FeatureViewModel : BaseTodoViewModel<Feature>
{
    private readonly IRepository<Feature> repository;

    private string? description;
    public string? Description
    {
        get => description;
        set
        {
            description = value;

            OnPropertyChanged(nameof(Description));
        }
    }

    public FeatureViewModel(IRepository<Feature> repository) : base()
    {
        SaveCommand = new RelayCommand(async () => await SaveAsync());
        this.repository = repository;
    }

    public override void UpdateModel(Todo model)
    {
        if (model is not Feature feature) return;

        base.UpdateModel(feature);

        if (feature is null) return;

        Description = feature.Description;
    }

    public override async Task SaveAsync()
    {
        var filip = new User("Filip Ekberg");
        
        if(string.IsNullOrEmpty(Title))
        {
            ShowError?.Invoke($"{nameof(Title)} cannot be empty");
            return;
        }

        if (Model is null)
        {
            Model = new Feature(Title,
                Description ?? "No descriptiopn",
                "UI",
                1,
                filip,
                filip)
            {
                DueDate = System.DateTimeOffset.UtcNow,
                Parent = Parent,
                IsCompleted = IsCompleted
            };
        }
        else
        {
            Model = Model with
            {
                Title = Title,
                Description = Description,
                Parent = Parent,
                IsCompleted = IsCompleted
            };
        }

        await repository.AddAsync(Model);
        
        await repository.SaveChangesAsync();

        WeakReferenceMessenger.Default.Send<TodoSavedMessage>(new(Model));
    }
}