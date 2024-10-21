using MassTransit;
using Messaging;

namespace OrderAPI.Consumers
{
  public class OrderRecievedConsumer : IConsumer<IOrderRecievedEvent>
  {
    private readonly ILogger<OrderRecievedConsumer> logger;
  

    public OrderRecievedConsumer(ILogger<OrderRecievedConsumer> logger)
    {
      this.logger = logger;
    }
    public async Task Consume(ConsumeContext<IOrderRecievedEvent> context)
    {
      // süreç devam ederken context üzerinden direkt olarak IPublishEndpoint servisine bağlanmadan event gönderebiliriz.

      this.logger.LogInformation($"Order Recieved {context.Message.OrderId}");

      // veritabanına bağlan orderStatus durumunu recieved olarak işaretle
      // event fırlat, faturaya hazır olsun.

      // await context.Publish<IOrderProceededEvent>(new { CorrelationId = context.Message.CorrelationId, OrderId = context.Message.OrderId });

      await Task.CompletedTask;

    }
  }
}
