using MassTransit;
using Messaging;
using SagaService.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SagaService
{
  // Workflow sınıfımız
  // tüm state geçişleri ve saga üzerinden orkestrayon bu servis üzerinde koşar.
  // Bu bir servis sınıfı görevi görür.
  public class OrderSagaStateMachine : MassTransitStateMachine<OrderSagaState>
  {
    public State Received { get; set; }

    public State Proceeded { get; set; }

    public Event<IOrderCommand> OrderCommand { get; set; }

    public Event<IOrderRecievedEvent> OrderRecieved { get; set; }
    public Event<IOrderProceededEvent> OrderProceeded { get; set; }


    public OrderSagaStateMachine()
    {
      InstanceState(i => i.CurrentState);

      // Başlangıçta OrderState takibi için CorrelationId oluşturduk
      // Daha sonra state değişimlerinde bu CorrelationId üzerinden çalışmamız gerekiyor ki OrderState farklı bir nesne state gibi görünmesin yoksa db yeni kayıt atar. Doğru bir state takibi yapamayız.

      // OrderCode kod bazlı CorrelationId unique olarak oluştur.
      // OrderId unique değilse bu durumda  CorrelationId üretimde sıkıntı yaşayabiliriz.

      Event(() => OrderCommand, c => c.CorrelateBy(state =>
        state.OrderId.ToString(), context => context.Message.OrderId.ToString()).SelectId(s => Guid.NewGuid()));

      Event(() => OrderProceeded, c => c.CorrelateById(s => s.Message.CorrelationId));


      Initially(
        When(OrderCommand)
        .Then(context =>
        {
          //Saga State git Event den gelen bilgi ile eşle.
          context.Instance.OrderCode = context.Data.OrderCode;
          context.Instance.OrderId = context.Data.OrderId;

        }) // Event sonrası bir eylem bir action tetiklemek için. 
        .ThenAsync(context =>
          Console.Out.WriteLineAsync($"Saga Order Recieved {context.Data.OrderId}"))
        .TransitionTo(Received)
        .Publish(context => new OrderRecievedEvent
        {
          CorrelationId = context.Instance.CorrelationId,
          OrderId = context.Instance.OrderId,
          OrderCode = context.Instance.OrderCode

        }));


      // Recieved aşamasında iken
      During(Received,
        When(OrderRecieved)
        .ThenAsync(context =>
           Console.Out.WriteLineAsync($"Saga Order Proceeded {context.Data.OrderId}")
        )
        .TransitionTo(Proceeded)
        .Publish(context => new OrderProcededEvent
        {
          CorrelationId = context.Instance.CorrelationId,
          OrderId = context.Instance.OrderId,

        })
        .Then(context =>
        {
          Console.Out.WriteLineAsync($"Saga Order Completed {context.Data.OrderId}");
        })
       .Finalize());


      // SetCompletedWhenFinalized(); // State Completed yapılır.
      // Bu methodu çalıştırınca artık tüm süreç bittiği için state takibi sonlanmış olup veritabanından state kaydı silinir.

    }


  }
}
