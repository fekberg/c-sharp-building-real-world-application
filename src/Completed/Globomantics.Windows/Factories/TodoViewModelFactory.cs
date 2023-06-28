using Globomantics.Domain;
using Globomantics.Windows.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Globomantics.Windows.Factories;

public class TodoViewModelFactory
{
    public static IEnumerable<string> TodoTypes = new[]
    {
        nameof(Bug),
        nameof(Feature)
    };

    private readonly IServiceProvider serviceProvider;

    public TodoViewModelFactory(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public ITodoViewModel CreateViewModel(string type,
        IEnumerable<Todo> tasks,
        Todo? model = default)
    {
        ITodoViewModel? viewModel = type switch
        {
            nameof(Bug) => serviceProvider.GetService<BugViewModel>(),
            nameof(Feature) => serviceProvider.GetService<FeatureViewModel>(),
            _ => throw new NotImplementedException()
        };

        ArgumentNullException.ThrowIfNull(viewModel);

        if(tasks is not null && tasks.Any())
        {
            viewModel.AvailableParentTasks = tasks;
        }

        if(model is not null)
        {
            viewModel.UpdateModel(model);
        }

        return viewModel;
    }
}
