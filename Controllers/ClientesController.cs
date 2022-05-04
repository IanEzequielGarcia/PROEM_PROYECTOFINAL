#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EJERCICIO_FINAL.Data;
using PROEM_PROYECTOFINALMVC.Models;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Kernel.Pdf.Canvas.Draw;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace PROEM_PROYECTOFINALMVC.Controllers
{
    public class ClientesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public void CalcularSaldoCuentaCorriente(int usuarioId)
        {
            float saldo = 0;
            Cliente usuario = _context.Cliente.Find(usuarioId);
            var facturas =  _context.Factura.ToList();
            var pagos = _context.Pago.ToList();
            var items = _context.Item.ToList();
            foreach(var factura in facturas)
            {
                foreach(var pago in pagos)
                {
                    if(pago.Cliente==usuario)
                    {
                        saldo += pago.Importe;
                    }
                }
                if(factura.Cliente==usuario)
                {
                    foreach(var item in items)
                    {
                        if(item.IdFactura==factura.Numero)
                        {
                            saldo -= item.Precio;
                        }
                    }
                }
            }
            usuario.SaldoCuentaCorriente = saldo;
            _context.Update(usuario);
            _context.SaveChanges();
        }

        public ClientesController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> ExportPdfTotalVentas(int id)
        {
            Cliente cliente = await _context.Cliente.FindAsync(id);
            var facturas = await _context.Factura.ToListAsync();
            var pagos = await _context.Pago.ToListAsync();
            var item = await _context.Item.ToListAsync();

            var st = new MemoryStream();
            PdfWriter writer = new PdfWriter(st);
            PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf, iText.Kernel.Geom.PageSize.A4);
            var ls = new LineSeparator(new SolidLine());

            float saldo = 0;

            Table tabla= new Table(4, true);

            Cell cell = new Cell()
            .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
            .Add(new Paragraph("Fecha"));
            tabla.AddCell(cell);

            cell = new Cell()
            .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
            .Add(new Paragraph("Tipo"));
            tabla.AddCell(cell);

            cell = new Cell()
            .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
            .Add(new Paragraph("Importe"));
            tabla.AddCell(cell);

            cell = new Cell()
            .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
            .Add(new Paragraph("Saldo"));
            tabla.AddCell(cell);

            facturas.OrderBy(f => f.Fecha).ToList();
            pagos.OrderBy(p => p.Fecha).ToList();
            foreach(Facturas factura in facturas)
            {
                float totalFactura=0;
                foreach (Items items in item)
                {
                    if (factura.Cliente==cliente && items.Facturas == factura)
                    {
                        totalFactura += items.Precio;
                    }
                }
                foreach (Pagos pago in pagos)
                {
                    if(pago.Cliente == cliente)
                        if (pago.Fecha >= factura.Fecha)
                        {
                            string fecha = pago.Fecha.ToString();
                            cell = new Cell()
                            .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                            .Add(new Paragraph($"{fecha}"));
                            tabla.AddCell(cell);

                            cell = new Cell()
                            .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                            .Add(new Paragraph("Pago"));
                            tabla.AddCell(cell);

                            cell = new Cell()
                            .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                            .Add(new Paragraph($"{pago.Importe}"));
                            tabla.AddCell(cell);

                            saldo += pago.Importe;

                            cell = new Cell()
                            .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                            .Add(new Paragraph($"{saldo}"));
                            tabla.AddCell(cell);
                            ///////////////////////////////////////////////////////////////
                            fecha = factura.Fecha.ToString();
                            cell = new Cell()
                            .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                            .Add(new Paragraph($"{fecha}"));
                            tabla.AddCell(cell);

                            cell = new Cell()
                            .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                            .Add(new Paragraph("Factura"));
                            tabla.AddCell(cell);

                            cell = new Cell()
                            .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                            .Add(new Paragraph($"{totalFactura}"));
                            tabla.AddCell(cell);

                            saldo -= totalFactura;

                            cell = new Cell()
                            .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                            .Add(new Paragraph($"{saldo}"));
                            tabla.AddCell(cell);
                        }
                        else if(factura.Cliente==cliente)
                        {
                            string fecha = factura.Fecha.ToString();
                            cell = new Cell()
                            .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                            .Add(new Paragraph($"{fecha}"));
                            tabla.AddCell(cell);

                            cell = new Cell()
                            .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                            .Add(new Paragraph("Factura"));
                            tabla.AddCell(cell);

                            cell = new Cell()
                            .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                            .Add(new Paragraph($"{totalFactura}"));
                            tabla.AddCell(cell);

                            saldo -= totalFactura;

                            cell = new Cell()
                            .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                            .Add(new Paragraph($"{saldo}"));
                            tabla.AddCell(cell);
                            ////////////////////////////////////////////////////////////////
                            fecha = pago.Fecha.ToString();
                            cell = new Cell()
                            .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                            .Add(new Paragraph($"{fecha}"));
                            tabla.AddCell(cell);

                            cell = new Cell()
                            .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                            .Add(new Paragraph("Pago"));
                            tabla.AddCell(cell);

                            cell = new Cell()
                            .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                            .Add(new Paragraph($"{pago.Importe}"));
                            tabla.AddCell(cell);

                            saldo += pago.Importe;

                            cell = new Cell()
                            .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                            .Add(new Paragraph($"{saldo}"));
                            tabla.AddCell(cell);
                        }
                }
            }


            document.Add(tabla);
            document.Add(ls);

            document.Close();

            var st2 = new MemoryStream();
            var array = st.ToArray();
            st2.Write(array, 0, array.Length);
            st2.Position = 0;

            return File(st2, "application/pdf", $"{cliente.Nombre}{cliente.Codigo}.pdf");

        }
        public async Task<IActionResult> ExportPdf()
        {
            var items = _context.Item.ToList();
            var clientes = _context.Cliente.ToList();
            var facturas = _context.Factura.ToList();
            var pagos = _context.Pago.ToList();

            var entity = _context.Item
           .Include(f => f.Facturas)
           .GroupBy(f => f.Facturas.Cliente.Nombre)
           .Select(f => new { Nombre = f.Key, Total = f.Sum(i => i.Precio) })
           .OrderByDescending(f => f.Total)
           .ToList();
            var entity2 = _context.Pago
            .Include(f => f.Cliente)
            .GroupBy(f => f.Cliente.Nombre)
            .Select(f => new { Nombre = f.Key, Total = f.Sum(i => i.Importe) })
            .OrderByDescending(f => f.Total)
            .ToList();
            
            //var entity = await _context.Articulo.ToListAsync();
            IWorkbook workBook = new XSSFWorkbook();
            ISheet sheet = workBook.CreateSheet("Total Ventas");
            var titulos = workBook.CreateCellStyle();
            var fuente = workBook.CreateFont();
            fuente.IsBold = true;
            fuente.FontHeight = 30;
            IRow row = sheet.CreateRow(0);
            var celda = row.CreateCell(0);
            celda.SetCellValue("Cliente");
            celda.CellStyle = titulos;
            //
            //celda = row.CreateCell(1);
            //celda.SetCellValue("Tipo");
            //celda.CellStyle = titulos;
            celda = row.CreateCell(1);
            celda.SetCellValue("Total");
            celda.CellStyle = titulos;

            sheet.AutoSizeColumn(0);
            sheet.AutoSizeColumn(1);
            //sheet.AutoSizeColumn(2);

            int rowNum = 1;
            foreach (var entities in entity)
            {
                row = sheet.CreateRow(rowNum);
                row.CreateCell(0).SetCellValue(entities.Nombre);
                row.CreateCell(1).SetCellValue(entities.Total);
                rowNum++;
            }
            /*int rowNum = 1;
            foreach(Cliente cliente in clientes)
            {
                row = sheet.CreateRow(rowNum);
                row.CreateCell(0).SetCellValue(cliente.Nombre);
                foreach (Facturas factura in facturas)
                {
                    if(rowNum>1)
                    {
                        row = sheet.CreateRow(rowNum);
                        row.CreateCell(0).SetCellValue(cliente.Nombre);
                    }
                    float totalFactura = 0;
                    row.CreateCell(1).SetCellValue("factura");
                    foreach (Items item in items)
                    {
                        if(item.IdFactura==factura.Numero)
                            totalFactura += item.Precio;
                    }
                    row.CreateCell(2).SetCellValue(totalFactura);
                    rowNum++;
                }
                foreach (Pagos pago in pagos)
                {
                    if (rowNum > 1)
                    {
                        row = sheet.CreateRow(rowNum);
                        row.CreateCell(0).SetCellValue(cliente.Nombre);
                    }
                    row.CreateCell(1).SetCellValue("pago");
                    row.CreateCell(2).SetCellValue(pago.Importe);
                    rowNum++;
                }
                
            }*/

            var st = new MemoryStream();
            workBook.Write(st);
            await st.FlushAsync();
            var st2 = new MemoryStream();
            var array = st.ToArray();
            st2.Write(array, 0, array.Length);
            st2.Position = 0;
            return File(st2, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Reporte.xlsx");
        }

        // GET: Clientes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Cliente.ToListAsync());
        }

        // GET: Clientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Cliente
                .FirstOrDefaultAsync(m => m.Codigo == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // GET: Clientes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clientes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Codigo,Nombre,Direccion,Saldo")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                cliente.SaldoCuentaCorriente = 0;
                _context.Add(cliente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }

        // GET: Clientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Cliente.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        // POST: Clientes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Codigo,Nombre,Direccion,Saldo")] Cliente cliente)
        {
            if (id != cliente.Codigo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cliente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExists(cliente.Codigo))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }

        // GET: Clientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Cliente
                .FirstOrDefaultAsync(m => m.Codigo == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cliente = await _context.Cliente.FindAsync(id);
            _context.Cliente.Remove(cliente);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClienteExists(int id)
        {
            return _context.Cliente.Any(e => e.Codigo == id);
        }
    }
}
