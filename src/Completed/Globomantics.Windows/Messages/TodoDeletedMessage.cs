using CommunityToolkit.Mvvm.Messaging.Messages;
using Globomantics.Domain;

namespace Globomantics.Windows.Messages;

public class TodoDeletedMessage : ValueChangedMessage<Todo>
{
    public TodoDeletedMessage(Todo value) : base(value)
    {
    }
}