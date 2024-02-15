namespace rinha_2024_q1.Entities;

public class Extrato
{
    public Saldo Saldo { get; set; }
    public List<TransacaoDto> Ultimas_transacoes { get; set; }
}

public class Saldo
{
    public int Total { get; set; }
    public DateTime Data_Extrato { get; set; }
    public int Limite { get; set; }
}
