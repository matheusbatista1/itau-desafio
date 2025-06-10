namespace InvestmentControl.Domain.Entities;

public class Usuario
{
    public int Id { get; private set; }
    public string Nome { get; private set; }
    public string Email { get; private set; }
    public decimal PercentualCorretagem { get; private set; }

    public ICollection<Operacao> Operacoes { get; private set; } = null;
    public ICollection<Posicao> Posicoes { get; private set; } = null;

    public Usuario() { }

    public Usuario(string nome, string email, decimal percentualCorretagem)
    {
        if (string.IsNullOrWhiteSpace(nome) || string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Nome e e-mail são obrigatórios.");

        if (percentualCorretagem < 0)
            throw new ArgumentException("Corretagem não pode ser negativa.");

        Nome = nome;
        Email = email;
        PercentualCorretagem = percentualCorretagem;
    }
}