import { useEffect, useState } from "react";
import { getClassificacao } from "../services/posicaoService";
import { DashboardLayout } from "../components/DashboardLayout";
import { FaCrown, FaMedal } from 'react-icons/fa';

export default function InvestorRanking() {
  const [classificacao, setClassificacao] = useState<null | {
    classificacaoPosicao: { usuarioId: number; nomeCliente: string; valor: number }[];
    classificacaoCorretagemPaga: { usuarioId: number; nomeCliente: string; valor: number }[];
  }>(null);

  useEffect(() => {
    getClassificacao()
      .then(setClassificacao)
      .catch(console.error);
  }, []);

  if (!classificacao) {
    return (
      <DashboardLayout>
        <div className="p-8 mt-12">Carregando...</div>
      </DashboardLayout>
    );
  }

  return (
    <DashboardLayout>
      <div className="p-8 bg-gradient-to-br from-[#FCFAF7] to-[#F9F6F2] min-h-screen mt-12 text-[#20160F] font-sans">
        <h1 className="text-2xl font-bold mb-2">Placar de investidores</h1>
        <p className="mb-8 text-[#9A8F87] text-base">
          Veja quem são os maiores investidores e os clientes que mais pagaram taxas de corretagem na plataforma.
        </p>

        <section className="mb-14">
          <h2 className="text-xl font-bold mb-4 flex items-center gap-2 text-[#E66A19]">
            <FaCrown className="text-[#E66A19]" />
            Top 10 Investidores
          </h2>
          <div className="overflow-x-auto flex justify-center">
            <table className="min-w-[480px] max-w-xl w-full mx-auto text-sm text-left border-separate border-spacing-y-1 shadow rounded-lg">
              <caption className="sr-only">Ranking dos principais investidores</caption>
              <thead className="bg-[#F2EDE8] text-[#E66A19] font-bold">
                <tr>
                  <th scope="col" className="rounded-tl-lg px-3 py-2 text-center w-24">Classificação</th>
                  <th scope="col" className="px-3 py-2 text-left w-56">Cliente</th>
                  <th scope="col" className="rounded-tr-lg px-3 py-2 text-right w-32">Quantia</th>
                </tr>
              </thead>
              <tbody>
                {classificacao.classificacaoPosicao.map((item, i) => (
                  <tr
                    key={item.usuarioId}
                    className={`
                      ${i === 0 ? "bg-[#FFE5CC] font-bold" : i === 1 ? "bg-[#FFF3E0]" : i === 2 ? "bg-[#FFF8F0]" : "bg-[#F6F0EB]"}
                      hover:bg-[#F2EDE8] transition rounded-lg
                    `}
                  >
                    <td className="py-2 px-3 flex items-center gap-2 justify-center">
                      {i === 0 && <FaCrown className="text-[#E66A19]" title="1º lugar" />}
                      {i === 1 && <FaMedal className="text-[#E6B819]" title="2º lugar" />}
                      {i === 2 && <FaMedal className="text-[#C0C0C0]" title="3º lugar" />}
                      {i + 1}
                    </td>
                    <td className="py-2 px-3 text-left truncate">{item.nomeCliente}</td>
                    <td className="py-2 px-3 text-right">
                      {item.valor.toLocaleString('pt-BR', {
                        style: 'currency',
                        currency: 'BRL',
                      })}
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </section>

        <hr className="my-8 border-[#F2EDE8]" />

        <section>
          <h2 className="text-xl font-bold mb-4 flex items-center gap-2 text-[#E66A19]">
            <FaMedal className="text-[#E66A19]" />
            Top 10 Clientes por Taxa de Corretagem
          </h2>
          <div className="overflow-x-auto flex justify-center">
            <table className="min-w-[480px] max-w-xl w-full mx-auto text-sm text-left border-separate border-spacing-y-1 shadow rounded-lg">
              <caption className="sr-only">
                Ranking dos clientes que mais pagaram taxas de corretagem
              </caption>
              <thead className="bg-[#F2EDE8] text-[#E66A19] font-bold">
                <tr>
                  <th scope="col" className="rounded-tl-lg px-3 py-2 text-center w-24">Classificação</th>
                  <th scope="col" className="px-3 py-2 text-left w-56">Cliente</th>
                  <th scope="col" className="rounded-tr-lg px-3 py-2 text-right w-32">Taxas Pagas</th>
                </tr>
              </thead>
              <tbody>
                {classificacao.classificacaoCorretagemPaga.map((item, i) => (
                  <tr
                    key={item.usuarioId}
                    className={`
                      ${i === 0 ? "bg-[#FFE5CC] font-bold" : i === 1 ? "bg-[#FFF3E0]" : i === 2 ? "bg-[#FFF8F0]" : "bg-[#F6F0EB]"}
                      hover:bg-[#F2EDE8] transition rounded-lg
                    `}
                  >
                    <td className="py-2 px-3 flex items-center gap-2 justify-center">
                      {i === 0 && <FaCrown className="text-[#E66A19]" title="1º lugar" />}
                      {i === 1 && <FaMedal className="text-[#E6B819]" title="2º lugar" />}
                      {i === 2 && <FaMedal className="text-[#C0C0C0]" title="3º lugar" />}
                      {i + 1}
                    </td>
                    <td className="py-2 px-3 text-left truncate">{item.nomeCliente}</td>
                    <td className="py-2 px-3 text-right">
                      {item.valor.toLocaleString('pt-BR', {
                        style: 'currency',
                        currency: 'BRL',
                      })}
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </section>
      </div>
    </DashboardLayout>
  );
}