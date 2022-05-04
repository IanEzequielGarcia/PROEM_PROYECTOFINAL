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

namespace PROEM_PROYECTOFINALMVC.Controllers
{
    public class FacturasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FacturasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Factura
        public async Task<IActionResult> Index()
        {
            return View(await _context.Factura.ToListAsync());
        }

        // GET: Factura/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var facturas = await _context.Factura
                .FirstOrDefaultAsync(m => m.Numero == id);
            if (facturas == null)
            {
                return NotFound();
            }

            return View(facturas);
        }
        public IActionResult Create(int? id)
        {
            ItemFacturaViewModel model = new();
            IEnumerable<Cliente> clientes = _context.Cliente;
            if (id != null)
            {
                model.Factura = _context.Factura.Find(id);
                model.Items = _context.Item.Where(i => i.Facturas == model.Factura).ToList();
                ViewBag.editar = true;
                ViewBag.ClienteId = model.Factura.IdCliente;
                clientes=clientes.Where(i => i.Codigo == model.Factura.IdCliente);
                ViewData["Clientes"] = clientes;
            }
            else
            {
                model.Items = new List<Items>();
                ViewData["Clientes"] = clientes;
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Agregar(ItemFacturaViewModel valores)
        {
            if (valores.Items == null)
                valores.Items = new List<Items>();
            valores.Items.Add(valores.Item);
            valores.Item = new Items();
            IEnumerable<Cliente> clientes = _context.Cliente;
            ViewData["Clientes"] = clientes;
            return View("create", valores);
        }

        [HttpPost]
        public IActionResult Create(ItemFacturaViewModel valores,int ClienteId)
        {
            if (valores.Factura.Numero == 0)
            {
                valores.Factura.Cliente = _context.Cliente.Find(ClienteId);
                valores.Factura.IdCliente = valores.Factura.Cliente.Codigo;
                _context.Factura.Add(valores.Factura);
            }
            else
            {
                _context.Factura.Attach(valores.Factura);
                valores.Factura.Cliente = _context.Cliente.Find(ClienteId);
                valores.Factura.IdCliente = valores.Factura.Cliente.Codigo;
                _context.Entry(valores.Factura).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            }
            //valores.Factura.Cliente = _context.Cliente.Find(ClienteId);
            //_context.Add(valores.Factura);
            //_context.SaveChanges();
            Cliente cliente = valores.Factura.Cliente;
            //Cliente cliente = _context.Cliente.Find(valores.Factura.IdCliente);
            foreach (var item in valores.Items)
            {
                item.Facturas = valores.Factura;
                if (item.Codigo == 0)
                {
                    cliente.SaldoCuentaCorriente = cliente.SaldoCuentaCorriente - item.Precio;
                    item.IdFactura = item.Facturas.Numero;
                    _context.Item.Add(item);
                }
                else
                {
                    float aux = 0;
                    Items item2 =_context.Item.Find(item.Codigo);
                    if(item2.Precio>item.Precio)
                    {
                        aux = item2.Precio - item.Precio;
                        valores.Factura.Cliente.SaldoCuentaCorriente += aux;
                    }
                    else if(item2.Precio<item.Precio)
                    {
                        aux = item.Precio - item2.Precio;
                        valores.Factura.Cliente.SaldoCuentaCorriente -= aux;
                    }
                    var entry = _context.Item.First(e => e.Codigo == item.Codigo);
                    _context.Entry(entry).CurrentValues.SetValues(item);
                    //_context.SaveChanges();

                    //_context.Item.Attach(item);
                    //_context.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                }
            }
            _context.SaveChanges();
            return Redirect("index");
        }
        // GET: Factura/Create
        /*public IActionResult Create()
        {
            IEnumerable<Cliente> clientes = _context.Cliente;
            ViewData["Clientes"]=clientes;
            return View();
        }

        // POST: Factura/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Numero,Fecha")] Facturas facturas,int ClienteId,int cantidad,string descripcion,float precio)
        {
            
            try
            {
                facturas.Cliente = await _context.Cliente.FindAsync(ClienteId);
                _context.Add(facturas);
                await _context.SaveChangesAsync();
                facturas = _context.Factura.FirstOrDefault(acc => acc.Cliente == facturas.Cliente && acc.Fecha == facturas.Fecha);
                
                Items item = new Items(descripcion, cantidad, precio, facturas.Numero);
                _context.Add(item);
                Cliente cliente = facturas.Cliente;
                cliente.SaldoCuentaCorriente = cliente.SaldoCuentaCorriente - item.Precio;
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return View(facturas);
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Factura/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var facturas = await _context.Factura.FindAsync(id);
            if (facturas == null)
            {
                return NotFound();
            }
            return View(facturas);
        }
        */
        // POST: Factura/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Numero,Fecha")] Facturas facturas)
        {
            if (id != facturas.Numero)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(facturas);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FacturasExists(facturas.Numero))
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
            return View(facturas);
        }

        // GET: Factura/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var facturas = await _context.Factura
                .FirstOrDefaultAsync(m => m.Numero == id);
            if (facturas == null)
            {
                return NotFound();
            }

            return View(facturas);
        }

        // POST: Factura/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var facturas = await _context.Factura.FindAsync(id);
            _context.Factura.Remove(facturas);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FacturasExists(int id)
        {
            return _context.Factura.Any(e => e.Numero == id);
        }
    }
}
