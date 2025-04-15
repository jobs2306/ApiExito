using ApiExito.Model;
using ApiExito.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.IO.Image;
using iText.Kernel.Font;
using iText.IO.Font.Constants;
using iText.Layout.Borders;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.IO.Font;
using static iText.Kernel.Font.PdfFontFactory;
using Microsoft.AspNetCore.Authorization;

namespace ApiExito.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportesController : ControllerBase
    {
        private readonly IControlVehiculoService _controlService;
        private readonly IVehiculoService _vehiculoService;
        private readonly IClienteService _clienteService;
        private readonly IWebHostEnvironment _env;

        public ReportesController(IControlVehiculoService controlService, IVehiculoService vehiculoService, IClienteService clienteService, IWebHostEnvironment env)
        {
            _controlService = controlService;
            _vehiculoService = vehiculoService;
            _clienteService = clienteService;
            _env = env;
        }

        [HttpGet("generar/{id}")]
        [Authorize]
        public async Task<IActionResult> GenerarInforme(int id)
        {
            var control = await _controlService.GetByIdAsync(id);
            if (control == null) return NotFound("Control no encontrado");

            var vehiculo = await _vehiculoService.GetByIdAsync(control.Vehiculoid);
            if (vehiculo == null) return NotFound("Vehículo no encontrado");

            var cliente = await _clienteService.GetByIdAsync(vehiculo.Clienteid);
            if (cliente == null) return NotFound("Cliente no encontrado");

            using var stream = new MemoryStream();
            var writer = new PdfWriter(stream);
            var pdf = new PdfDocument(writer);
            var doc = new Document(pdf);

            var boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
            var italicFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_OBLIQUE);

            doc.SetMargins(30, 30, 30, 30);

            var bannerPath = System.IO.Path.Combine(_env.WebRootPath ?? "wwwroot", "banner", "banner01.png");
            if (System.IO.File.Exists(bannerPath))
            {
                var bannerData = ImageDataFactory.Create(bannerPath);
                var banner = new iText.Layout.Element.Image(bannerData)
                    .ScaleToFit(PageSize.A4.GetWidth(), 100) // ajusta al ancho de la página menos márgenes
                    .SetHorizontalAlignment(HorizontalAlignment.CENTER);

                doc.Add(banner);
                doc.Add(new Paragraph(" "));
            }

            // Helper para celdas del lado izquierdo con formato
            Cell Etiqueta(string texto)
            {
                return new Cell()
                    .Add(new Paragraph(texto).SetFont(boldFont))
                    .SetTextAlignment(TextAlignment.LEFT);
            }

            // Helper para celdas del lado derecho normales
            Cell Valor(string texto)
            {
                return new Cell()
                    .Add(new Paragraph(texto))
                    .SetTextAlignment(TextAlignment.LEFT);
            }

            // Tabla de datos cliente/vehículo
            var datosTable = new Table(UnitValue.CreatePercentArray(new float[] { 1, 2 })).UseAllAvailableWidth();
            datosTable.AddCell(Etiqueta("Cliente:")).AddCell(Valor(cliente.nombre));
            if(cliente.cc != 0)
                datosTable.AddCell(Etiqueta("Cédula:")).AddCell(Valor(cliente.cc.ToString() ?? "-"));
            else
                datosTable.AddCell(Etiqueta("NIT:")).AddCell(Valor(cliente.nit ?? "-"));

            datosTable.AddCell(Etiqueta("Teléfono:")).AddCell(Valor(cliente.celular ?? "-"));
            datosTable.AddCell(Etiqueta("Placa:")).AddCell(Valor(vehiculo.placa));
            datosTable.AddCell(Etiqueta("Marca:")).AddCell(Valor(vehiculo.marca));
            datosTable.AddCell(Etiqueta("Modelo:")).AddCell(Valor(vehiculo.modelo));
            datosTable.AddCell(Etiqueta("Combustible:")).AddCell(Valor(vehiculo.diesel_gasolina));
            datosTable.AddCell(Etiqueta("Kilometraje:")).AddCell(Valor(control.kilometraje.ToString()));
            datosTable.AddCell(Etiqueta("Nivel de Combustible:")).AddCell(Valor(control.nivel + "%"));
            datosTable.AddCell(Etiqueta("Fecha ingreso:")).AddCell(Valor(control.fecha.ToString("dd/MM/yyyy HH:mm")));
            datosTable.AddCell(Etiqueta("Fecha salida:")).AddCell(Valor(control.fecha_salida?.ToString("dd/MM/yyyy HH:mm") ?? "No registrada"));
            datosTable.AddCell(Etiqueta("Técnico:")).AddCell(Valor(control.tecnico_encargado));

            doc.Add(datosTable);
            doc.Add(new Paragraph(" "));

            // Accesorios
            var accesorios = new Dictionary<string, bool?>
            {
                {"Espejo Derecho", control.EspejoDerecho },
                {"Espejo Izquierdo", control.EspejoIzquierdo },
                {"Espejo Retrovisor", control.EspejoRetro },
                {"Rejilla Aire", control.RejillaAire },
                {"Tapete", control.Tapete },
                {"Plumillas", control.Plumillas },
                {"Memoria USB", control.MemoriaUsb },
                {"Tapa Gasolina", control.TapaGasolian },
                {"Batería", control.Bateria },
                {"Radio", control.Radio },
                {"Vidrios Puertas", control.VidriosPuertas },
                {"Panorámico Delantero", control.PanoramicoDel },
                {"Panorámico Trasero", control.PanoramicoTra },
                {"Llanta Repuesto", control.LlantaRep },
                {"Placa Delantera", control.PlacaDel },
                {"Placa Trasera", control.PlacaTra },
                {"Medidor de Aceite", control.MedidorAceite },
                {"Tapas Llanta", control.TapasLlanta },
                {"Luz Delantera Der", control.LuzDelDer },
                {"Luz Delantera Izq", control.LuzDelIz1 },
                {"Luz Trasera Der", control.LuzTrasDer },
                {"Luz Trasera Izq", control.LuzTrasIz1 },
                {"Rayones", control.Rayones },
                {"Pangones", control.Pangones },
                {"Kit Carrera", control.KitCarrera },
                {"Tapa Radiador", control.TapaRadiador },
                {"Marquilla Cromada", control.MarquillaCromada },
            };

            doc.Add(new Paragraph("Accesorios").SetFont(boldFont).SetFontSize(16));
            var accesoriosTable = new Table(UnitValue.CreatePercentArray(new float[] { 3, 1 })).UseAllAvailableWidth();
            foreach (var acc in accesorios)
            {
                accesoriosTable.AddCell(new Cell()
                    .Add(new Paragraph(acc.Key).SetFont(boldFont))
                    .SetTextAlignment(TextAlignment.LEFT));

                accesoriosTable.AddCell(new Cell()
                    .Add(new Paragraph(acc.Value == true ? "Sí" : "No"))
                    .SetTextAlignment(TextAlignment.LEFT));
            }

            doc.Add(accesoriosTable);

            doc.Add(new Paragraph("\n")); // espacio
            // Función local para sección con borde
            void AddTextoSeccion(string titulo, string contenido)
            {
                var cell = new Cell()
                    .Add(new Paragraph(titulo).SetFont(boldFont).SetFontSize(11))
                    .Add(new Paragraph(contenido ?? "").SetFontSize(10))
                    .SetBorder(Border.NO_BORDER);

                var table = new Table(1)
                    .UseAllAvailableWidth()
                    .AddCell(cell)
                    .SetBorder(new iText.Layout.Borders.SolidBorder(1));

                doc.Add(table);
                doc.Add(new Paragraph(" ")); // Espacio entre secciones
            }

            // Secciones
            AddTextoSeccion("Trabajo a realizar", control.trabajo_realizar ?? "");
            AddTextoSeccion("Condición mecánica", control.condicion_mecanica ?? "");
            AddTextoSeccion("Observaciones", control.observacion ?? "");
            AddTextoSeccion("Otros accesorios", control.OtrosAccesorios ?? "");

            ///Firmas
            doc.Add(new Paragraph("\n")); // espacio

            var table = new Table(3).UseAllAvailableWidth();
            table.SetTextAlignment(TextAlignment.CENTER);

            // Función local para cargar cada firma
            void AddFirma(string label, string fileName)
            {
                var path = System.IO.Path.Combine(_env.WebRootPath ?? "wwwroot", "firmas", fileName);
                if (System.IO.File.Exists(path))
                {
                    var imageData = ImageDataFactory.Create(path);
                    var image = new iText.Layout.Element.Image(imageData)
                        .ScaleToFit(120, 60)
                        .SetHorizontalAlignment(HorizontalAlignment.CENTER);

                    var cell = new Cell()
                        .Add(new Paragraph(" ")) // espacio arriba
                        .Add(image)
                        .Add(new Paragraph(label).SetTextAlignment(TextAlignment.CENTER).SetFont(boldFont).SetFontSize(12))
                        .SetBorder(Border.NO_BORDER)
                        .SetTextAlignment(TextAlignment.CENTER);

                    table.AddCell(cell);
                }
                else
                {
                    var emptyCell = new Cell()
                        .Add(new Paragraph(" "))
                        .Add(new Paragraph(" "))
                        .Add(new Paragraph(label).SetTextAlignment(TextAlignment.CENTER).SetFont(boldFont))
                        .SetBorder(Border.NO_BORDER)
                        .SetTextAlignment(TextAlignment.CENTER);

                    table.AddCell(emptyCell);
                }
            }

            // Agregar las 3 celdas
            AddFirma("Recepcionista", "firma.png");
            AddFirma("Mecánico", "blanco.png");
            AddFirma("Cliente", "blanco.png");

            // Agregar tabla al documento
            doc.Add(table);

            // Crear celda con contenido
            var cell = new Cell()
            .SetBorder(Border.NO_BORDER)
            .SetPadding(0) // elimina relleno interno
            .SetMarginBottom(0) // elimina espacio inferior
            .Add(new Paragraph("Términos y condiciones")
            .SetFont(boldFont)
            .SetFontSize(18)
            .SetTextAlignment(TextAlignment.CENTER)
            .SetMarginBottom(0) // sin espacio debajo del título
            .SetPadding(0)) // sin relleno
            .Add(new Paragraph("Estimado Cliente terminado el trabajo de su vehículo deberá ser retirado del taller ya que pasado los dos (2) días será trasladado a un parqueadero donde usted se hace responsable.")
            .SetFontSize(12)
            .SetTextAlignment(TextAlignment.CENTER)
            .SetMarginBottom(0)
            .SetPadding(0));


            // Crear tabla con una celda y borde exterior
            var tabla = new Table(1)
                .UseAllAvailableWidth()
                .AddCell(cell)
                .SetBorder(new iText.Layout.Borders.SolidBorder(1));

            doc.Add(tabla);

            doc.Add(new Paragraph("\n")); // espacio

            var logoPath = System.IO.Path.Combine(_env.WebRootPath ?? "wwwroot", "banner", "logo.png");
            var logoTable = new Table(1).UseAllAvailableWidth();

            if (System.IO.File.Exists(logoPath))
            {
                var logoData = ImageDataFactory.Create(logoPath);
                var logo = new iText.Layout.Element.Image(logoData)
                    .ScaleToFit(120, 120)
                    .SetHorizontalAlignment(HorizontalAlignment.CENTER);

                logoTable.AddCell(new Cell()
                    .Add(logo)
                    .SetBorder(Border.NO_BORDER)
                    .SetTextAlignment(TextAlignment.CENTER));
            }
            else
            {
                logoTable.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER)
                    .SetTextAlignment(TextAlignment.CENTER));
            }
            doc.Add(logoTable);

            doc.Add(new Paragraph("\n")); // espacio

            var agradecimiento = new Paragraph("GRACIAS POR PREFERIRNOS")
            .SetFont(boldFont)
            .SetFontSize(25)
            .SetTextAlignment(TextAlignment.CENTER);

            doc.Add(agradecimiento);

            doc.Close();
            return File(stream.ToArray(), "application/pdf", $"informe_{vehiculo.placa}_{DateTime.Now:yyyyMMddHHmmss}.pdf");
        }
    }
}
