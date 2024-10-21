using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using SagaService;

public class Program
{

  public static void Main(string[] args)
  {
    CreateHostBuilder(args).Build().Run(); // uygulamayı çalıştırdık.
  }


  public static IHostBuilder CreateHostBuilder(string[] args)
  {
    return Host.CreateDefaultBuilder(args).ConfigureServices((hostContext, services) =>
    {
      services.AddMassTransit(opt =>
      {
        opt.AddSagaStateMachine<OrderSagaStateMachine,OrderSagaState>().InMemoryRepository();

        opt.UsingRabbitMq((context, config) =>
        {
          config.Host(hostContext.Configuration.GetConnectionString("RabbitConn"));
          config.ReceiveEndpoint("order-saga", e =>
          {
            e.ConfigureSaga<OrderSagaState>(context);
          });
        });

      });

    });
  }

}
