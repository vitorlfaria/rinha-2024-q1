namespace rinha_2024_q1.Entities;

public class Transacao(int clientId, int valor, string tipo, string descricao)
{
    public int Valor { get; set; } = valor;
    public string Tipo { get; set; } = tipo;
    public string Descricao { get; set; } = descricao;
    public DateTime Realizda_Em { get; set; } = DateTime.Now;
    public int ClienteId { get; set; } = clientId;

    public void Validate()
    {
        AssertionConcern.AssertRequired(Descricao, "Descrição é obrigatória");
        AssertionConcern.AssertRequired(Tipo, "Tipo é obrigatório");
        AssertionConcern.AssertLength(Descricao, 1, 10, "Descrição deve ter entre 1 e 10 caracteres");
        AssertionConcern.AssertLength(Tipo, 1, 1, "Tipo deve ter 1 caractere");
        AssertionConcern.AssertPositive(Valor, "Valor deve ser positivo");
    }
}
