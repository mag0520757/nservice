using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using System.Threading.Tasks;
using Sales.Messages;
using Billing.Messages;


namespace Shipping
{
    public class OrderPlacedHandler :
      IHandleMessages<OrderPlaced>
    {
        static ILog log = LogManager.GetLogger<OrderPlacedHandler>();

        public Task Handle(OrderPlaced message, IMessageHandlerContext context)
        {
            log.Info($"Received OrderPlaced, OrderId = {message.OrderId} - Charging credit card...");
            

            return Task.CompletedTask;
          
        }
    }
}
