using Globomantics.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Globomantics.Windows.ViewModels;

public interface ITodoViewModel : IViewModel
{
    IEnumerable<Todo>? AvailableParentTasks { get; set; }

    ICommand DeleteCommand { get; }
    ICommand SaveCommand { get; }
    Task SaveAsync();
    void UpdateModel(Todo model);
}
