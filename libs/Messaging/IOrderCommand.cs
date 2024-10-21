using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging
{
  // Sipariş verme sürecini başlatacak sınıfa ait parametreler
  public interface IOrderCommand
  {
    int OrderId { get; set; }
    string OrderCode { get; set; }
  }
}
