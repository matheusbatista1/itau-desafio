import { useEffect, useState } from "react";
import { DashboardLayout } from "../components/DashboardLayout";
import { getOperacoesByUsuarioId } from "../services/usuarioService";

const usuarioId = Number(localStorage.getItem("usuarioId"));

const formatarMoeda = (valor: number) =>
  new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(valor);

const formatarData = (data: string) => {
  const d = new Date(data);
  return d.toLocaleDateString("pt-BR");
};

const tipoOperacaoLabel = (tipo: number) => {
  if (tipo === 1) return "Compra";
  if (tipo === 2) return "Venda";
  return "Outro";
};

export default function Transactions() {
  const [transacoes, setTransacoes] = useState<any[]>([]);
  const [carregando, setCarregando] = useState(true);

  useEffect(() => {
    getOperacoesByUsuarioId(usuarioId)
      .then(setTransacoes)
      .catch(console.error)
      .finally(() => setCarregando(false));
  }, []);

  const totalCorretagens = transacoes.reduce((acc, t) => acc + (t.corretagem || 0), 0);

  return (
    <DashboardLayout>
      <div className="p-6 bg-[#FCFAF7] min-h-screen mt-20">
        <div className="flex flex-col md:flex-row md:justify-between md:items-center mb-2 gap-2">
          <div>
            <h1 className="text-2xl font-bold text-[#20160F]">Operações</h1>
            <p className="mt-2 mb-0 text-[#9A8F87] text-base">
              Visualize o histórico das suas transações realizadas na plataforma.
            </p>
          </div>
          <div className="flex flex-col items-end">
            <span className="text-[#9A8F87] text-sm mb-1">
              Total de corretagens pagas
            </span>
            <span className="bg-[#F2EDE8] border border-[#E66A19]/30 rounded-lg px-8 py-3 text-[#E66A19] text-xl font-extrabold shadow-sm mb-2">
              {formatarMoeda(totalCorretagens)}
            </span>
          </div>
        </div>
        <div className="flex gap-4 border-b text-[#9A8F87] text-sm font-medium mb-4 mt-6 items-center">
          <button
            aria-pressed={true}
            className="pb-2 border-b-2 border-[#E66A19] text-[#20160F]"
            disabled
          >
            Todos
          </button>
        </div>
        <div className="overflow-x-auto flex justify-center">
          {carregando ? (
            <div className="w-full text-center py-8 text-[#9A8F87] text-base">
              Carregando...
            </div>
          ) : transacoes.length === 0 ? (
            <div className="w-full text-center py-8 text-[#9A8F87] text-base">
              Nenhuma transação encontrada.
            </div>
          ) : (
            <table className="min-w-[1000px] max-w-5xl w-full mx-auto text-sm border-separate border-spacing-y-2 shadow rounded-lg bg-white">
              <thead className="bg-[#F2EDE8] text-[#20160F] font-bold">
                <tr>
                  <th className="px-4 py-2 text-center">Data</th>
                  <th className="px-4 py-2 text-center">Tipo de Operação</th>
                  <th className="px-4 py-2 text-center">Ativo</th>
                  <th className="px-4 py-2 text-center">Quantidade</th>
                  <th className="px-4 py-2 text-center">Valor por Unidade</th>
                  <th className="px-4 py-2 text-center">Corretagem</th>
                  <th className="px-4 py-2 text-center">Total</th>
                </tr>
              </thead>
              <tbody>
                {transacoes.map((t, i) => (
                  <tr
                    key={`${t.data}-${t.nomeAtivo}-${i}`}
                    className="bg-[#f6f4f3ca] hover:bg-[#F2EDE8] transition"
                  >
                    <td className="py-2 px-4 text-center">{formatarData(t.data)}</td>
                    <td className="py-2 px-4 text-center">{tipoOperacaoLabel(t.tipoOperacao)}</td>
                    <td className="py-2 px-4 text-center truncate max-w-[120px]">{t.nomeAtivo}</td>
                    <td className="py-2 px-4 text-center">{t.quantidade}</td>
                    <td className="py-2 px-4 text-center text-[#c09372] font-medium">{formatarMoeda(t.precoUnitario)}</td>
                    <td className="py-2 px-4 text-center">{formatarMoeda(t.corretagem)}</td>
                    <td className="py-2 px-4 text-center text-[#c09372] font-medium">{formatarMoeda(t.totalInvestido)}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          )}
        </div>
      </div>
    </DashboardLayout>
  );
}