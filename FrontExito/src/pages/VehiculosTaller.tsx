import React, { useEffect, useState } from 'react'
import axios from 'axios'
import { useNavigate } from 'react-router-dom'

type ControlVehiculo = {
  id: number
  fecha: string
  tecnico_encargado: string
  vehiculoid: number
}

type Vehiculo = {
  id: number
  placa: string
  clienteid: number
}

type Cliente = {
  id: number
  nombre: string
}

type VehiculoEnTaller = {
  id: number
  fecha: string
  tecnico: string
  placa: string
  cliente: string
}

const VehiculosTaller = () => {
  const [vehiculos, setVehiculos] = useState<VehiculoEnTaller[]>([])
  const navigate = useNavigate()

  const fetchData = async () => {
    const api = axios.create({ baseURL: 'https://localhost:7136/api/' })
    try {
      const controlesResponse = await api.get<ControlVehiculo[]>('controlvehiculo/taller')
      const controles = controlesResponse.data

      const vehiculosData: VehiculoEnTaller[] = await Promise.all(
        controles.map(async (control) => {
          const vehiculoRes = await api.get<Vehiculo>(`vehiculo/${control.vehiculoid}`)
          const vehiculo = vehiculoRes.data

          const clienteRes = await api.get<Cliente>(`cliente/${vehiculo.clienteid}`)
          const cliente = clienteRes.data

          const fechaFormateada = new Date(control.fecha).toLocaleString('es-CO', {
            day: '2-digit',
            month: '2-digit',
            year: 'numeric',
            hour: 'numeric',
            minute: 'numeric',
            second: 'numeric',
            hour12: true,
          })

          return {
            id: control.id,
            fecha: fechaFormateada,
            tecnico: control.tecnico_encargado,
            placa: vehiculo.placa,
            cliente: cliente.nombre,
          }
        })
      )

      setVehiculos(vehiculosData)
    } catch (err) {
      console.error('Error al cargar vehículos en taller: ', err)
    }
  }

  useEffect(() => {
    fetchData()
  }, [])

  return (
    <div style={styles.container}>
      <div style={styles.header}>
        <img src="/logo.png" alt="Logo" style={styles.logo} />
        <h2 style={styles.title}>Lubricentro Éxito</h2>
        <button style={styles.boton } onClick={fetchData}>🔄️ Actualizar</button>
        <button style={styles.boton} onClick={() => navigate('/dashboard')}>🏠 Home</button>
      </div>

      <h3 style={styles.subtitle}>Vehículos en Taller</h3>

      {vehiculos.length === 0 ? (
        <p style={styles.text}>No hay vehículos actualmente.</p>
      ) : (
        <div style={styles.lista}>
          {vehiculos.map((v) => (
            <div key={v.id} style={styles.fila}>
              <div style={styles.columna}><strong>{v.fecha}</strong></div>
              <div style={styles.columna}>Placa: {v.placa}</div>
              <div style={styles.columna}>Técnico: {v.tecnico}</div>
              <div style={styles.columna}>Cliente: {v.cliente}</div>
              <div style={styles.columna}>
              <button 
              style={{ backgroundColor: '#00b32d'}}
              onClick={() => navigate(`/vehiculo/${v.id}`)}>
                Ver Vehículo
              </button>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  )
}

const styles: { [key: string]: React.CSSProperties } = {
  container: {
    backgroundColor: '#003366',
    minHeight: '100vh',
    padding: '20px',
    fontFamily: 'Arial, sans-serif',
    color: 'white'
  },
  header: {
    display: 'flex',
    alignItems: 'center',
    justifyContent: 'space-between',
    marginBottom: '20px'
  },
  logo: {
    width: '80px',
    height: '80px'
  },
  title: {
    flex: 1,
    textAlign: 'center',
    fontSize: '28px',
    margin: 0
  },
  subtitle: {
    textAlign: 'center',
    fontSize: '22px',
    marginBottom: '15px'
  },
  text: {
    textAlign: 'center'
  },
  lista: {
    display: 'flex',
    flexDirection: 'column',
    gap: '10px'
  },
  fila: {
    display: 'flex',
    backgroundColor: '#002b55',
    padding: '10px',
    border: '1px solid #ccc',
    borderRadius: '6px'
  },
  columna: {
    flex: 1,
    padding: '0 10px',
    display: 'flex',
    alignItems: 'center'
  },
  verBtn: {
    backgroundColor: '#007bff',
    color: '#fff',
    border: 'none',
    padding: '6px 12px',
    borderRadius: '6px',
    cursor: 'pointer'
  },
  boton: {
    backgroundColor: '#00aa00',
    color: 'white',
    border: 'none',
    padding: '10px 20px',
    borderRadius: '30px',
    fontWeight: 'bold',
    cursor: 'pointer'
  }
}

export default VehiculosTaller
