using Globomantics.Domain;
using Microsoft.EntityFrameworkCore;

namespace Globomantics.Infrastructure.Data.Repositories;

public abstract class TodoRepository<T> : IRepository<T>
    where T : TodoTask
{
    protected readonly GlobomanticsDbContext Context;

    public TodoRepository(GlobomanticsDbContext context)
    {
        Context = context;
    }

    public abstract Task AddAsync(T item);

    public virtual async Task<IEnumerable<T>> AllAsync() 
    {
        return await Context.TodoTasks.Where(t => !t.IsDeleted)
            .Include(t => t.CreatedBy)
            .Include(t => t.Parent)
            .Select(x => DataToDomainMapping.MapTodoFromData<Data.Models.TodoTask, T>(x))
            .ToArrayAsync();
    }

    public abstract Task<T> GetAsync(Guid Id);

    public virtual async Task<T> FindByAsync(string title)
    {
        var task = await Context.TodoTasks.SingleAsync(task => task.Title == title);

        return DataToDomainMapping.MapTodoFromData<Data.Models.TodoTask, T>(task);
    }

    protected async Task SetParentAsync(Data.Models.Todo toBeAdded, Todo item)
    {
        Data.Models.TodoTask? existingParent = null;

        if (item.Parent is not null)
        {
            existingParent = Context.TodoTasks.FirstOrDefault(x => x.Id == item.Parent.Id);
        }

        if (existingParent is not null)
        {
            toBeAdded.Parent = existingParent;
        }
        else if (item.Parent is not null)
        {
            var parentToAdd = DomainToDataMapping.MapTodoFromDomain<TodoTask, Data.Models.TodoTask>(item.Parent);

            toBeAdded.Parent = parentToAdd;

            await Context.AddAsync(parentToAdd);
        }
    }

    public async Task SaveChangesAsync()
    {
        await Context.SaveChangesAsync();
    }
}
