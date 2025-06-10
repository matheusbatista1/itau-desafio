// path: src/components/Header.tsx
import { useNavigate } from "react-router-dom";
import { FaSearch } from 'react-icons/fa';

export function Header() {
  const navigate = useNavigate();

  return (
    <header className="fixed top-0 left-0 w-full z-50 bg-white flex justify-between items-center px-8 py-4 border-b">
      <div
        className="flex items-center gap-3 cursor-pointer"
        onClick={() => navigate("/dashboard")}
        tabIndex={0}
        role="button"
        aria-label="Ir para a página inicial"
        onKeyDown={e => { if (e.key === "Enter" || e.key === " ") navigate("/dashboard"); }}
      >
        <img 
          src="src/assets/images/logo-itau.png" 
          alt="Itaú logo in orange and white, next to the text Itaú Invest, representing a professional and trustworthy financial environment" 
          className="h-8 w-8" 
        />
        <h1 className="text-2xl font-bold text-[#20160F]">Itaú Invest</h1>
      </div>
      <button onClick={() => navigate("/lookup")}>
        <FaSearch />
      </button>
    </header>
  );
}
