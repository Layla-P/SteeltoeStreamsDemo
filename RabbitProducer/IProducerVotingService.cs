using Steeltoe.Messaging.Handler.Attributes;
using Steeltoe.Messaging;
using Steeltoe.Stream.Attributes;
using Steeltoe.Stream.Binding;
using Steeltoe.Stream.Messaging;
using Steeltoe.Messaging.Support;
using Microsoft.Extensions.Logging;

namespace RabbitProducer;


public interface IProducerVotingService
{
    bool Record(Vote vote);
    Vote GetVote();
}

public class DefaultVotingService : IProducerVotingService
{
    private readonly ILogger<DefaultVotingService> _logger;
    private readonly MessageSender _processor;
    private readonly ISource _source;
    private Vote _vote = new();
    public DefaultVotingService(
        ILogger<DefaultVotingService> logger,
        MessageSender processor,
        ISource source)
    {
        _logger = logger;
        _processor = processor;
        _source = source;
    }

    public Vote GetVote()
    {
        return _vote;
    }
    public bool Record(Vote vote)
    {
        _vote = vote;
        var message = _processor.Handle(vote);
        return _source.Output.Send(message);
    }
}


public class Vote
{
    public string Choice { get; set; } = "Default";
}


[EnableBinding(typeof(ISource))]
public class MessageSender
{

    private readonly BinderAwareChannelResolver _resolver;
    public MessageSender(BinderAwareChannelResolver resolver)
    {

        _resolver = resolver;
    }

    [SendTo(ISource.OUTPUT + ".vote-detail")]
    public IMessage Handle(Vote vote)
    {
        return MessageBuilder.WithPayload(vote).Build();
    }
}
