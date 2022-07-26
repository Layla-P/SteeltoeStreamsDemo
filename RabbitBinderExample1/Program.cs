using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitBinderExample;
using Steeltoe.Common.Hosting;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using Steeltoe.Extensions.Configuration.Placeholder;
using Steeltoe.Management.Endpoint;
using Steeltoe.Stream.StreamHost;

//CONSUMER

var builder = StreamHost
              .CreateDefaultBuilder<VoteHandler>(args)
              .ConfigureServices(svc => svc.AddSingleton<IConsumerVotingService, DefaultVotingService>()); 

var host = builder.Build();

await host.StartAsync();

Console.ReadLine();