using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging
{
  public interface IOrderProceededEvent
  {
    Guid CorrelationId { get; set; }
    int OrderId { get; set; }
  }
}
