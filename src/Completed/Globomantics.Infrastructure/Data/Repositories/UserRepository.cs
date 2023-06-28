using Globomantics.Domain;
using Microsoft.EntityFrameworkCore;

namespace Globomantics.Infrastructure.Data.Repositories;

public class UserRepository : IRepository<User>
{
    private readonly GlobomanticsDbContext context;

    public UserRepository(GlobomanticsDbContext context)
    {
        this.context = context;
    }

    public async Task AddAsync(User user)
    {
        var existingUser = await context.Users.SingleOrDefaultAsync(u => u.Id == user.Id);

        if (existingUser is null)
        {
            var userToAdd = DomainToDataMapping.MapUser(user);

            await context.Users.AddAsync(userToAdd);
        }
        else
        {
            existingUser.Name = user.Name;

            context.Users.Update(existingUser);
        }
    }

    public async Task<IEnumerable<User>> AllAsync()
    {
        return await context.Users
            .Select(x => DataToDomainMapping.MapUser(x))
            .ToArrayAsync();
    }

    public async Task<User> FindByAsync(string name)
    {
        var user = await context.Users.SingleAsync(user => user.Name == name);

        return DataToDomainMapping.MapUser(user);
    }

    public async Task<User> GetAsync(Guid id)
    {
        var user = await context.Users.SingleAsync(user => user.Id == id);

        return DataToDomainMapping.MapUser(user);
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}
