import React, { useState } from 'react'
import './RegistroEntrada.css'
import { useNavigate } from "react-router-dom";
import axios from 'axios'


const RegistroEntrada = () => {

  const navigate = useNavigate();
  
  const [fechaIngreso, setFechaIngreso] = useState(() => {
    const today = new Date()
    const year = today.getFullYear()
    const month = String(today.getMonth() + 1).padStart(2, '0') // enero es 0
    const day = String(today.getDate()).padStart(2, '0')
    return `${year}-${month}-${day}`
  })

  // Cliente
  const [nombre, setNombre] = useState('')
  const [nit, setNit] = useState('')
  const [cc, setCc] = useState('')
  const [celular, setCelular] = useState('')

  // Vehiculo
  const [placa, setPlaca] = useState('')
  const [marca, setMarca] = useState('')
  const [modelo, setModelo] = useState('')
  const [color, setColor] = useState('')
  const [tipo, setTipo] = useState('')
  const [combustible, setCombustible] = useState('Gasolina')
  const [kilometraje, setKilometraje] = useState(0)
  const [nivel, setNivel] = useState(0)

  // ControlVehiculo
  const [tecnico, setTecnico] = useState('')
  const [trabajoRealizar, setTrabajoRealizar] = useState('')
  const [condicionMecanica, setCondicionMecanica] = useState('')
  const [observaciones, setObservaciones] = useState('')
  const [otrosAccesorios, setOtrosAccesorios] = useState('')

  // Accesorios
  const [accesorios, setAccesorios] = useState<{ [key: string]: boolean }>({})

  const toggleAccesorio = (key: string) => {
    setAccesorios((prev) => ({ ...prev, [key]: !prev[key] }))
  }

  const resetFormulario = () => {
    setNombre('')
    setNit('')
    setCc('')
    setCelular('')
    setPlaca('')
    setMarca('')
    setModelo('')
    setColor('')
    setTipo('')
    setCombustible('Gasolina')
    setKilometraje(0)
    setNivel(0)
    setTecnico('')
    setTrabajoRealizar('')
    setCondicionMecanica('')
    setObservaciones('')
    setOtrosAccesorios('')
    setAccesorios({})
    setFechaIngreso(() => {
      const today = new Date()
      const year = today.getFullYear()
      const month = String(today.getMonth() + 1).padStart(2, '0')
      const day = String(today.getDate()).padStart(2, '0')
      return `${year}-${month}-${day}`
    })
  }
  

  const handleGenerarIngreso = async () => {
    if (!tecnico) return alert('Debe indicar el Técnico encargado')
    if (!nit && !cc) return alert('Debe indicar NIT o CC')
    if (!nombre) return alert('Debe indicar el nombre del cliente')
    if (!placa || !marca || !modelo) return alert('Debe indicar Placa, Marca y Modelo')

    const apiUrl = 'https://localhost:7136/api/'
    const metodos = axios.create({ baseURL: apiUrl })

    try {
      // Verificar o crear cliente
      let clienteResponse = null
      try {
        const ruta = cc ? `cliente/cc/${cc}` : `cliente/nit/${nit}`
        clienteResponse = await metodos.get(ruta)
      } catch {
        const nuevoCliente = { nombre, cc: parseInt(cc || '0'), nit, celular }
        const res = await metodos.post('cliente', nuevoCliente)
        clienteResponse = { data: res.data }
      }

      const clienteId = clienteResponse.data.id

      // Verificar o crear vehiculo
      let vehiculoResponse = null
      try {
        vehiculoResponse = await metodos.get(`vehiculo/placa/${placa}`)
      } catch {
        const nuevoVehiculo = {
          placa,
          marca,
          modelo,
          color,
          tipo,
          diesel_gasolina: combustible,
          Clienteid: clienteId,
        }
        const res = await metodos.post('vehiculo', nuevoVehiculo)
        vehiculoResponse = { data: res.data }
      }

      const vehiculoId = vehiculoResponse.data.id

      // Crear ControlVehiculo
      const control = {
        fecha: fechaIngreso,
        tecnico_encargado: tecnico,
        condicion_mecanica: condicionMecanica,
        kilometraje: kilometraje,
        nivel: nivel,
        trabajo_realizar: trabajoRealizar,
        observacion: observaciones,
        Vehiculoid: vehiculoId,
        fecha_salida: null,
        OtrosAccesorios: otrosAccesorios,
        ...accesorios,
      }

      const ingreso = await metodos.post('controlvehiculo', control)

      if (ingreso.status === 200 || ingreso.status === 201) {
        alert('Ingreso registrado satisfactoriamente')
        resetFormulario()
        
      } else {
        alert('Error al generar ingreso')
      }
    } 
    
    catch (err) {
      if (axios.isAxiosError(err)) {
        const mensaje = err.response?.data || 'Error desconocido'
        const texto = typeof mensaje === 'string' ? mensaje : JSON.stringify(mensaje)
        alert(texto)
      } else {
        alert('Error al procesar la solicitud')
      }
    }
  }
  
  
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
              value={tecnico}
              onChange={(e) => setTecnico(e.target.value)}
            />

          </div>
          <div className="campo">
            <label>Fecha de Ingreso</label>
            <input
              type="date"
              value={fechaIngreso}
              onChange={(e) => setFechaIngreso(e.target.value)}
            />
          </div>
        </div>

        <h3>Datos del Vehículo y Cliente</h3>
        <div className="registro-fila">
          <div className="grupo">
            <label>Nombre</label>
            <input type="text" value={nombre} onChange={(e) => setNombre(e.target.value)} />
            <label>NIT</label>
            <input type="text" value={nit} onChange={(e) => setNit(e.target.value)} />
            <label>CC</label>
            <input type="text" value={cc} onChange={(e) => setCc(e.target.value)} />
            <label>Celular</label>
            <input type="text" value={celular} onChange={(e) => setCelular(e.target.value)} />
          </div>
          <div className="grupo">
            <label>Placa</label>
            <input type="text" value={placa} onChange={(e) => setPlaca(e.target.value)} />
            <label>Marca</label>
            <input type="text" value={marca} onChange={(e) => setMarca(e.target.value)} />
            <label>Modelo</label>
            <input type="text" value={modelo} onChange={(e) => setModelo(e.target.value)} />
            <label>Color</label>
            <input type="text" value={color} onChange={(e) => setColor(e.target.value)} />
            <label>Tipo</label>
            <input type="text" value={tipo} onChange={(e) => setTipo(e.target.value)} />
          </div>
          <div className="grupo">
            <label>Combustible</label>
            <select value={combustible} onChange={(e) => setCombustible(e.target.value)}>
              <option>Gasolina</option>
              <option>Diesel</option>
            </select>
            <label>Kilometraje</label>
            <input type="number" value={kilometraje} onChange={(e) => setKilometraje(Number(e.target.value))} />
            <label>Nivel</label>
            <input type="range" min="0" max="100" value={nivel} onChange={(e) => setNivel(Number(e.target.value))} />
          </div>
        </div>
        
        <div className="campo">
          <label>Trabajo a realizar</label>
          <textarea rows={5} value={trabajoRealizar} onChange={(e) => setTrabajoRealizar(e.target.value)} />
        </div>

        <div className="campo">
          <label>Condición Mecánica</label>
          <textarea rows={5} value={condicionMecanica} onChange={(e) => setCondicionMecanica(e.target.value)} />
        </div>

        <h3>Accesorios y Herramientas</h3>
        <div className="registro-accesorios">
        <div className="columna">
          <label><input type="checkbox" checked={accesorios.EspejoDerecho || false} onChange={() => toggleAccesorio('EspejoDerecho')} /> Espejo lateral Derecho</label>
          <label><input type="checkbox" checked={accesorios.EspejoIzquierdo || false} onChange={() => toggleAccesorio('EspejoIzquierdo')} /> Espejo lateral Izquierdo</label>
          <label><input type="checkbox" checked={accesorios.EspejoRetro || false} onChange={() => toggleAccesorio('EspejoRetro')} /> Espejo Retrovisor</label>
          <label><input type="checkbox" checked={accesorios.RejillaAire || false} onChange={() => toggleAccesorio('RejillaAire')} /> Rejillas de Aire</label>
          <label><input type="checkbox" checked={accesorios.Tapete || false} onChange={() => toggleAccesorio('Tapete')} /> Tapete</label>
          <label><input type="checkbox" checked={accesorios.Plumillas || false} onChange={() => toggleAccesorio('Plumillas')} /> Plumillas limpia vidrio</label>
          <label><input type="checkbox" checked={accesorios.MemoriaUsb || false} onChange={() => toggleAccesorio('MemoriaUsb')} /> Memoria USB</label>
          <label><input type="checkbox" checked={accesorios.TapaGasolian || false} onChange={() => toggleAccesorio('TapaGasolian')} /> Tapa Gasolina</label>
          <label><input type="checkbox" checked={accesorios.Bateria || false} onChange={() => toggleAccesorio('Bateria')} /> Batería</label>
        </div>

        <div className="columna">
          <label><input type="checkbox" checked={accesorios.Radio || false} onChange={() => toggleAccesorio('Radio')} /> Radio</label>
          <label><input type="checkbox" checked={accesorios.VidriosPuertas || false} onChange={() => toggleAccesorio('VidriosPuertas')} /> Vidrios Puertas</label>
          <label><input type="checkbox" checked={accesorios.PanoramicoDel || false} onChange={() => toggleAccesorio('PanoramicoDel')} /> Panorámico Delantero</label>
          <label><input type="checkbox" checked={accesorios.PanoramicoTra || false} onChange={() => toggleAccesorio('PanoramicoTra')} /> Panorámico Trasero</label>
          <label><input type="checkbox" checked={accesorios.LlantaRep || false} onChange={() => toggleAccesorio('LlantaRep')} /> Llanta de Repuesto</label>
          <label><input type="checkbox" checked={accesorios.PlacaDel || false} onChange={() => toggleAccesorio('PlacaDel')} /> Placa Delantera</label>
          <label><input type="checkbox" checked={accesorios.PlacaTra || false} onChange={() => toggleAccesorio('PlacaTra')} /> Placa Trasera</label>
          <label><input type="checkbox" checked={accesorios.MedidorAceite || false} onChange={() => toggleAccesorio('MedidorAceite')} /> Medidor Aceite</label>
          <label><input type="checkbox" checked={accesorios.TapasLlanta || false} onChange={() => toggleAccesorio('TapasLlanta')} /> Tapas Llanta</label>
        </div>

        <div className="columna">
          <label><input type="checkbox" checked={accesorios.LuzDelDer || false} onChange={() => toggleAccesorio('LuzDelDer')} /> Luces Delantera Derecha</label>
          <label><input type="checkbox" checked={accesorios.LuzDelIz1 || false} onChange={() => toggleAccesorio('LuzDelIz1')} /> Luces Delantera Izquierda</label>
          <label><input type="checkbox" checked={accesorios.LuzTrasDer || false} onChange={() => toggleAccesorio('LuzTrasDer')} /> Luces Traseras Derecha</label>
          <label><input type="checkbox" checked={accesorios.LuzTrasIz1 || false} onChange={() => toggleAccesorio('LuzTrasIz1')} /> Luces Traseras Izquierda</label>
          <label><input type="checkbox" checked={accesorios.Rayones || false} onChange={() => toggleAccesorio('Rayones')} /> Rayones</label>
          <label><input type="checkbox" checked={accesorios.Pangones || false} onChange={() => toggleAccesorio('Pangones')} /> Pangones</label>
          <label><input type="checkbox" checked={accesorios.KitCarrera || false} onChange={() => toggleAccesorio('KitCarrera')} /> Kit de carretera</label>
          <label><input type="checkbox" checked={accesorios.TapaRadiador || false} onChange={() => toggleAccesorio('TapaRadiador')} /> Tapa Radiador</label>
          <label><input type="checkbox" checked={accesorios.MarquillaCromada || false} onChange={() => toggleAccesorio('MarquillaCromada')} /> Marquillas Cromadas</label>
        </div>

        </div>

        <div className="campo">
          <label>Otros Accesorios</label>
          <textarea rows={5} value={otrosAccesorios} onChange={(e) => setOtrosAccesorios(e.target.value)} />
        </div>
        
        <div className="campo">
          <label>Observaciones</label>
          <textarea rows={5} value={observaciones} onChange={(e) => setObservaciones(e.target.value)} />
        </div>

        <div className="registro-footer">
        <button onClick={handleGenerarIngreso}>Generar Ingreso</button>
        </div>
      </div>
    </div>
  )
}

export default RegistroEntrada