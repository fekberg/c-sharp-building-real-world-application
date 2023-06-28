namespace Globomantics.Infrastructure.Data;

public class DomainToDataMapping
{
    public static TTo MapTodoFromDomain<TFrom, TTo>(Domain.Todo input)
        where TFrom : Domain.Todo
        where TTo : Data.Models.Todo
    {
        var model = input switch
        {
            Domain.Bug bug => MapBug(bug),
            Domain.Feature feature => MapFeature(feature),
            Domain.TodoTask task => MapTask(task),
            _ => throw new NotImplementedException()
        } as TTo;

        return model;
    }

    public static Data.Models.User MapUser(Domain.User user)
    {
        return new() { Id = user.Id, Name = user.Name };
    }

    private static Data.Models.Bug MapBug(Domain.Bug bug)
    {
        return new()
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
            Description = bug.Description,
            AssignedToId = bug.AssignedTo?.Id,
            CreatedById = bug.CreatedBy?.Id
        };
    }

    private static Data.Models.Feature MapFeature(Domain.Feature feature)
    {
        return new()
        {
            Id = feature.Id,
            Component = feature.Component,
            CreatedDate = feature.CreatedDate,
            DueDate = feature.DueDate,
            IsCompleted = feature.IsCompleted,
            IsDeleted = feature.IsDeleted,
            Title = feature.Title,
            Description = feature.Description,
            Priority = feature.Priority,
            AssignedToId = feature.AssignedTo?.Id,
            CreatedById = feature.CreatedBy?.Id
        };
    }

    private static Data.Models.TodoTask MapTask(Domain.TodoTask task)
    {
        return new()
        {
            Id = task.Id,
            CreatedDate = task.CreatedDate,
            DueDate = task.DueDate,
            IsCompleted = task.IsCompleted,
            IsDeleted = task.IsDeleted,
            Title = task.Title,
            CreatedById = task.CreatedBy?.Id
        };
    }
}
