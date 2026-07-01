using CQRS.Core.Events;

namespace Post.Common.Events;

public abstract class MessageUpdateEvent : BaseEvent
{
    protected MessageUpdateEvent() : base(nameof(MessageUpdateEvent))
    {
    }
    public int Version { get; set; }
    public string Type { get; set; }
}