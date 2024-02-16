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
                Data_Extrato = DateTime.Now,
                Total = cliente.Saldo
            },
            Ultimas_transacoes = transacaoDtos
        };

        return Results.Ok(extrato);
    }
}
