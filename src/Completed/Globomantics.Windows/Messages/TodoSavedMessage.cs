using CommunityToolkit.Mvvm.Messaging.Messages;
using Globomantics.Domain;

namespace Globomantics.Windows.Messages;

public class TodoSavedMessage : ValueChangedMessage<Todo>
{
    public TodoSavedMessage(Todo value) : base(value)
    {
    }
}
