using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.EntityFrameworkCore;
using rinha_2024_q1;
using rinha_2024_q1.Data;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.AddDbContextPool<RinhaDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")),
    poolSize: 100);

builder.Services.AddRequestTimeouts(options => options.DefaultPolicy = new RequestTimeoutPolicy{Timeout = TimeSpan.FromSeconds(60)});

var app = builder.Build();

app.MapGet("/healthcheck", () => "Working, thanks!");

var clientApi = app.MapGroup("/clientes");

clientApi.MapPost("/{id}/transacoes", ClienteService.HandleTransacao);

clientApi.MapGet("/{id}/extrato", ClienteService.HandleExtrato);

clientApi.MapGet("/", ClienteService.GetClientes);

app.Run();
