using Order.Repository;
using Order.Services;
using SqlSugar;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddScoped<ISqlSugarClient>(provider =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    var db = new SqlSugarScope(new ConnectionConfig()
    {
        ConnectionString = connectionString,
        DbType = DbType.MySql, 
        IsAutoCloseConnection = true,
    },
    db =>
    {
        //// 配置AOP
        //db.Aop.OnLogExecuting = (sql, pars) =>
        //{
        //    Console.WriteLine(sql); // 输出SQL
        //};
    });
    return db;
});
// 配置 Redis 
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var configuration = ConfigurationOptions.Parse("localhost:6379", true);
    return ConnectionMultiplexer.Connect(configuration);
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "rabbitMQ 订单测试 API", Version = "v1" });
});
builder.Services.AddHostedService<RabbitMqListener>();
builder.Services.AddScoped<IOrderConsumerService, OrderConsumerService>();
builder.Services.AddScoped<IOrderProducer, OrderProducer>();
builder.Services.AddScoped<RedisHelper>();
builder.Services.AddScoped<IOmsCustomerOrderRepository, OmsCustomerOrderRepository>();
builder.Services.AddScoped<IOmsCustomerOrderExtandRepository, OmsCustomerOrderExtandRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

// 6. 配置中间件管道
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "order v1"));
}
app.MapControllers();

app.Run();
