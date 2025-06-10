const API_BASE = "https://localhost:7163/api/operacao";

// Retorna o preço médio por ativo para um usuário
export async function getPrecoMedio() {
  const response = await fetch(`${API_BASE}/preco-medio`);
  if (!response.ok) throw new Error("Erro ao buscar preço médio");
  return response.json();
}

// Retorna o faturamento da corretora com as corretagens
export async function getFaturamentoCorretora() {
  const response = await fetch(`${API_BASE}/corretora/faturamento`);
  if (!response.ok) throw new Error("Erro ao buscar faturamento da corretora");
  return response.json();
}