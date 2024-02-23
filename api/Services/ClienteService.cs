using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using rinha_2024_q1.Data;
using rinha_2024_q1.Entities;

namespace rinha_2024_q1;

public static class ClienteService
{
    public static async Task<IResult> GetClientes([FromServices] RinhaDbContext context)
    {
        var clientes = await context.Clientes.ToListAsync();
        return Results.Ok(clientes);
    }

    public static async Task<IResult> HandleExtrato(int id, [FromServices] RinhaDbContext context)
    {
        Cliente cliente = await context.Clientes.FindAsync(id);
        if (cliente == null)
        {
            return Results.NotFound("Cliente não encontrado");
        }

        List<TransacaoDto> transacaoDtos = [];
        List<Transacao> transacoes = await context.Transacoes
            .Where(t => t.ClienteId == id)
            .OrderByDescending(t => t.Realizada_Em)
            .Take(10)
            .ToListAsync();
        
        for (int i = 0; i < transacoes.Count; i++)
        {
            TransacaoDto transacaoDto = new(
                transacoes[i].Valor,
                transacoes[i].Tipo,
                transacoes[i].Descricao,
                transacoes[i].Realizada_Em
            );
            transacaoDtos.Add(transacaoDto);
        }

        Extrato extrato = new()
        {
            Saldo = new Saldo
            {
                Limite = cliente.Limite,
                Data_Extrato = DateTime.Now.ToUniversalTime(),
                Total = cliente.Saldo
            },
            Ultimas_transacoes = transacaoDtos
        };

        return Results.Ok(extrato);
    }

    public static async Task<IResult> HandleTransacao(int id, [FromBody] TransacaoRequest transacaoRequest, [FromServices] RinhaDbContext context)
    {
        try
        {
            transacaoRequest.Validate();
            var pgTransaction = context.Database.BeginTransaction();
            Cliente cliente = await context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return Results.NotFound("Cliente não encontrado");
            }

            if(transacaoRequest.Tipo == "c")
            {
                cliente.Saldo += transacaoRequest.Valor;
            }
            else if(transacaoRequest.Tipo == "d")
            {
                int novoSaldo = cliente.Saldo - transacaoRequest.Valor;
                if(novoSaldo < (cliente.Limite * -1))
                {
                    return Results.UnprocessableEntity("Limite excedido");
                }
                cliente.Saldo = novoSaldo;
            }

            context.Transacoes.Add(new Transacao
            {
                ClienteId = id,
                Valor = transacaoRequest.Valor,
                Tipo = transacaoRequest.Tipo,
                Descricao = transacaoRequest.Descricao,
                Realizada_Em = DateTime.Now.ToUniversalTime()
            });

            await context.SaveChangesAsync();
            await pgTransaction.CommitAsync();

            return Results.Ok(new TransacaoResponse(cliente.Limite, cliente.Saldo));
        }
        catch (Exception e)
        {
            return Results.UnprocessableEntity(e.Message);
        }
    }
}
