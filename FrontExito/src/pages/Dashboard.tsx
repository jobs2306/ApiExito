import React from 'react'
import { useNavigate } from 'react-router-dom'

const Dashboard = () => {
  const navigate = useNavigate()

  const handleLogout = () => {
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
        <button style={styles.button} onClick={() => navigate('/login')}>Cambiar Usuario</button>

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
