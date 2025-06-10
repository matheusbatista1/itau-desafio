type Cotacao = {
  ativoNome: string;
  quantidade: number;
  precoMedio: number;
  precoAtual: number;
  valorMercado: number;
  totalInvestido: number;
  lucroOuPrejuizo: number;
};

type Props = {
  cotacoes: Cotacao[];
};

const formatarMoeda = (valor: number) =>
  new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(valor);

export function MovementTable({ cotacoes }: Props) {
  return (
    <table className="w-full mt-6 border-separate border-spacing-y-2 shadow rounded-lg bg-white text-sm">
      <thead>
        <tr className="bg-[#F2EDE8] text-[#20160F] font-bold">
          <th className="px-4 py-2 text-left">Ativo</th>
          <th className="px-4 py-2 text-center">Quantidade</th>
          <th className="px-4 py-2 text-center">Preço Médio</th>
          <th className="px-4 py-2 text-center">Preço Atual do Ativo</th>
          <th className="px-4 py-2 text-center">Valor de Mercado</th>
          <th className="px-4 py-2 text-center">Total Investido</th>
          <th className="px-4 py-2 text-center">Lucro ou Prejuízo</th>
        </tr>
      </thead>
      <tbody>
        {cotacoes.map((linha, index) => (
          <tr key={index} className="hover:bg-[#F2EDE8] transition">
            <td className="px-4 py-2">{linha.ativoNome}</td>
            <td className="px-4 py-2 text-center">{linha.quantidade}</td>
            <td className="px-4 py-2 text-center">{formatarMoeda(linha.precoMedio)}</td>
            <td className="px-4 py-2 text-center">{formatarMoeda(linha.precoAtual)}</td>
            <td className="px-4 py-2 text-center">{formatarMoeda(linha.valorMercado)}</td>
            <td className="px-4 py-2 text-center">{formatarMoeda(linha.totalInvestido)}</td>
            <td className={`px-4 py-2 text-center font-medium ${linha.lucroOuPrejuizo >= 0 ? 'text-green-700' : 'text-red-600'}`}>
              {linha.lucroOuPrejuizo >= 0 ? '+' : '-'}
              {formatarMoeda(Math.abs(linha.lucroOuPrejuizo))}
            </td>
          </tr>
        ))}
      </tbody>
    </table>
  );
}