using Microsoft.Extensions.Logging;
using Steeltoe.Messaging;
using Steeltoe.Messaging.Handler.Attributes;
using Steeltoe.Stream.Attributes;
using Steeltoe.Stream.Binding;
using Steeltoe.Stream.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RabbitProducer;

public interface IVotingService
{
    Task Record(Vote vote);
    Vote GetVote();
}

public class DefaultVotingService : IVotingService
{
    private readonly ILogger<DefaultVotingService> _logger;
    private readonly TransformProcessor _processor;
    private Vote _vote;
    public DefaultVotingService(ILogger<DefaultVotingService> logger, TransformProcessor processor)
    {
        _logger = logger;
        _processor = processor;
    }

    public Vote GetVote()
    {
        return _vote;
    }
    public async Task Record(Vote vote)
    {
        _vote = vote;
        await _processor.Handle(vote);
    }
}


public class Vote
{
    public string Choice { get; set; } = "Default";
}


[EnableBinding(typeof(ISource))]
public class TransformProcessor
{

    private readonly BinderAwareChannelResolver _resolver;
    public TransformProcessor( BinderAwareChannelResolver resolver)
    {
        
        _resolver = resolver;
    }

    [SendTo(ISource.OUTPUT + ".vote")]   
    public async Task Handle(Vote vote)
{
        var messageChannel = _resolver.ResolveDestination("vote");

        //var vote = _votingService.GetVote();
        var message = JsonSerializer.Serialize(vote); ;
        var streamMessage = Message.Create(Encoding.UTF8.GetBytes(message));
        var messageWasSent = await messageChannel.SendAsync(streamMessage);
        //await _votingService.Record(vote);
    }
}
