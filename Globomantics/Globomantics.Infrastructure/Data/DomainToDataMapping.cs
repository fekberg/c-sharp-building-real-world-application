namespace Globomantics.Infrastructure.Data;

public class DomainToDataMapping
{
    public static TTo MapTodoFromDomain<TFrom, TTo>(Domain.Todo input)
        where TFrom : Domain.Todo
        where TTo : Data.Models.Todo
    {
        var model = input switch
        {
            Domain.Bug bug => MapFromDomainBug(bug),
            Domain.Feature feature => MapFromDomainFeature(feature),
            Domain.TodoTask task => MapFromDomainTodoTask(task),
            _ => throw new NotImplementedException()
        } as TTo;

        ArgumentNullException.ThrowIfNull(model);

        return model;
    }
    public static Data.Models.User MapFromDomainUser(Domain.User user)
    {
        return new () { Id = user.Id, Name = user.Name };
    }
    private static Data.Models.Bug MapFromDomainBug(Domain.Bug bug)
    {
        return new Data.Models.Bug()
        {
            Id = bug.Id,
            AffectedUsers = bug.AffectedUsers,
            AffectedVersion = bug.AffectedVersion,
            CreatedDate = bug.CreatedDate,
            DueDate = bug.DueDate,
            Images = bug.Images.Select(image => new Data.Models.Image { ImageData = Convert.ToBase64String(image) }).ToArray(),
            IsCompleted = bug.IsCompleted,
            IsDeleted = bug.IsDeleted,
            Severity = (Data.Models.Severity)bug.Severity,
            Title = bug.Title,
            Description = bug.Description
        };
    }
    private static Data.Models.Feature MapFromDomainFeature(Domain.Feature feature)
    {
        return new Data.Models.Feature()
        {
            Id = feature.Id,
            Component = feature.Component,
            CreatedDate = feature.CreatedDate,
            DueDate = feature.DueDate,
            IsCompleted = feature.IsCompleted,
            IsDeleted = feature.IsDeleted,
            Title = feature.Title,
            Description = feature.Description,
            Priority = feature.Priority
        };
    }
    private static Data.Models.TodoTask MapFromDomainTodoTask(Domain.TodoTask task)
    {
        return new Data.Models.TodoTask()
        {
            Id = task.Id,
            CreatedDate = task.CreatedDate,
            DueDate = task.DueDate,
            IsCompleted = task.IsCompleted,
            IsDeleted = task.IsDeleted,
            Title = task.Title
        };
    }
}

