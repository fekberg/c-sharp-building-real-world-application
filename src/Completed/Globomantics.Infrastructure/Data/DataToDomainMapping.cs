using Microsoft.VisualBasic;

namespace Globomantics.Infrastructure.Data;

public class DataToDomainMapping
{
    public static TTo MapTodoFromData<TFrom, TTo>(TFrom input)
        where TFrom : Data.Models.Todo
        where TTo : Domain.Todo
    {
        var model = input switch
        {
            Data.Models.Bug bug => MapBug(bug),
            Data.Models.Feature feature => MapFeature(feature),
            Data.Models.TodoTask task => MapTask(task),
            _ => throw new NotImplementedException()
        } as TTo;

        return model;
    }

    public static Domain.User MapUser(Data.Models.User? user)
    {
        if (user is null) return null!;

        return new(user.Name) { Id = user.Id };
    }

    private static Domain.Bug MapBug(Data.Models.Bug bug)
    {
        return new(bug.Title,
            bug.Description,
            (Domain.Severity)bug.Severity,
            bug.AffectedVersion,
            bug.AffectedUsers,
            MapUser(bug.CreatedBy),
            MapUser(bug.AssignedTo),
            bug?.Images?.Select(
                image => Convert.FromBase64String(image.ImageData)).ToArray()
                ?? Enumerable.Empty<byte[]>().ToArray()
        )
        { 
            Id = bug.Id,
            DueDate = bug.DueDate
        };
    }

    private static Domain.Feature MapFeature(Data.Models.Feature feature)
    {
        return new(feature.Title,
            feature.Description,
            feature.Component,
            feature.Priority,
            MapUser(feature.CreatedBy),
            MapUser(feature.AssignedTo))
        {
            Id = feature.Id,
            DueDate = feature.DueDate,
            IsCompleted = feature.IsCompleted
        };
    }

    private static Domain.TodoTask MapTask(Data.Models.TodoTask task)
    {
        return new(task.Title,
            task.DueDate,
            MapUser(task.CreatedBy))
        {
            Id = task.Id
        };
    }
}
