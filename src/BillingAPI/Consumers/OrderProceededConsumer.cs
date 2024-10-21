using MassTransit;
using Messaging;

namespace BillingAPI.Consumers
{
  // 3.sıradaki sürecimiz
  public class OrderProceededConsumer : IConsumer<IOrderProceededEvent>
  {
    private readonly ILogger<OrderProceededConsumer> logger;

    public OrderProceededConsumer(ILogger<OrderProceededConsumer> logger)
    {
      this.logger = logger;
    }
    public Task Consume(ConsumeContext<IOrderProceededEvent> context)
    {

      this.logger.LogInformation($"{context.Message.OrderId} faturalandırmaya hazır");

      // veritabanı işlemleri

      return Task.CompletedTask;
    }
  }
}
