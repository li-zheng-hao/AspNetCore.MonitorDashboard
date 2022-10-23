using Prometheus;
using Prometheus.SystemMetrics;
using Webapi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSystemMetrics();
builder.Services.AddHostedService<MetricsService>();
// ...
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
//http请求的中间件
app.UseHttpMetrics();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    //映射监控地址为  /metrics
    endpoints.MapMetrics();
    endpoints.MapControllers();
});

app.Run();