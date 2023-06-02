using Globomantics.Domain;
using Microsoft.EntityFrameworkCore;

namespace Globomantics.Infrastructure.Data.Repositories;

public class BugRepository : TodoRepository<Bug>
{
    public BugRepository(GlobomanticsDbContext context) : base(context) { }

    public override async Task AddAsync(Bug bug)
    {
        var existingBug = await Context.Bugs.FirstOrDefaultAsync(b => b.Id == bug.Id);

        var user = await Context.Users.SingleOrDefaultAsync(u => u.Id == bug.CreatedBy.Id);

        user ??= new() { Id = bug.CreatedBy.Id, Name = bug.CreatedBy.Name };

        if (existingBug is not null)
        {
            await UpdateAsync(bug, existingBug, user);
        }
        else
        {
            await CreateAsync(bug, user);
        }
    }

    public override async Task<IEnumerable<Bug>> AllAsync()
    {
        return await Context.Bugs.Where(b => !b.IsDeleted)
            .Include(b => b.Images)
            .Include(t => t.CreatedBy)
            .Include(t => t.AssigedTo)
            .Select(x => DataToDomainMapping.MapTodoFromData<Data.Models.Bug, Bug>(x))
            .ToArrayAsync();
    }

    private async Task UpdateAsync(Bug bug, Data.Models.Bug bugToUpdate, Data.Models.User user)
    {
        await SetParentAsync(bugToUpdate, bug);

        bugToUpdate.IsCompleted = bug.IsCompleted;
        bugToUpdate.AffectedVersion = bug.AffectedVersion;
        bugToUpdate.AffectedUsers = bug.AffectedUsers;
        bugToUpdate.Description = bug.Description;
        bugToUpdate.Title = bug.Title;
        bugToUpdate.Severity = (Data.Models.Severity)bug.Severity;

        Context.Bugs.Update(bugToUpdate);
    }

    private async Task CreateAsync(Bug bug, Data.Models.User user)
    {
        var bugToAdd = DomainToDataMapping.MapTodoFromDomain<Bug, Data.Models.Bug>(bug);

        await SetParentAsync(bugToAdd, bug);

        bugToAdd.CreatedBy = user;

        await Context.AddAsync(bugToAdd);
    }

    public override async Task<Bug> GetAsync(Guid id)
    {
        var data = await Context.Bugs.SingleAsync(bug => bug.Id == id);

        return DataToDomainMapping.MapTodoFromData<Data.Models.Bug, Domain.Bug>(data);
    }
}
