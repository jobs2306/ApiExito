import React from 'react'
import { useNavigate } from 'react-router-dom'
import { useEffect } from 'react'
import api from '../api'

const Dashboard = () => {

  useEffect(() => {
    const verificarToken = async () => {
      const token = localStorage.getItem('token')
      if (!token) {
        navigate('/login')
        return
      }

      try {
        await api.get('auth/test', {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        })
        // Token válido, no hace nada
      } catch {
        localStorage.removeItem('token')
        navigate('/login')
      }
    }

    verificarToken()
  }, [])

  const navigate = useNavigate()

  const handleLogout = () => {

    const confirmar = window.confirm('¿Estás seguro de que desea cerrar sesión?')

    if (!confirmar) return // Si el usuario cancela, no hace nada

    localStorage.removeItem('token') // Elimina el token guardado
    navigate('/login') // Redirige al login
  }

  return (
    <div style={styles.container}>
      <div style={styles.panel}>
        <img src="/logo.png" alt="Logo" style={styles.logo} />
        <h1 style={styles.title}>ÉXITO APP</h1>

        <button style={styles.button} onClick={() => navigate('/ingreso')}>Ingresar Vehículo</button>
        <button style={styles.button} onClick={() => navigate('/taller')}>Vehículos en taller</button>
        <button style={styles.button} onClick={() => navigate('/historial')}>Historial de Vehículos</button>
        {/* Botón de Registro de Usuario */}
        <button style={styles.button} onClick={() => navigate('/register')}>Registrar Usuario</button>

        {/* Botón de Cerrar Sesión */}
        <button style={styles.logoutButton} onClick={handleLogout}>🔒 Cerrar Sesión</button>
      </div>
    </div>
  )
}

const styles: { [key: string]: React.CSSProperties } = {
  container: {
    backgroundColor: '#003366',
    height: '100vh',
    display: 'flex',
    justifyContent: 'center',
    alignItems: 'center',
  },
  panel: {
    display: 'flex',
    flexDirection: 'column',
    alignItems: 'center',
    gap: '20px',
  },
  logo: {
    width: '80px',
    height: '80px',
    marginBottom: '10px',
  },
  title: {
    color: 'white',
    fontSize: '24px',
    fontWeight: 'bold',
    marginBottom: '20px',
  },
  button: {
    width: '250px',
    padding: '12px',
    backgroundColor: '#00aa00',
    color: 'white',
    fontSize: '16px',
    fontWeight: 'bold',
    border: 'none',
    borderRadius: '30px',
    cursor: 'pointer',
  },
  registerButton: {
    width: '250px',
    padding: '12px',
    backgroundColor: '#00aa00',
    color: 'white',
    fontSize: '16px',
    fontWeight: 'bold',
    border: 'none',
    borderRadius: '30px',
    cursor: 'pointer',
    marginTop: '20px',
  },
  logoutButton: {
    width: '250px',
    padding: '12px',
    backgroundColor: '#cc0000',
    color: 'white',
    fontSize: '16px',
    fontWeight: 'bold',
    border: 'none',
    borderRadius: '30px',
    cursor: 'pointer',
    marginTop: '20px',
  },
}

export default Dashboard
