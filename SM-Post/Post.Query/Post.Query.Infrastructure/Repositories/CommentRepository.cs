using MongoDB.Driver.Linq;
using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;
using Post.Query.Infrastructure.DataAccess;

namespace Post.Query.Infrastructure.Repositories;

public class CommentRepository : ICommentRepository
{
    private readonly DatabaseContextFactory _dbContextFactory;

    public CommentRepository(DatabaseContextFactory dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task CreateAsync(CommentEntity comment)
    {
        await using DatabaseContext context = _dbContextFactory.CreateDbContext();
        context.Comments.Add(comment);
        
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(CommentEntity comment)
    {
        await using DatabaseContext context = _dbContextFactory.CreateDbContext();
        context.Comments.Update(comment);
        
        await context.SaveChangesAsync();
    }

    public async Task<CommentEntity> GetCommentByIdAsync(Guid commentId)
    {
        await using DatabaseContext context = _dbContextFactory.CreateDbContext();

        return await context.Comments.FirstOrDefaultAsync(x => x.CommentId == commentId);
    }

    public async Task DeleteAsync(Guid commentId)
    {
        await using DatabaseContext context = _dbContextFactory.CreateDbContext();
        var comment = await context.Comments.FirstOrDefaultAsync(x => x.CommentId == commentId);
        if (comment == null)
            return;
        
        context.Comments.Remove(comment);
        await context.SaveChangesAsync();
    }
}