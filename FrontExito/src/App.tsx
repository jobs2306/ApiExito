import { Routes, Route, Navigate } from 'react-router-dom'
import LoginPage from './pages/LoginPage'
import RegisterPage from './pages/RegisterPage'
import Dashboard from './pages/Dashboard'
import RegistroEntrada from './pages/RegistroEntrada'
import VehiculosTaller from './pages/VehiculosTaller'
import VerVehiculo from './pages/VerVehiculo'
import HistVehiculos from './pages/HistVehiculos'


function App() {
  const isAuthenticated = !!localStorage.getItem('token')

  return (
    <Routes>
      <Route path="/" element={isAuthenticated ? <Navigate to="/dashboard" /> : <Navigate to="/login" />} />
      <Route path="/login" element={<LoginPage />} />
      <Route path="/register" element={<RegisterPage />} />
      <Route path="/dashboard" element={isAuthenticated ? <Dashboard /> : <Navigate to="/login" />} />

      <Route path="/ingreso" element={<RegistroEntrada />} />
      <Route path="/taller" element={<VehiculosTaller />} />

      <Route path="/vehiculo/:id" element={<VerVehiculo />} />
      <Route path="/historial" element={<HistVehiculos />} />
    </Routes>
  )
}

export default App
