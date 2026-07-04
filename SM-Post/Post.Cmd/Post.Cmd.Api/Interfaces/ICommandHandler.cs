using Post.Cmd.Api.Commands;

namespace Post.Cmd.Api.Interfaces;

public interface ICommandHandler
{
    Task HandleAsync(NewPostCommand command);
    Task HandleAsync(EditMessageCommand command);
    Task HandleAsync(LikePostCommand command);
    Task HandleAsync(AddCommentCommand command);
    Task HandleAsync(EditCommentCommand command);
    Task HandleAsync(RemoveCommentCommand command);
    Task HandleAsync(DeletePostCommand command);
}