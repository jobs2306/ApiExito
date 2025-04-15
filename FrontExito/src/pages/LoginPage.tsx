import React, { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { Eye, EyeOff } from 'lucide-react'
import './login.css'
import api from '../api' 
import { useEffect } from 'react'


const LoginPage = () => {
  const navigate = useNavigate()
  const [usuario, setUsuario] = useState('')
  const [password, setPassword] = useState('')
  const [showPassword, setShowPassword] = useState(false)
  const [error, setError] = useState<string | null>(null)

  useEffect(() => {
    const verificarToken = async () => {
      const token = localStorage.getItem('token')
  
      if (!token) return
  
      try {
        await api.get('auth/test', {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        })
        navigate('/dashboard')
      } catch {
        localStorage.removeItem('token')
      }
    }
  
    verificarToken()
  }, [])

  const handleLogin = async (e: React.FormEvent) => {
    e.preventDefault()
    setError(null)
  
    try {
      const response = await api.post('auth/login', {
        email: usuario,
        password,
      })
  
      const { token } = response.data
      localStorage.setItem('token', token.result)
      console.log('Token guardado:', token)
      alert('Sesión iniciada') 
      navigate('/dashboard')
    } 
    catch (err: any) {
      setError(err.response?.data || 'Error de conexión')
    }
  }
  

  return (
    <div className="login-contenedor">
      {/* Panel izquierdo */}
      <div className="login-left">
        <img src="/logo.png" alt="Logo" className="logo" />
        <h1 className="title">ÉXITO APP</h1>
        <p className="subtitle">Debe iniciar sesión</p>

        <form className="login-form" onSubmit={handleLogin}>
          <label>Usuario</label>
          <input
            type="text"
            value={usuario}
            onChange={(e) => setUsuario(e.target.value)}
            required
          />

          <label>Contraseña</label>
          <div className="password-wrapper">
            <input
              type={showPassword ? 'text' : 'password'}
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
            />
            <button type="button" onClick={() => setShowPassword(!showPassword)}>
              {showPassword ? <EyeOff size={18} color="#003366" /> : <Eye size={18} color="#003366" />}
            </button>
          </div>

          {error && <div className="error">{error}</div>}

          <button type="submit" className="btn-green">Iniciar Sesión</button>
        </form>
      </div>
    </div>
  )
}

export default LoginPage
