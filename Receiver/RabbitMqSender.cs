using MassTransit;
using Newtonsoft.Json;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Receiver
{
    public class RabbitMqSender : IConsumer<Product>
    {
        public async Task Consume(ConsumeContext<Product> context)
        {
            var product = context.Message;
            Console.WriteLine(JsonConvert.SerializeObject(product, Formatting.Indented));
        }
    }
}
