namespace Globomantics.Infrastructure.Data;

public class DataToDomainMapping
{
    public static TTo MapTodoFromData<TFrom, TTo>(TFrom input) 
        where TFrom : Data.Models.Todo
        where TTo : Domain.Todo
    {
        var model = input switch
        {
            Data.Models.Bug bug => MapFromDomainBug(bug),
            Data.Models.Feature feature => MapFromDomainFeature(feature),
            Data.Models.TodoTask task => MapFromDomainTodoTask(task),
            _ => throw new NotImplementedException()
        } as TTo;

        ArgumentNullException.ThrowIfNull(model);

        return model;
    }

    public static Domain.User MapDomainUserFromData(Data.Models.User? user)
    {
        if (user is null) return null!;

        return new Domain.User(user.Name) { Id = user.Id };
    }
    private static Domain.Bug MapFromDomainBug(Data.Models.Bug bug)
    {
        return new Domain.Bug(bug.Title,
            bug.Description,
            (Domain.Severity)bug.Severity,
            bug.AffectedVersion,
            bug.AffectedUsers,
            MapDomainUserFromData(bug.CreatedBy),
            MapDomainUserFromData(bug.AssigedTo),
            bug?.Images?.Select(image => Convert.FromBase64String(image.ImageData)).ToArray() ?? Enumerable.Empty<byte[]>())
        {
            Id = bug.Id,
            DueDate = bug.DueDate
        };
    }
    private static Domain.Feature MapFromDomainFeature(Data.Models.Feature feature)
    {
        return new Domain.Feature(feature.Title,
            feature.Description,
            feature.Component,
            feature.Priority,
            MapDomainUserFromData(feature.CreatedBy),
            MapDomainUserFromData(feature.AssigedTo))
        {
            Id = feature.Id,
            DueDate = feature.DueDate,
            IsCompleted = feature.IsCompleted
        };
    }
    private static Domain.TodoTask MapFromDomainTodoTask(Data.Models.TodoTask task)
    {
        return new Domain.TodoTask(task.Title,
            task.DueDate,
            MapDomainUserFromData(task.CreatedBy))
        {
            Id = task.Id
        };
    }
}