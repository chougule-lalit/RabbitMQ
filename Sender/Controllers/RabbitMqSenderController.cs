using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sender.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RabbitMqSenderController : ControllerBase
    {
        private readonly IBus _bus;
        private readonly IRequestClient<BalanceUpdate> _requestClient;

        public RabbitMqSenderController(
            IBus bus,
            IRequestClient<BalanceUpdate> requestClient)
        {
            _bus = bus;
            _requestClient = requestClient;
        }

        [HttpPost]
        [Route("send-rabbitMQ")]
        public async Task<IActionResult> Consume()
        {
            var product = new Product();
            product.Name = "Computer";
            product.Price = 500;

            var url = new Uri("rabbitmq://localhost/send-tutorial");
            var endpoint = await _bus.GetSendEndpoint(url);
            await endpoint.Send(product);

            return Ok("RabbitMQ Sent...");
        }

        [HttpPost]
        [Route("publish-rabbitMQ")]
        public async Task<IActionResult> Event()
        {
            await _bus.Publish(new Person
            {
                Email = "lalit.chougule@ness.com",
                Name = "Lalit Chougule"
            });

            return Ok("RabbitMQ Event published...");
        }

        [HttpPost]
        [Route("request-response-rabbitMQ")]
        public async Task<IActionResult> RequestResponse()
        {
            var requestData = new BalanceUpdate
            {
                Type = "Debit",
                Amount = 50
            };

            var request = _requestClient.Create(requestData);
            var response = await request.GetResponse<BalanceUpdate>();

            return Ok(response);
        }
    }
}
