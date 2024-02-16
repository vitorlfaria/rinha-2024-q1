using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using rinha_2024_q1.Data;

namespace rinha_2024_q1;

public static class ClienteService
{
    public static async Task<IResult> GetClientes([FromServices] RinhaDbContext context)
    {
        // Busca todos os clientes no banco de dados
        var clientes = await context.Clientes.ToListAsync();
        return Results.Ok(clientes);
    }
}
