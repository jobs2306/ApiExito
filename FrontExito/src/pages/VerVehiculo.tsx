import React, { useEffect, useState } from 'react'
import { useParams, useNavigate } from 'react-router-dom'
import axios from 'axios'
import './RegistroEntrada.css'

const VerVehiculo = () => {
  const { id } = useParams()
  const navigate = useNavigate()
  const [control, setControl] = useState<any>(null)
  const [fechaSalida, setFechaSalida] = useState('')

  const fetchControl = async () => {
    try {
      const res = await axios.get(`https://localhost:7136/api/controlvehiculo/${id}`)
      const data = res.data
      setControl({
        ...data,
        ...Object.fromEntries(Object.entries(data).map(([k, v]) => [k, v === null ? false : v]))
      })
    } catch (err) {
      console.error('Error cargando vehiculo:', err)
    }
  }

  useEffect(() => {
    fetchControl()
  }, [id])

  const handleToggle = (key: string) => {
    setControl((prev: any) => ({ ...prev, [key]: !prev[key] }))
  }

  const handleActualizar = async () => {
    try {
      const actualizado = {
        ...control,
        vehiculo: undefined,
        fecha_salida: control.fecha_salida || null,
        dias_garantia:
          control.dias_garantia === '' || control.dias_garantia === undefined
            ? null
            : parseInt(control.dias_garantia)
      }
  
      await axios.put(`https://localhost:7136/api/controlvehiculo/${id}`, actualizado)
      alert('Información actualizada')
    } catch (err) {
      if (axios.isAxiosError(err)) {
        const mensaje = err.response?.data || 'Error desconocido'
        const texto = typeof mensaje === 'string' ? mensaje : JSON.stringify(mensaje)
        alert(texto)
      } else {
        alert('Error al procesar la solicitud')
      }
    }
  }
  
  
  const handleDarSalida = async () => {
    try {
      // Obtener la fecha actual en formato YYYY-MM-DD (hora de Colombia)
          // Generar la fecha de hoy en formato compatible con DateTime
          const hoy = new Date().toISOString().split('T')[0]


          const salida = {
          ...control,
          vehiculo: undefined,
          fecha_salida: fechaSalida || hoy,
          dias_garantia:
          control.dias_garantia === '' || control.dias_garantia === undefined || isNaN(Number(control.dias_garantia))
          ? null
          : parseInt(control.dias_garantia)
        }
  
      await axios.put(`https://localhost:7136/api/controlvehiculo/${id}`, salida)
      alert('Vehículo dado de salida')
      navigate('/taller')
    } catch (err) {
      if (axios.isAxiosError(err)) {
        const mensaje = err.response?.data || 'Error desconocido'
        const texto = typeof mensaje === 'string' ? mensaje : JSON.stringify(mensaje)
        alert(texto)
      } else {
        alert('Error al procesar la solicitud')
      }
    }
  }
  

  if (!control) return <p>Cargando...</p>

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
            <input
              type="text"
              value={control.tecnico_encargado}
              onChange={(e) => setControl({ ...control, tecnico_encargado: e.target.value })}
            />
          </div>
          <div className="campo">
            <label>Fecha de Ingreso</label>
            <input type="text" value={new Date(control.fecha).toLocaleString()} readOnly />
          </div>
        </div>

        <div className="registro-fila">
          <div className="campo">
            <label>Kilometraje</label>
            <input type="number" value={control.kilometraje} onChange={(e) => setControl({ ...control, kilometraje: Number(e.target.value) })} />
            <label>Nivel</label>
            <input type="range" min="0" max="100" value={control.nivel} onChange={(e) => setControl({ ...control, nivel: Number(e.target.value) })} />
          </div>
        </div>

        <div className="campo">
          <label>Trabajo a realizar</label>
          <textarea rows={5} value={control.trabajo_realizar || ''} onChange={(e) => setControl({ ...control, trabajo_realizar: e.target.value })} />
        </div>

        <div className="campo">
          <label>Condición Mecánica</label>
          <textarea rows={5} value={control.condicion_mecanica || ''} onChange={(e) => setControl({ ...control, condicion_mecanica: e.target.value })} />
        </div>

        <h3>Accesorios y Herramientas</h3>
        <div className="registro-accesorios">
          {[ 
            'EspejoDerecho', 'EspejoIzquierdo', 'EspejoRetro', 'RejillaAire', 'Tapete', 'Plumillas', 'MemoriaUsb', 'TapaGasolian', 'Bateria',
            'Radio', 'VidriosPuertas', 'PanoramicoDel', 'PanoramicoTra', 'LlantaRep', 'PlacaDel', 'PlacaTra', 'MedidorAceite', 'TapasLlanta',
            'LuzDelDer', 'LuzDelIz1', 'LuzTrasDer', 'LuzTrasIz1', 'Rayones', 'Pangones', 'KitCarrera', 'TapaRadiador', 'MarquillaCromada'
          ].reduce((cols, key, idx) => {
            const col = Math.floor(idx / 9)
            cols[col] = cols[col] || []
            cols[col].push(key)
            return cols
          }, [] as string[][]).map((col, i) => (
            <div className="columna" key={i}>
              {col.map((key) => (
                <label key={key}><input type="checkbox" checked={control[key]} onChange={() => handleToggle(key)} /> {key}</label>
              ))}
            </div>
          ))
        }</div>

        <div className="campo">
          <label>Otros Accesorios</label>
          <textarea rows={5} value={control.OtrosAccesorios || ''} onChange={(e) => setControl({ ...control, OtrosAccesorios: e.target.value })} />
        </div>

        <div className="campo">
          <label>Observaciones</label>
          <textarea rows={5} value={control.observacion || ''} onChange={(e) => setControl({ ...control, observacion: e.target.value })} />
        </div>

        <div className="campo">
          <label>Días de Garantía</label>
          <input
          type="number"
          min="0"
          value={control.dias_garantia ?? ''}
          onChange={(e) => {
          const value = e.target.value
          setControl({ ...control, dias_garantia: value === '' ? null : parseInt(value) })
          }}
          />
        </div>

        <div className="registro-footer">
          <button onClick={handleActualizar}>Actualizar</button>
          <input type="date" value={fechaSalida} onChange={(e) => setFechaSalida(e.target.value)} style={{ margin: '10px' }} />
          <button onClick={handleDarSalida} style={{ backgroundColor: '#0066cc' }}>Dar Salida</button>
        </div>
      </div>
    </div>
  )
}

export default VerVehiculo