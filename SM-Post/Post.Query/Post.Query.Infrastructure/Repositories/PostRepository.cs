using Microsoft.EntityFrameworkCore;
using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;
using Post.Query.Infrastructure.DataAccess;

namespace Post.Query.Infrastructure.Repositories;

public class PostRepository : IPostRepository
{
    private readonly DatabaseContextFactory _dbContextFactory;

    public PostRepository(DatabaseContextFactory dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task CreateAsync(PostEntity post)
    {
        await using DatabaseContext context = _dbContextFactory.CreateDbContext();
        context.Posts.Add(post);
        
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(PostEntity post)
    {
        await using DatabaseContext context = _dbContextFactory.CreateDbContext();
        context.Posts.Update(post);
        
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid postId)
    {
        await using DatabaseContext context = _dbContextFactory.CreateDbContext();
        var post = await GetByIdAsync(postId);
        if (post == null)
            return;
        
        context.Posts.Remove(post);
        await context.SaveChangesAsync();
    }

    public async Task<PostEntity> GetByIdAsync(Guid postId)
    {
        await using DatabaseContext context = _dbContextFactory.CreateDbContext();

        return await context.Posts
            .Include(x => x.Comments)
            .FirstOrDefaultAsync(x => x.PostId == postId);
    }

    public async Task<List<PostEntity>> GetAllAsync()
    {
        await using DatabaseContext context = _dbContextFactory.CreateDbContext();

        return await context.Posts
            .AsNoTracking()
            .Include(x => x.Comments)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<PostEntity>> GetByAuthorAsync(string author)
    {
        await using DatabaseContext context = _dbContextFactory.CreateDbContext();

        return await context.Posts
            .Include(x => x.Comments)
            .Where(x => x.Author.Contains(author))
            .ToListAsync();;
    }

    public async Task<List<PostEntity>> GetWithLikesAsync(int numberOfLikes)
    {
        await using DatabaseContext context = _dbContextFactory.CreateDbContext();
        
        return await context.Posts
            .AsNoTracking()
            .Include(x => x.Comments)
            .AsNoTracking()
            .Where(x => x.Likes >= numberOfLikes)
            .ToListAsync();
    }

    public async Task<List<PostEntity>> GetWithCommentsAsync()
    {
        await using DatabaseContext context = _dbContextFactory.CreateDbContext();

        return await context.Posts
            .AsNoTracking()
            .Include(x => x.Comments)
            .AsNoTracking()
            .Where(x => x.Comments != null && x.Comments.Any())
            .ToListAsync();
    }
}