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
    public class OrderBillHandler :
    IHandleMessages<OrderBilled>
    {
        static ILog log = LogManager.GetLogger<OrderBillHandler>();

        public Task Handle(OrderBilled message, IMessageHandlerContext context)
        {
            log.Info($"Received OrderBilled, OrderId = {message.OrderId} - Charging credit card...");

            return Task.CompletedTask;
            //var OrderBilled = new OrderBilled
            //{
            //    OrderId = message.OrderId
            //};
            //return context.Publish(OrderBilled);
        }
    }
}
