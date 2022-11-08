using MassTransit;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Receiver
{
    public class RabbitMqRequestResponse : IConsumer<BalanceUpdate>
    {
        public async Task Consume(ConsumeContext<BalanceUpdate> context)
        {
            var data = context.Message;

            data.Balance = 700;

            await context.RespondAsync(data);
        }
    }
}
