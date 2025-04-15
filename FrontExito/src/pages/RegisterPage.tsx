import React, { useState, useEffect } from 'react'
import api from '../api'
import { useNavigate } from 'react-router-dom'
import './login.css'
import axios from 'axios' // 

const RegisterPage = () => {
  const navigate = useNavigate()

  const [usuario, setUsuario] = useState('')
  const [password, setPassword] = useState('')
  const [confirmPassword, setConfirmPassword] = useState('')
  const [rol, setRol] = useState('')
  const [roles, setRoles] = useState<string[]>([])
  const [error, setError] = useState<string | null>(null)
  const [success, setSuccess] = useState<string | null>(null)

  useEffect(() => {
    const verificarToken = async () => {
      const token = localStorage.getItem('token')
      if (!token) return navigate('/login')

      try {
        await api.get('auth/test', {
          headers: { Authorization: `Bearer ${token}` },
        })
      } catch {
        localStorage.removeItem('token')
        navigate('/login')
      }
    }

    const cargarRoles = async () => {
      try {
        const res = await api.get('auth/roles')
        setRoles(res.data)
      } catch {
        setRoles([])
      }
    }

    verificarToken()
    cargarRoles()
  }, [])

  const handleRegister = async (e: React.FormEvent) => {
    e.preventDefault()
    setError(null)
    setSuccess(null)

    if (password !== confirmPassword) {
      setError('Las contraseñas no coinciden')
      return
    }

    try {
      await api.post('auth/register', {
        email: usuario,
        password,
        role: rol,
      })

      setSuccess('Usuario registrado correctamente. Redirigiendo...')
      setTimeout(() => navigate('/login'), 1500)
    } catch (err: any) {

      if (axios.isAxiosError(err)) {

        if(err.status === 401) 
        {
          const mensaje = "No tiene el permiso necesario para hacer esto"
          alert(mensaje)
          return
        }
      }

      if (err.response) {
        const mensaje = Array.isArray(err.response.data)
          ? err.response.data[0].description
          : err.response.data
        alert(mensaje) 
      } else {
        setError('Error de conexión')
      }
    }
  }

  return (
    <div className="login-contenedor">
      <div className="login-left">
        <img src="/logo.png" alt="Logo" className="logo" />
        <h1 className="title">REGISTRO USUARIO</h1>
        <p className="subtitle">Ingrese los datos para registrar</p>
        <form className="login-form" onSubmit={handleRegister}>
          <label>Usuario</label>
          <input
            type="text"
            value={usuario}
            onChange={(e) => setUsuario(e.target.value)}
            required
          />

          <label>Contraseña</label>
          <input
            type="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
          />

          <label>Confirmar Contraseña</label>
          <input
            type="password"
            value={confirmPassword}
            onChange={(e) => setConfirmPassword(e.target.value)}
            required
          />

          <label>Rol</label>
          <select
            value={rol}
            onChange={(e) => setRol(e.target.value)}
            required
          >
            <option value="">Seleccione un rol</option>
            {roles.map((r) => (
              <option key={r} value={r}>{r}</option>
            ))}
          </select>

          {error && <div className="error">{error}</div>}
          {success && <div className="success">{success}</div>}

          <button type="submit" className="btn-green">Registrarse</button>
          
          <button type="submit" className="btn-green" onClick={() => navigate('/dashboard')}>🏠 Home</button>

        </form>
      </div>
    </div>
  )
}
export default RegisterPage
