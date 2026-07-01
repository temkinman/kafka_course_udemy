using CQRS.Core.Commands;
using CQRS.Core.Infrastructure;

namespace Post.Cmd.Infrastructure.Dispatchers;

public class CommandDispatcher : ICommnadDispatcher
{
    private readonly Dictionary<Type, Func<BaseCommand, Task>> _handlers = new ();
    
    public void RegisterHandler<T>(Func<T, Task> handler) where T : BaseCommand
    {
        if (_handlers.ContainsKey(typeof(T)))
        {
            throw new IndexOutOfRangeException("You cannot register multiple handlers for this type.");
        } 
        
        _handlers.Add(typeof(T), cmd => handler((T)cmd));
    }

    public async Task SendAsync(BaseCommand command)
    {
        if (_handlers.TryGetValue(command.GetType(), out var handler))
        {
            await handler(command);
        }
        else
        {
            throw new ArgumentException(nameof(handler), "The command type is not registered!");
        }
    }
}