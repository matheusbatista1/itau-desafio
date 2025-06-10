const API_BASE = "https://localhost:7163/api/posicao";

// Retorna a classificação top 10 posições e top 10 clientes que mais pagaram corretagens
export async function getClassificacao() {
  const response = await fetch(`${API_BASE}/classificacao`);
  if (!response.ok) throw new Error("Erro ao buscar classificação");
  return response.json();
}

// Retorna a posição de um usuário
export async function getPosicaoByUsuarioId(usuarioId: number) {
  const response = await fetch(`${API_BASE}/usuario/${usuarioId}`);
  if (!response.ok) throw new Error("Erro ao buscar posição do usuário");
  return response.json();
}