using MassTransit;
using Newtonsoft.Json;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Receiver
{
    public class RabbitMqPublisher : IConsumer<Person>
    {
        public async Task Consume(ConsumeContext<Person> context)
        {
            var person = context.Message;
            Console.WriteLine(JsonConvert.SerializeObject(person, Formatting.Indented));
        }
    }
}
