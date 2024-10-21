using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SagaService
{
  // Persistance olarak State kullanıldığında EF gibi yapılarda bir State Entity haline geliyor.
  public class OrderSagaState : SagaStateMachineInstance
  {
    public Guid CorrelationId { get; set; } // Her bir state nesnesine ait unique Id, state geçişlerinin hangi nesne üzerinden hareket ettiğini bu Id belirler

    public State CurrentState { get; set; } // Herbir transation işleminde buradaki state takip edilip güncellenir.

    public int OrderId { get; set; }
    public string OrderCode { get; set; }

  }
}
