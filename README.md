# Instalación y Configuración del Backend (.NET Web API) para uso Local

Este documento explica paso a paso cómo preparar e instalar el backend del proyecto Éxito App en un entorno local dentro de una empresa. El backend está desarrollado en .NET y se ejecuta como un servicio de Windows para que se inicie automáticamente al prender el equipo y sin mostrar ventanas de consola.

---

## 📅 Requisitos

- Windows 10/11 x64 en el equipo del cliente.
- Acceso como administrador para instalar el servicio.
- No se requiere instalar .NET en el cliente (self-contained).
- Libreria: Microsoft.Extensions.Hosting.WindowsServices

---

## ✅ Parte 1: Publicación del Backend (Solo en PC de desarrollo)

1. Abre una terminal en la carpeta del proyecto backend (.csproj).

2. Ejecuta el siguiente comando:

```bash
 dotnet publish -c Release -r win-x64 --self-contained true -o C:\publicaciones\apiexito
```

Esto genera todos los archivos necesarios para ejecutar el backend de forma independiente en una carpeta lista para copiar.

---

## 📤 Parte 2: Transferir al Computador del Cliente

1. Copia la carpeta `C:\publicaciones` generada a una ubicación fija en el equipo del cliente, por ejemplo:

```
C:\publicaciones
```

2. Dentro de esa carpeta debería estar el ejecutable:

```
C:\publicaciones\apiexito\ApiExito.exe
```

---

## 🚩 Parte 3: Registrar el Backend como Servicio de Windows

1. Abre una terminal (PowerShell o CMD) **como Administrador** en el equipo del cliente.

2. Ejecuta el siguiente comando para registrar el servicio:

```cmd
sc create ApiExito binPath= "C:\publicaciones\apiexito\ApiExito.exe"
```

3. Para iniciar el servicio manualmente (opcional):

```powershell
sc start ApiExito
```

4. Para detener el servicio (opcional):

```powershell
sc stop ApiExito
```

5. El servicio se iniciará automáticamente al prender el computador.

---

## 🌐 Detalles adicionales

- **Puerto por defecto:** La API suele correr en `http://0.0.0.0:5136`. Asegúrese de que no haya firewalls bloqueando.
- La consola **no se mostrará** porque el ejecutable fue generado como `Windows Application` por defecto al usar `--self-contained`.

---

## 🚫 Eliminación del servicio

Si en algún momento deseas eliminar el servicio del sistema:

```powershell
sc delete ApiExito
```

---

## 🌟 Verificación de errores
1. Pulsa Win + R, escribe eventvwr y presiona Enter.

2. En el panel izquierdo, ve a:

Registro de Windows > Aplicación

3. Busca eventos con origen:

 - .NET Runtime
 - Application Error
 - ApiExito (si especificaste el nombre del servicio así)

Allí verás excepciones, fallos de arranque y trazas si el proceso se cae.



# 🖥️ Frontend: React (Interfaz Web)

## ✅ Parte 1: Construir la Aplicación (Solo en PC de desarrollo)

1. Abre una terminal en la carpeta raíz del proyecto frontend (donde está el `package.json`).

2. Ejecuta:

```bash
npm install
npm run build
```

Esto generará una carpeta `/dist` (o `/build` dependiendo de la configuración) que contiene todos los archivos estáticos necesarios.

---

## 📤 Parte 2: Transferir al Computador del Cliente

1. Copia la carpeta `/dist` al computador del cliente, por ejemplo:

```
C:\publicaciones\frontend
```

2. Asegúrate de instalar `Node.js` en el equipo cliente (https://nodejs.org) para usar `serve`.

---

## 🚀 Parte 3: Ejecutar el Frontend con Serve

1. Abre una terminal y ejecuta:

```bash
npm install -g serve
```

2. Luego, en la ruta raíz del proyecto (fuera de `dist`):

```bash
serve -s dist -l 5173
```

Esto ejecutará el frontend en `http://localhost:5173`.

3. Opcionalmente, puedes crear un acceso directo al navegador apuntando a esa dirección.

---

## ⚙️ Configuración al Iniciar el Sistema (Frontend)

1. Crea un archivo `.bat` en la carpeta de publicaciones:

```bat
cd C:\publicaciones\frontend
serve -s dist -l 5173
```

2. Crea un archivo `.vbs` en la carpeta de publicaciones:

```bat
Set WshShell = CreateObject("WScript.Shell")
WshShell.Run chr(34) & "C:\publicaciones\initfrontedexito.bat" & chr(34), 0
Set WshShell = Nothing
```

2. Coloca el archivo `.vbs` en la ruta de inicio de windows:

```
C:\Users\[usuario]\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\Startup
```

Esto hará que la interfaz se inicie automáticamente cuando se prenda el computador.

---

Con estas instrucciones completas, tanto el backend como el frontend quedarán funcionando automáticamente al iniciar el sistema, sin necesidad de intervención del usuario.