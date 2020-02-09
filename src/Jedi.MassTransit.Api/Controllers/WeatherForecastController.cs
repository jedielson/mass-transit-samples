using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using Jedi.MassTransit.ServiceBus.Messages;
using Jedi.MassTransit.ServiceBus.Messages.Events;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Jedi.MassTransit.Api.Controllers
{
    [ApiController]
    [Route("api/clima")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IMediator _mediator;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            var faker = new Faker();
            var @event = new IniciarExecucaoEvent
            {
                Data = DateTime.Now,
                Nome = faker.Random.AlphaNumeric(10)
            };

            _logger.LogInformation($"Sending Message: {@event.Nome} at {@event.Data}");

            await _mediator.Publish(@event);

            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
