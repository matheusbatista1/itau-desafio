import { useEffect, useState } from "react";
import { DashboardLayout } from '../components/DashboardLayout';
import { CardSummary } from '../components/CardSummary';
import { MovementTable } from '../components/MovementTable';
import { getPosicaoByUsuarioId } from "../services/posicaoService";
import { getPosicaoGlobalByUsuarioId, getTotalCorretagemByUsuarioId, getCotacoesByUsuarioId } from "../services/usuarioService";

const usuarioId = Number(localStorage.getItem("usuarioId"));

const formatarMoeda = (valor: number) =>
  new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(valor);

export default function Dashboard() {
  const [nomeUsuario, setNomeUsuario] = useState<string>("");
  const [totalInvestido, setTotalInvestido] = useState<number>(0);
  const [totalCorretagem, setTotalCorretagem] = useState<number>(0);
  const [posicaoGlobal, setPosicaoGlobal] = useState<number>(0);
  const [cotacoes, setCotacoes] = useState<any[]>([]);
  const [carregando, setCarregando] = useState(true);

  useEffect(() => {
  const usuarioId = Number(localStorage.getItem("usuarioId"));
    if (!usuarioId) {
      window.location.href = "/";
      return;
    }
    async function fetchData() {
      try {
        const posicoes = await getPosicaoByUsuarioId(usuarioId);
        const somaPosicaoGlobal = posicoes.reduce(
          (acc: number, ativo: any) => acc + (ativo.valorTotal || 0),
          0
        );
        setPosicaoGlobal(somaPosicaoGlobal);

        const global = await getPosicaoGlobalByUsuarioId(usuarioId);
        setNomeUsuario(global.nomeUsuario);
        setTotalInvestido(global.totalInvestido);

        const corretagem = await getTotalCorretagemByUsuarioId(usuarioId);
        setTotalCorretagem(corretagem.totalCorretagem);

        const cotacoesData = await getCotacoesByUsuarioId(usuarioId);
        setCotacoes(cotacoesData);
      } catch (err) {
        console.error(err);
      } finally {
        setCarregando(false);
      }
    }
    fetchData();
  }, []);

  if (carregando) {
    return (
      <DashboardLayout>
        <div className="p-8 mt-20">Carregando...</div>
      </DashboardLayout>
    );
  }

  return (
    <DashboardLayout>
      <h2 className="text-2xl font-bold text-[#20160F] mt-20 mb-2">
        Meus investimentos
      </h2>
      <p className="text-[#9A8F87] mb-6">
        Acompanhe seus investimentos, lucros e cotações atuais.
      </p>
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4 mt-2">
        <CardSummary title="Investidor" value={nomeUsuario} />
        <CardSummary
          title="Investimentos"
          value={<span className="text-[#E66A19] font-bold">{formatarMoeda(totalInvestido)}</span>}
        />
        <CardSummary title="Corretagem paga" value={formatarMoeda(totalCorretagem)} />
        <CardSummary
          title="Posição Global"
          value={<span className="text-green-700 font-bold">{formatarMoeda(posicaoGlobal)}</span>}
        />
      </div>
      <h2 className="mt-10 font-bold text-lg text-[#20160F]">Cotações</h2>
      <p className="text-[#9A8F87] mb-6 mt-2">
        Confira as cotações atuais dos seus investimentos.
      </p>
      <div className="bg-white rounded-lg shadow p-4 mt-2 overflow-x-auto">
        <MovementTable cotacoes={cotacoes} />
      </div>
    </DashboardLayout>
  );
}