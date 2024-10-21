using MassTransit;
using Messaging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OrderAPI.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class OrdersController : ControllerBase
  {
    private readonly ISendEndpointProvider sendEndpointProvider;

    public OrdersController(ISendEndpointProvider sendEndpointProvider)
    {
      this.sendEndpointProvider = sendEndpointProvider;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder()
    {
      var uri = new Uri($"queue:order-saga"); // queue: olması önemli yoksa uri hatası alırız.

      ISendEndpoint sendEndpoint = await this.sendEndpointProvider.GetSendEndpoint(uri);

      Random rdm = new Random();

      int randomId = rdm.Next(1, 1000);

      await sendEndpoint.Send<IOrderCommand>(new { OrderId = randomId, OrderCode = $"ORD-{randomId}" });

      return Ok();
    }
  }
}
