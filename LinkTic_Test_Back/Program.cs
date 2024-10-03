using LinkTic_Test_Back.Application.Services;
using LinkTic_Test_Back.Domain.Entities;
using LinkTic_Test_Back.Domain.Interfaces;
using LinkTic_Test_Back.Domain.Services;
using LinkTic_Test_Back.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.WebSockets;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add context of db
builder.Services.AddDbContext<ECommerceContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add repositories
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>(); 

// Add services
builder.Services.AddScoped<IOrderService, OrderService>(); 
builder.Services.AddScoped<IProductService, ProductService>(); 

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("NewPolitics", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Add CORS middleware
app.UseCors("NewPolitics");

// Enable WebSocket support
app.UseWebSockets();


app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.MapGet("/ws", async context =>
{
    if (context.WebSockets.IsWebSocketRequest)
    {
        WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
        await HandleWebSocketConnection(webSocket);
    }
});

async Task HandleWebSocketConnection(WebSocket webSocket)
{
    byte[] buffer = new byte[1024 * 4];
    WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

    while (!result.CloseStatus.HasValue)
    {
        string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
        Console.WriteLine($"Received: {message}");

        await ProcessMessage(message, webSocket);
        result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
    }

    await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
}
async Task ProcessMessage(string message, WebSocket webSocket)
{
    try
    {
        var actionData = JsonConvert.DeserializeObject<Dictionary<string, string>>(message);

        if (actionData != null && actionData.ContainsKey("action"))
        {
            switch (actionData["action"])
            {
                case "getOrders":
                    // Obtener el contexto usando el contenedor de servicios
                    using (var scope = app.Services.CreateScope())
                    {
                        var context = scope.ServiceProvider.GetRequiredService<ECommerceContext>();
                        var orders = await GetOrdersFromDatabase(context);
                        var ordersJson = JsonConvert.SerializeObject(orders);
                        await webSocket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(ordersJson)), WebSocketMessageType.Text, true, CancellationToken.None);
                    }
                    break;

                default:
                    await webSocket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes("Acción no reconocida.")), WebSocketMessageType.Text, true, CancellationToken.None);
                    break;
            }
        }
        else
        {
            await webSocket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes("Mensaje mal formado.")), WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }
    catch (JsonException ex)
    {
        Console.WriteLine($"Error de deserialización: {ex.Message}");
        await webSocket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes("Error de deserialización.")), WebSocketMessageType.Text, true, CancellationToken.None);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error en el procesamiento del mensaje: {ex.Message}");
        await webSocket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes($"Error en el procesamiento: {ex.Message}")), WebSocketMessageType.Text, true, CancellationToken.None);
    }
}
static async Task<List<Order>> GetOrdersFromDatabase(ECommerceContext context)
{
    return await context.Orders.Include(o => o.OrderDetails).ToListAsync();
}


app.Run();


