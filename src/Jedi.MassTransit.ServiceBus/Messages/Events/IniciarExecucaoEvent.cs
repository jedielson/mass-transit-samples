using MediatR;
using System;

namespace Jedi.MassTransit.ServiceBus.Messages.Events
{
    public class IniciarExecucaoEvent : INotification
    {
        public DateTime Data { get; set; }

        public string Nome { get; set; }
    }
}
