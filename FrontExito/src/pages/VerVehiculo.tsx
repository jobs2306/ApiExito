import { useEffect } from 'react'
import { useState } from 'react'
import { useParams, useNavigate } from 'react-router-dom'
import api from '../api'
import './RegistroEntrada.css'
import axios from 'axios'

const VerVehiculo = () => {
  const { id } = useParams()
  const navigate = useNavigate()

  const [control, setControl] = useState<any>(null)
  const [vehiculo, setVehiculo] = useState<any>(null)
  const [cliente, setCliente] = useState<any>(null)
  const [fechaSalida, setFechaSalida] = useState('')

  const accesoriosKeys = [
    'espejoDerecho', 'espejoIzquierdo', 'espejoRetro', 'rejillaAire', 'tapete', 'plumillas', 'memoriaUsb', 'tapaGasolian', 'bateria',
    'radio', 'vidriosPuertas', 'panoramicoDel', 'panoramicoTra', 'llantaRep', 'placaDel', 'placaTra', 'medidorAceite', 'tapasLlanta',
    'luzDelDer', 'luzDelIz1', 'luzTrasDer', 'luzTrasIz1', 'rayones', 'pangones', 'kitCarrera', 'tapaRadiador', 'marquillaCromada'
  ]

  useEffect(() => {
    const fetchAll = async () => {
      try {
        const ctrlRes = await api.get(`controlvehiculo/${id}`)
        const ctrlData = ctrlRes.data

        const accesoriosIniciales = accesoriosKeys.reduce((acc, key) => {
          acc[key] = ctrlData[key] === true
          return acc
        }, {} as Record<string, boolean>)

        setControl({ ...ctrlData, ...accesoriosIniciales })

        if (ctrlData.fecha_salida) {
          const salidaFormateada = new Date(ctrlData.fecha_salida).toISOString().split('T')[0]
          setFechaSalida(salidaFormateada)
        }

        const vehiculoRes = await api.get(`vehiculo/${ctrlData.vehiculoid}`)
        setVehiculo(vehiculoRes.data)

        const clienteRes = await api.get(`cliente/${vehiculoRes.data.clienteid}`)
        setCliente(clienteRes.data)
      } catch (err) {
        console.error('Error cargando datos:', err)
      }
    }

    fetchAll()
  }, [id])

  const handleToggle = (key: string) => {
    setControl((prev: any) => ({ ...prev, [key]: !prev[key] }))
  }

  const handleActualizar = async () => {
    try {
      if (!control || !vehiculo || !cliente) return

      const actualizadoControl = {
        ...control,
        vehiculo: undefined,
        fecha_salida: fechaSalida || null,
        dias_garantia: control.dias_garantia === '' ? null : parseInt(control.dias_garantia)
      }

      const actualizadoVehiculo = { ...vehiculo, cliente: undefined }
      const actualizadoCliente = { ...cliente }

      await Promise.all([
        api.put(`cliente/${cliente.id}`, actualizadoCliente),
        api.put(`vehiculo/${vehiculo.id}`, actualizadoVehiculo),
        api.put(`controlvehiculo/${id}`, actualizadoControl)
      ])

      alert('Información actualizada correctamente')
    } catch (err) {
      if(axios.isAxiosError(err)) 
        {
          if(err.status === 401) 
            {
              const mensaje = "No tiene el permiso necesario para hacer esto"
              alert(mensaje)
              return
            }
        }
      if (axios.isAxiosError(err)) {
        const mensaje = err.response?.data || 'Error desconocido'
        alert(typeof mensaje === 'string' ? mensaje : JSON.stringify(mensaje))
      } else {
        alert('Error al procesar la solicitud')
      }
    }
  }

  if (!control || !vehiculo || !cliente) return <p>Cargando...</p>

  return (
    <div className="registro-container">
      <div className="registro-header">
        <div className="header-bar">
          <img src="/logo.png" alt="Logo" className="registro-logo" />
          <div className="registro-titulo">
            <h1>Lubricentro Éxito</h1>
            <h2>Formato de recepción de vehículos</h2>
            <h3>Servicio de mantenimiento</h3>
          </div>
        </div>
        <div className="registro-header-bar">
          <button onClick={() => navigate('/dashboard')}>🏠 Home</button>
        </div>
      </div>

      <div className="registro-datos">
        <div className="registro-fila">
          <div className="campo">
            <label>Técnico Encargado</label>
            <input type="text" value={control.tecnico_encargado} onChange={(e) => setControl({ ...control, tecnico_encargado: e.target.value })} />
          </div>
          <div className="campo">
            <label>Fecha de Ingreso</label>
            <input type="text" value={new Date(control.fecha).toLocaleString()} readOnly />
          </div>
        </div>

        <h3>Datos del Vehículo y Cliente</h3>
        <div className="registro-fila">
          <div className="grupo">
            <label>Nombre</label>
            <input type="text" value={cliente.nombre} onChange={(e) => setCliente({ ...cliente, nombre: e.target.value })} />
            <label>NIT</label>
            <input type="text" value={cliente.nit || ''} onChange={(e) => setCliente({ ...cliente, nit: e.target.value })} />
            <label>CC</label>
            <input type="text" value={cliente.cc || ''} onChange={(e) => setCliente({ ...cliente, cc: parseInt(e.target.value) || 0 })} />
            <label>Celular</label>
            <input type="text" value={cliente.celular || ''} onChange={(e) => setCliente({ ...cliente, celular: e.target.value })} />
          </div>

          <div className="grupo">
            <label>Placa</label>
            <input type="text" value={vehiculo.placa} onChange={(e) => setVehiculo({ ...vehiculo, placa: e.target.value })} />
            <label>Marca</label>
            <input type="text" value={vehiculo.marca} onChange={(e) => setVehiculo({ ...vehiculo, marca: e.target.value })} />
            <label>Modelo</label>
            <input type="text" value={vehiculo.modelo} onChange={(e) => setVehiculo({ ...vehiculo, modelo: e.target.value })} />
            <label>Color</label>
            <input type="text" value={vehiculo.color || ''} onChange={(e) => setVehiculo({ ...vehiculo, color: e.target.value })} />
            <label>Tipo</label>
            <input type="text" value={vehiculo.tipo || ''} onChange={(e) => setVehiculo({ ...vehiculo, tipo: e.target.value })} />
          </div>

          <div className="grupo">
            <label>Combustible</label>
            <select value={vehiculo.diesel_gasolina} onChange={(e) => setVehiculo({ ...vehiculo, diesel_gasolina: e.target.value })}>
              <option value="Gasolina">Gasolina</option>
              <option value="Diesel">Diesel</option>
            </select>
            <label>Kilometraje</label>
            <input type="number" value={control.kilometraje} onChange={(e) => setControl({ ...control, kilometraje: Number(e.target.value) })} />
            <label>Nivel</label>
            <input type="range" min="0" max="100" value={control.nivel} onChange={(e) => setControl({ ...control, nivel: Number(e.target.value) })} />
          </div>
        </div>

        <div className="campo">
          <label>Trabajo a realizar</label>
          <textarea rows={4} value={control.trabajo_realizar || ''} onChange={(e) => setControl({ ...control, trabajo_realizar: e.target.value })} />
        </div>

        <div className="campo">
          <label>Condición Mecánica</label>
          <textarea rows={4} value={control.condicion_mecanica || ''} onChange={(e) => setControl({ ...control, condicion_mecanica: e.target.value })} />
        </div>

        <h3>Accesorios</h3>
        <div className="registro-accesorios">
          {accesoriosKeys.reduce((cols, key, idx) => {
            const col = Math.floor(idx / 9)
            cols[col] = cols[col] || []
            cols[col].push(key)
            return cols
          }, [] as string[][]).map((col, i) => (
            <div className="columna" key={i}>
              {col.map((key) => (
                <label key={key}>
                  <input type="checkbox" checked={Boolean(control[key])} onChange={() => handleToggle(key)} /> {key}
                </label>
              ))}
            </div>
          ))}
        </div>

        <div className="campo">
          <label>Otros Accesorios</label>
          <textarea rows={4} value={control.otrosAccesorios || ''} onChange={(e) => setControl({ ...control, otrosAccesorios: e.target.value })} />
        </div>

        <div className="campo">
          <label>Observaciones</label>
          <textarea rows={4} value={control.observacion || ''} onChange={(e) => setControl({ ...control, observacion: e.target.value })} />
        </div>

        <div className="campo">
          <label>Días de Garantía</label>
          <input type="number" value={control.dias_garantia ?? ''} onChange={(e) => setControl({ ...control, dias_garantia: e.target.value })} />
        </div>

        <div className="registro-footer">
  <input
    type="date"
    value={fechaSalida}
    onChange={(e) => setFechaSalida(e.target.value)}
    style={{ marginRight: '10px' }}
    disabled={!!control.fecha_salida}
  />

  <button
    onClick={handleActualizar}
    style={{ marginRight: '10px' }}
  >
    Actualizar Información
  </button>

  <button
    style={{ backgroundColor: '#0066cc' }}
    disabled={!!control.fecha_salida}
    onClick={async () => {
      try {
        if (!control) return
        const confirmar = window.confirm('¿Estás seguro de que deseas dar salida al vehículo?')
        if (!confirmar) return

        if(control.dias_garantia == 0)
        {
          const confirmar = window.confirm('El vehículo no tiene días de garantía, ¿deseas continuar?')
          if (!confirmar) return
        }
        
        const fechaFinal = fechaSalida || new Date().toISOString().split('T')[0]

        await api.put(`controlvehiculo/${id}`, {
          ...control,
          vehiculo: undefined,
          fecha_salida: fechaFinal,
          dias_garantia:
            control.dias_garantia === '' || isNaN(Number(control.dias_garantia))
              ? null
              : parseInt(control.dias_garantia)
        })

        alert('Vehículo dado de salida')
        navigate('/taller')
      } catch (err) {
        if (axios.isAxiosError(err)) {
          const mensaje = err.response?.data || 'Error desconocido'
          alert(typeof mensaje === 'string' ? mensaje : JSON.stringify(mensaje))
        } else {
          alert('Error al procesar la solicitud')
        }
      }
    }}
  >
    Dar Salida
  </button>

  {control.fecha_salida && (
  <button
    style={{ backgroundColor: '#cc0000', marginLeft: '10px' }}
    onClick={async () => {
      try {
        const confirmar = window.confirm('¿Deseas cancelar la salida del vehículo y volver a ingresarlo al taller?')
        if (!confirmar) return

        await api.put(`controlvehiculo/${id}`, {
          ...control,
          vehiculo: undefined,
          fecha_salida: null,
          dias_garantia:
            control.dias_garantia === '' || isNaN(Number(control.dias_garantia))
              ? null
              : parseInt(control.dias_garantia)
        })

        alert('Salida cancelada, el vehículo ha sido reingresado al taller.')
        setFechaSalida('')
        setControl((prev: any) => ({ ...prev, fecha_salida: null }))
      } catch (err) {
        if (axios.isAxiosError(err)) {
          const mensaje = err.response?.data || 'Error desconocido'
          alert(typeof mensaje === 'string' ? mensaje : JSON.stringify(mensaje))
        } else {
          alert('Error al procesar la solicitud')
        }
      }
    }}
  >
    Cancelar Salida
  </button>
)}

</div>


      </div>
    </div>
  )
}

export default VerVehiculo