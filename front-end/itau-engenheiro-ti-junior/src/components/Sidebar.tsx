// path: src/components/Sidebar.tsx
import { Link, useLocation, useNavigate } from "react-router-dom";
import { FaHome, FaExchangeAlt, FaTrophy, FaChartBar, FaSignOutAlt } from "react-icons/fa";

const links = [
  { to: "/dashboard", label: "Início", icon: <FaHome /> },
  { to: "/transactions", label: "Operações", icon: <FaExchangeAlt /> },
  { to: "/ranking", label: "Ranking", icon: <FaTrophy /> },
  { to: "/broker", label: "Visão da Corretora", icon: <FaChartBar /> }, // novo link
];

export function Sidebar() {
  const location = useLocation();
  const navigate = useNavigate();

  function handleLogout() {
    // Aqui você pode limpar tokens, localStorage, etc, se necessário
    navigate("/");
  }

  return (
    <aside className="bg-[#F6F4F1] w-64 min-h-screen p-4 mt-20" aria-label="Menu lateral">
      <nav className="flex flex-col gap-1">
        {links.map(link => {
          const active = location.pathname === link.to;
          return (
            <Link
              key={link.to}
              to={link.to}
              className={`
                flex items-center gap-3 px-3 py-2 rounded-md transition-colors
                text-[#20160F] text-base font-medium
                ${active ? "bg-[#eee8e1] font-semibold" : "hover:text-[#E66A19]"}
              `}
              aria-current={active ? "page" : undefined}
            >
              <span className="text-lg">{link.icon}</span>
              <span className="truncate">{link.label}</span>
            </Link>
          );
        })}
        {/* Botão de logout */}
        <button
          onClick={handleLogout}
          className="flex items-center gap-3 px-3 py-2 rounded-md transition-colors text-[#E66A19] text-base font-medium mt-6 hover:bg-[#EDE3DA]"
        >
          <FaSignOutAlt className="text-lg" />
          <span className="truncate">Sair</span>
        </button>
      </nav>
    </aside>
  );
}