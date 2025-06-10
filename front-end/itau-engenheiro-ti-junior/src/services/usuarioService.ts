const API_BASE = "https://localhost:7163/api/usuario";

// Retorna todas operações de um usuário
export async function getOperacoesByUsuarioId(usuarioId: number) {
  const response = await fetch(`${API_BASE}/${usuarioId}/operacoes`);
  if (!response.ok) throw new Error("Erro ao buscar operações do usuário");
  return response.json();
}

// Retorna a posição global de um usuário
export async function getPosicaoGlobalByUsuarioId(usuarioId: number) {
  const response = await fetch(`${API_BASE}/${usuarioId}/global`);
  if (!response.ok) throw new Error("Erro ao buscar posição global do usuário");
  return response.json(); 
}

// Retorna o total de corretagem de um usuário
export async function getTotalCorretagemByUsuarioId(usuarioId: number) {
  const response = await fetch(`${API_BASE}/${usuarioId}/corretagem`);
  if (!response.ok) throw new Error("Erro ao buscar total de corretagem do usuário");
  return response.json(); 
}

// Retorna as cotações de todos ativos do usuário
export async function getCotacoesByUsuarioId(usuarioId: number) {
  const response = await fetch(`${API_BASE}/${usuarioId}/cotacoes`);
  if (!response.ok) throw new Error("Erro ao buscar cotações do usuário");
  return response.json();
}

// Retorna o Id do usuário a partir do email informado
export async function getUsuarioIdByEmail(email: string) {
  const response = await fetch(`${API_BASE}/${encodeURIComponent(email)}`);
  if (!response.ok) throw new Error("Erro ao buscar usuário pelo email");
  return response.json();
}