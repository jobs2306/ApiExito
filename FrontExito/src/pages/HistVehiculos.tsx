import React, { useEffect, useState } from 'react'
import axios from 'axios'
import { useNavigate } from 'react-router-dom'

type ControlVehiculo = {
  id: number
  fecha: string
  tecnico_encargado: string
  vehiculoid: number
  fecha_salida: string
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
  fecha_salida: string
}

const HistVehiculos = () => {
  const [vehiculos, setVehiculos] = useState<VehiculoEnTaller[]>([])
  const [filtro, setFiltro] = useState({
    fecha: '',
    fecha_salida: '',
    placa: '',
    tecnico: '',
    cliente: ''
  })
  const navigate = useNavigate()

  const fetchData = async () => {
    const api = axios.create({ baseURL: 'https://localhost:7136/api/' })
    try {
      const controlesResponse = await api.get<ControlVehiculo[]>('controlvehiculo')
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

          const fechaSalidaFormateada = control.fecha_salida
            ? new Date(control.fecha_salida).toLocaleString('es-CO', {
                day: '2-digit',
                month: '2-digit',
                year: 'numeric',
                hour: 'numeric',
                minute: 'numeric',
                second: 'numeric',
                hour12: true,
              })
            : ''

          return {
            id: control.id,
            fecha: fechaFormateada,
            fecha_salida: fechaSalidaFormateada,
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

  const vehiculosFiltrados = vehiculos.filter((v) =>
    v.fecha.toLowerCase().includes(filtro.fecha.toLowerCase()) &&
    v.fecha_salida.toLowerCase().includes(filtro.fecha_salida.toLowerCase()) &&
    v.placa.toLowerCase().includes(filtro.placa.toLowerCase()) &&
    v.tecnico.toLowerCase().includes(filtro.tecnico.toLowerCase()) &&
    v.cliente.toLowerCase().includes(filtro.cliente.toLowerCase())
  )

  return (
    <div style={styles.container}>
      <div style={styles.header}>
        <img src="/logo.png" alt="Logo" style={styles.logo} />
        <h2 style={styles.title}>Lubricentro Éxito</h2>
        <button style={styles.boton} onClick={fetchData}>🔄️ Actualizar</button>
        <button style={{ ...styles.boton, marginLeft: '10px' }} onClick={() => navigate('/dashboard')}>🏠 Home</button>
      </div>

      <h3 style={styles.subtitle}>Historial de Vehículos</h3>

      <div style={{ display: 'flex', gap: '10px', marginBottom: '15px' }}>
        <input placeholder="Fecha Ingreso" style={styles.filtroInput} onChange={e => setFiltro({ ...filtro, fecha: e.target.value })} />
        <input placeholder="Placa" style={styles.filtroInput} onChange={e => setFiltro({ ...filtro, placa: e.target.value })} />
        <input placeholder="Técnico" style={styles.filtroInput} onChange={e => setFiltro({ ...filtro, tecnico: e.target.value })} />
        <input placeholder="Cliente" style={styles.filtroInput} onChange={e => setFiltro({ ...filtro, cliente: e.target.value })} />
        <input placeholder="Fecha Salida" style={styles.filtroInput} onChange={e => setFiltro({ ...filtro, fecha_salida: e.target.value })} />
      </div>

      {vehiculosFiltrados.length === 0 ? (
        <p style={styles.text}>No hay vehículos que coincidan con el filtro.</p>
      ) : (
        <div style={styles.lista}>
          {vehiculosFiltrados.map((v) => (
            <div key={v.id} style={styles.fila}>
              <div style={styles.columna}><strong>{v.fecha}</strong></div>
              <div style={styles.columna}>Placa: {v.placa}</div>
              <div style={styles.columna}>Técnico: {v.tecnico}</div>
              <div style={styles.columna}>Cliente: {v.cliente}</div>
              <div style={styles.columna}><strong>{v.fecha_salida}</strong></div>
              <div style={styles.columnaBotones}>
                <button style={styles.botonAccion} onClick={() => navigate(`/vehiculo/${v.id}`)}>
                  Ver Vehículo
                </button>
              </div>
              <div style={styles.columnaBotones}>
                <button style={styles.botonAccion} onClick={() => navigate(`/informe/${v.id}`)}>
                  Informe
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
  columnaBotones: {
    flex: 1,
    display: 'flex',
    alignItems: 'center',
    justifyContent: 'flex-end',
    gap: '10px'
  },
  botonAccion: {
    backgroundColor: '#00b32d',
    color: 'white',
    border: 'none',
    padding: '8px 16px',
    borderRadius: '10px',
    fontWeight: 'bold',
    cursor: 'pointer',
    minWidth: '120px',
    textAlign: 'center'
  },
  boton: {
    backgroundColor: '#00aa00',
    color: 'white',
    border: 'none',
    padding: '10px 20px',
    borderRadius: '30px',
    fontWeight: 'bold',
    cursor: 'pointer'
  },
  filtroInput: {
    flex: 1,
    padding: '6px',
    borderRadius: '6px',
    border: '1px solid #ccc'
  }
}

export default HistVehiculos
