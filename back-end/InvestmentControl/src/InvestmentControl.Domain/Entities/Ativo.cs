namespace InvestmentControl.Domain.Entities;

public class Ativo
{
    public int Id { get; private set; }
    public string Codigo { get; private set; }
    public string Nome { get; private set; }

    public ICollection<Operacao> Operacoes { get; private set; } = null;
    public ICollection<Posicao> Posicoes { get; private set; } = null;
    public ICollection<Cotacao> Cotacoes { get; private set; } = null;

    public Ativo() { }

    public Ativo(string codigo, string nome)
    {
        if (string.IsNullOrWhiteSpace(codigo) || string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Código e nome são obrigatórios.");

        Codigo = codigo.ToUpper();
        Nome = nome;
    }
}