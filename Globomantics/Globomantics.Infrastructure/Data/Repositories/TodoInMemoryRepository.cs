using Globomantics.Domain;
using System.Collections.Concurrent;

namespace Globomantics.Infrastructure.Data.Repositories;

public class TodoInMemoryRepository : IRepository<Todo>
{
    private ConcurrentDictionary<Guid, Todo> Items { get; } = new();

    public Task AddAsync(Todo item)
    {
        Items.TryAdd(item.Id, item);

        return Task.CompletedTask;
    }

    public Task<IEnumerable<Todo>> AllAsync()
    {
        return Task.FromResult<IEnumerable<Todo>>(Items.Values.ToArray());
    }

    public Task<Todo> FindByAsync(string value)
    {
        return Task.FromResult(Items.Values.First(item => item.Title == value));
    }

    public Task<Todo> GetAsync(Guid Id)
    {
        return Task.FromResult(Items[Id]);
    }

    public Task SaveChangesAsync()
    {
        return Task.CompletedTask;
    }
}