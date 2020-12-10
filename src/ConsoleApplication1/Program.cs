using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;

namespace Movidesk.Worker.Email.Messages
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host("localhost", "/", h => {
                    h.Username("guest");
                    h.Password("guest");
                });
                
                cfg.Message<EmailReceived>(x => x.SetEntityName("movidesk.email.received"));

                cfg.Publish<EmailReceived>(t =>
                {
                    t.Durable = true;
                    t.AutoDelete = false;
                    t.ExchangeType = "headers";
                });
            });

            var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));

            await busControl.StartAsync(source.Token);
            try
            {
                do
                {
                    string value = await Task.Run(() =>
                    {
                        Console.WriteLine("Enter message (or quit to exit)");
                        Console.Write("> ");
                        return Console.ReadLine();
                    });

                    if("quit".Equals(value, StringComparison.OrdinalIgnoreCase))
                        break;

                    await busControl.Publish<EmailReceived>(new
                    {
                        Value = value
                    }, context => context.Headers.Set("email-type", 0));
                }
                while (true);
            }
            finally
            {
                await busControl.StopAsync();
            }
        }
    }
    
    public class EmailReceived
    {
        public string Value { get; set; }
    }
}