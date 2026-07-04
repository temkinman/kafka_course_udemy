using Post.Common.Events;
using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;

namespace Post.Query.Infrastructure.Handlers;

public class EventHandler : IEventHandler
{
    private readonly IPostRepository _postRepository;
    private readonly ICommentRepository _commentRepository;
    

    public EventHandler(IPostRepository postRepository, ICommentRepository commentRepository)
    {
        _postRepository = postRepository;
        _commentRepository = commentRepository;
    }

    public async Task On(PostCreatedEvent @event)
    {
        var post = new PostEntity
        {
            PostId = @event.Id,
            Author = @event.Author,
            DatePosted = @event.DatePosted,
            Message = @event.Message,
        };
        
        await _postRepository.CreateAsync(post);
    }

    public async Task On(PostLikedEvent @event)
    {
        var post = await _postRepository.GetByIdAsync(@event.Id);
        if(post == null)
            return;
        
        post.Likes++;
        await _postRepository.UpdateAsync(post);
    }

    public async Task On(PostRemovedEvent @event)
    {
        var post = await _postRepository.GetByIdAsync(@event.Id);
        if(post == null)
            return;
        
        await _postRepository.DeleteAsync(post.PostId);
    }

    public async Task On(MessageUpdatedEvent @event)
    {
        var post = await _postRepository.GetByIdAsync(@event.Id);
        if(post == null)
            return;
        
        post.Message = @event.Message;
        await _postRepository.UpdateAsync(post);
    }

    public async Task On(CommentAddedEvent @event)
    {
        var comment = new CommentEntity
        {
            PostId = @event.Id,
            CommentId = @event.CommentId,
            Comment = @event.Comment,
            CommentDate = @event.CommentDate,
            UserName = @event.UserName,
            Edited = false
        };
        
        await _commentRepository.CreateAsync(comment);
    }

    public async Task On(CommentUpdatedEvent @event)
    {
        var comment = await _commentRepository.GetCommentByIdAsync(@event.CommentId);
        if(comment == null)
            return;
        
        comment.Comment = @event.Comment;
        comment.Edited = true;
        comment.CommentDate = DateTime.Now;
        
        await _commentRepository.UpdateAsync(comment);
    }

    public async Task On(CommentRemovedEvent @event)
    {
        await _commentRepository.DeleteAsync(@event.CommentId);
    }
}