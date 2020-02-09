using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jedi.MassTransit.ServiceBus.Messages
{
    public class IniciarExecucao
    {
        public DateTime Data { get; set; }

        public string Nome { get; set; }
    }
}
