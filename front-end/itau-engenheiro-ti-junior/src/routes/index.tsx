// path: src/routes/index.tsx
import { BrowserRouter, Routes, Route } from 'react-router-dom'
import Dashboard from '../pages/Dashboard'
import Login from '../pages/Login'
import Transactions from '../pages/Transactions'
import InvestorRanking from '../pages/Ranking'
import BrokerView from "../pages/BrokerView";

export default function AppRoutes() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Login />} />
        <Route path="/dashboard" element={<Dashboard />} />
        <Route path="/transactions" element={<Transactions />} />
        <Route path="/ranking" element={<InvestorRanking />} />
        <Route path="/broker" element={<BrokerView />} />
        <Route path="*" element={<Dashboard />} />
      </Routes>
    </BrowserRouter>
  )
}
