using CQRS.Core.Commands;

namespace Post.Cmd.Api.Commands;

public class AddCommentCommand : BaseCommand
{
    public string Commnent { get; set; }
    public string UserName { get; set; }
}