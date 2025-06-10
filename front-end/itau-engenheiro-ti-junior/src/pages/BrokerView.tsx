import { useEffect, useState } from "react";
import { DashboardLayout } from "../components/DashboardLayout";
import { getFaturamentoCorretora } from "../services/operacaoService";

const formatarMoeda = (valor: number) =>
  new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(valor);

export default function BrokerView() {
  const [dados, setDados] = useState<{
    totalCorretagens: number;
    totalClientes: number;
    totalTransacoes: number;
  } | null>(null);

  useEffect(() => {
    getFaturamentoCorretora()
      .then(setDados)
      .catch(console.error);
  }, []);

  if (!dados) {
    return (
      <DashboardLayout>
        <div className="p-8 mt-20">Carregando...</div>
      </DashboardLayout>
    );
  }

  return (
    <DashboardLayout>
      <div className="p-8 mt-20">
        <h1 className="text-2xl font-bold text-[#20160F] mb-6">Visão da Corretora</h1>
        <div className="grid grid-cols-1 md:grid-cols-3 gap-6 mb-10">
          <div className="bg-[#F2EDE8] rounded-lg p-6 flex flex-col items-center shadow">
            <span className="text-[#9A8F87] text-sm mb-2">Total de corretagens recebidas</span>
            <span className="text-2xl font-bold text-[#E66A19]">{formatarMoeda(dados.totalCorretagens)}</span>
          </div>
          <div className="bg-[#F2EDE8] rounded-lg p-6 flex flex-col items-center shadow">
            <span className="text-[#9A8F87] text-sm mb-2">Total de clientes</span>
            <span className="text-2xl font-bold text-[#20160F]">{dados.totalClientes}</span>
          </div>
          <div className="bg-[#F2EDE8] rounded-lg p-6 flex flex-col items-center shadow">
            <span className="text-[#9A8F87] text-sm mb-2">Total de transações</span>
            <span className="text-2xl font-bold text-[#20160F]">{dados.totalTransacoes}</span>
          </div>
        </div>
        <div className="bg-white rounded-lg shadow p-6">
          <h2 className="text-lg font-bold mb-4 text-[#20160F]">Resumo financeiro</h2>
          <ul className="text-[#20160F] space-y-2">
            <li>
              <span className="font-medium">Corretagens:</span> {formatarMoeda(dados.totalCorretagens)}
            </li>
            <li>
              <span className="font-medium">Total de clientes:</span> {dados.totalClientes}
            </li>
            <li>
              <span className="font-medium">Total de transações:</span> {dados.totalTransacoes}
            </li>
          </ul>
        </div>
      </div>
    </DashboardLayout>
  );
}