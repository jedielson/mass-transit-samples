using Jedi.MassTransit.ServiceBus.Messages;
using Jedi.MassTransit.ServiceBus.Messages.Events;
using MassTransit;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Jedi.MassTransit.ServiceBus.Producers
{
    public class IniciarExecucaoProducer : INotificationHandler<IniciarExecucaoEvent>
    {
        private readonly IBus _bus;

        public IniciarExecucaoProducer(IBus bus)
        {
            _bus = bus;
        }

        public async Task Handle(IniciarExecucaoEvent notification, CancellationToken cancellationToken)
        {
            await _bus.Publish(new IniciarExecucao
            {
                Data = notification.Data,
                Nome = notification.Nome
            }, cancellationToken);
        }
    }
}
