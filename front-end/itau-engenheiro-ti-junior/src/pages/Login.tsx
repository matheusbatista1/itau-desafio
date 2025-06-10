import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { getUsuarioIdByEmail } from "../services/usuarioService";

export default function Login() {
  const [email, setEmail] = useState("");
  const [senha, setSenha] = useState("");
  const [mostrarSenha, setMostrarSenha] = useState(false);

  const navigate = useNavigate();

  async function handleLogin(e: React.FormEvent<HTMLFormElement>) {
    e.preventDefault();

    const emailValido = email.trim().toLowerCase();

    try {
      const result = await getUsuarioIdByEmail(emailValido);
      if (result && result.id) {
        localStorage.setItem("usuarioId", result.id);
        localStorage.setItem("usuarioNome", result.nome);
        localStorage.setItem("usuarioEmail", emailValido);
        navigate("/dashboard");
      } else {
        alert("Usuário não encontrado. Popule o banco de dados com dados para conseguir utilizar.");
      }
    } catch {
      alert("Usuário não encontrado. Popule o banco de dados com dados para conseguir utilizar.");
    }
  }

  return (
    <div className="min-h-screen bg-[#FCFAF7] flex flex-col">
      <header className="flex items-center px-6 py-4 border-b">
        <img src="src\\assets\\images\\logo-itau.png" alt="logo" className="h-8 mr-2" />
        <span className="font-bold text-lg">Itaú Invest</span>
      </header>

      <main className="flex-1 flex items-center justify-center">
        <form onSubmit={handleLogin} className="w-full max-w-sm space-y-4 text-center">
          <h1 className="text-2xl font-bold text-[#20160F]">Bem vindo de volta</h1>

          <div className="text-left">
            <label htmlFor="email" className="sr-only">E-mail</label>
            <input
              id="email"
              type="email"
              placeholder="E-mail"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              className="w-full px-4 py-3 rounded-md bg-[#F6F0EB] placeholder:text-[#9A8F87]"
              required
            />
          </div>

          <div className="text-left relative">
            <label htmlFor="senha" className="sr-only">Senha</label>
            <input
              id="senha"
              type={mostrarSenha ? "text" : "password"}
              placeholder="Senha"
              value={senha}
              onChange={(e) => setSenha(e.target.value)}
              className="w-full px-4 py-3 rounded-md bg-[#F6F0EB] placeholder:text-[#9A8F87]"
            />
            <button
              type="button"
              onClick={() => setMostrarSenha(!mostrarSenha)}
              className="absolute right-3 top-1/2 transform -translate-y-1/2 text-sm text-[#9A8F87]"
            >
              {mostrarSenha ? "Ocultar" : "Mostrar"}
            </button>
          </div>

          <a href="#" className="text-sm text-[#9A8F87] block text-center">Esqueceu a senha?</a>

          <button
            type="submit"
            className="w-full bg-[#E66A19] text-white font-semibold py-3 rounded-md hover:opacity-90 transition"
          >
            Acessar carteira
          </button>
        </form>
      </main>
    </div>
  );
}