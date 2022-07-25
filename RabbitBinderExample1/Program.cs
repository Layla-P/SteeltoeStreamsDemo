using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitBinderExample;
using Steeltoe.Common.Hosting;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using Steeltoe.Stream.StreamHost;

var builder = StreamHost
              .CreateDefaultBuilder<VoteHandler>(args)
              .ConfigureServices(svc => svc.AddSingleton<IVotingService, DefaultVotingService>());

var host = builder.Build();

await host.StartAsync();

Console.ReadLine();