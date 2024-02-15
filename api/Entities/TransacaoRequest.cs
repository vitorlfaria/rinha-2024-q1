namespace rinha_2024_q1.Entities;

public sealed record TransacaoRequest
{
    public int Valor { get; set; }
    public string Tipo { get; set; }
    public string Descricao { get; set; }

    public void Validate()
    {
        AssertionConcern.AssertRequired(Descricao, "Descrição é obrigatória");
        AssertionConcern.AssertRequired(Tipo, "Tipo é obrigatório");
        AssertionConcern.AssertLength(Descricao, 1, 10, "Descrição deve ter entre 1 e 10 caracteres");
        AssertionConcern.AssertLength(Tipo, 1, 1, "Tipo deve ter 1 caractere");
        AssertionConcern.AssertPositive(Valor, "Valor deve ser positivo");
    }
}
