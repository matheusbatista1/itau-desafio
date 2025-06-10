using InvestmentControl.Domain.Entities;
using System.Text.Json.Serialization;

public class Cotacao
{
    public int Id { get; private set; }
    public int AtivoId { get; private set; }
    public decimal PrecoUnitario { get; private set; }
    public DateTime DataHora { get; private set; }

    public Ativo Ativo { get; set; } = new Ativo();

    public Cotacao() { }

    [JsonConstructor]
    public Cotacao(int ativoId, decimal precoUnitario, DateTime dataHora)
    {
        if (precoUnitario <= 0)
            throw new ArgumentException("Preço da cotação deve ser maior que zero.");

        AtivoId = ativoId;
        PrecoUnitario = precoUnitario;
        DataHora = dataHora;
    }

    public void Atualizar(decimal novoPreco, DateTime novaDataHora)
    {
        if (novoPreco <= 0)
            throw new ArgumentException("Preço da cotação deve ser maior que zero.");

        if (PrecoUnitario != novoPreco || DataHora != novaDataHora)
        {
            PrecoUnitario = novoPreco;
            DataHora = novaDataHora;
        }
    }
}
