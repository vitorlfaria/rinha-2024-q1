using Microsoft.EntityFrameworkCore;
using rinha_2024_q1;
using rinha_2024_q1.Data;
using rinha_2024_q1.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<RinhaDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

app.MapGet("/healthcheck", () => "Working, thanks!");

var clientApi = app.MapGroup("/clientes");

clientApi.MapPost("/{id}/transacoes", (int id, TransacaoRequest transacaorequest) => {
    try
    {
        transacaorequest.Validate();
        var transacaoResponse = new TransacaoResponse(1000, -2345);
        return Results.Ok(transacaoResponse);
    }
    catch (Exception e)
    {
        return Results.UnprocessableEntity(e.Message);
    }
});

clientApi.MapGet("/{id}/extrato", (int id) => {
    var extrato = new Extrato
    {
        Saldo = new Saldo
        {
            Total = 1000,
            Data_Extrato = DateTime.Now,
            Limite = 5000
        },
        Ultimas_transacoes = new List<TransacaoDto>
        {
            new(1000, "credito", "Descrição", DateTime.Now),
            new(-2345, "debito", "Descrição", DateTime.Now)
        }
    };

    return Results.Ok(extrato);
});

clientApi.MapGet("/", ClienteService.GetClientes);

app.Run();
