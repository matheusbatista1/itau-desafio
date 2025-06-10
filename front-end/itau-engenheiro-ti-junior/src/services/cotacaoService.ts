const API_BASE = "https://localhost:7163/api/cotacao";

// Retorna a última cotação por ativo
export async function getUltimaCotacaoByAtivo(ativoId: number) {
  const response = await fetch(`${API_BASE}/ativo/${ativoId}`);
  if (!response.ok) throw new Error("Erro ao buscar última cotação do ativo");
  return response.json();
}