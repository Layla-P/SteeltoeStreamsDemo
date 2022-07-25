﻿using Microsoft.Extensions.Logging;

using Steeltoe.Stream.Attributes;
using Steeltoe.Stream.Messaging;


namespace RabbitBinderExample;

public interface IVotingService
{
    void Record(Vote vote);
}

public class DefaultVotingService : IVotingService
{
    private readonly ILogger<DefaultVotingService> _logger;

    public DefaultVotingService(ILogger<DefaultVotingService> logger)
    {
        _logger = logger;
    }

    public void Record(Vote vote)
    {
        Console.WriteLine("Received a vote for " + vote.Choice);
    }
}


public class Vote
{
    public string Choice { get; set; }
}


[EnableBinding(typeof(ISink))]
public class VoteHandler
{
    private readonly IVotingService votingService;
    public VoteHandler(IVotingService service)
    {
        votingService = service;
    }

    [StreamListener(ISink.INPUT)]
    public void Handle(Vote vote)
    {
        votingService.Record(vote);
    }
}
