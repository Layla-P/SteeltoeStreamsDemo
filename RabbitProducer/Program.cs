using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitProducer;
using Steeltoe.Messaging.Core;
using Steeltoe.Messaging;
using Steeltoe.Stream.Binding;
using Steeltoe.Stream.StreamHost;
using Microsoft.Extensions.Configuration;
using Steeltoe.Common.Hosting;
using Steeltoe.Extensions.Configuration.CloudFoundry;

var builder = StreamHost
              .CreateDefaultBuilder<TransformProcessor>(args)
              .ConfigureServices(svc => svc.AddSingleton<IVotingService, DefaultVotingService>());

var host = builder.Build();

await host.StartAsync();

var sp = host.Services;

var vs = sp.GetRequiredService<IVotingService>();

var choice = Console.ReadLine();

vs.Record(new Vote { Choice = choice });