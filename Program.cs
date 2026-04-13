// var router = new Router();
// router.MapGet("/codecamp", (ctx) =>
// {
//     Console.WriteLine("Hello World");
//     return "anything";
// });

// router.MapPost("/codecamp", (ctx) =>
// {
//     Console.WriteLine("We hawt");
//     return "nah";
// });
// var server = new TcpServer(5006, router);
// await server.StartAsync();
using http;

var builder = WebApplicationFactory.CreateBuilder();
builder.Services.AddControllers();
// builder.Services.AddTransient<TestService, TestService>();
builder.Services.AddTransient<TestService>();
builder.Services.AddTransient<TestChildService>();

var app = builder.Build();
app.MapControllers();

var testService = app.Services.GetService<TestService>();
testService.Print();

app.MapGet("/codecamp", (ctx) => $"We are codecamp batch 3. Response is generated from route: {ctx.Path}");

// app.MapPost("/codecamp", (ctx) =>
// {
//    Console.WriteLine("Hello World");
//    return "anything"; 
// });

await app.RunAsync(5005);

// public class TestService
// {
//    public TestService()
//    {
      
//    }
//    public void Print()
//    {
//       Console.WriteLine("Hello World");
//    }
// }