using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging
{
  public interface IOrderRecievedEvent
  {
    Guid CorrelationId { get; set; } // Saga üzerinden ilgili order state ait state takibi için tanımladığımız Id
    int OrderId { get; set; }

    string OrderCode { get; set; }
  }
}
