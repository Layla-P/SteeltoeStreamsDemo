using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitProducer;
using Steeltoe.Stream.StreamHost;

//PRODUCER

var builder = StreamHost
              .CreateDefaultBuilder<MessageSender>(args)
              .ConfigureServices(svc => svc.AddSingleton<IProducerVotingService, DefaultVotingService>());


var host = builder.Build();

await host.StartAsync();

var sp = host.Services;

var vs = sp.GetRequiredService<IProducerVotingService>();

do
{
    Console.WriteLine("Who do you vote for? ");
    var choice = Console.ReadLine();
    var response = vs.Record(new Vote { Choice = choice ?? "default" });
    Console.WriteLine($"Your choice was {response.ToString()}");
}
while (true);
