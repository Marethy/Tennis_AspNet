using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tennis.Models;
using Tennis.Data;

namespace Tennis.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdmOrderController : Controller
    {
        private readonly TennisWebMVCContext _context;

        public AdmOrderController(TennisWebMVCContext context)
        {
            _context = context;
        }

        // GET: Admin/AdmOrder
        public async Task<IActionResult> Index()
        {
            var orders = await _context.Orders
                              .Include(o => o.Customer)
                              .ToListAsync();
            return View(orders);
        }

        // GET: Admin/AdmOrder/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var order = await _context.Orders
                              .Include(o => o.Customer)
                              .Include(o => o.OrderDetails)
                              .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null) return NotFound();
            return View(order);
        }

        // GET: Admin/AdmOrder/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(
                _context.Customers.Select(c => new { c.CustomerId, c.CustomerFullName }),
                "CustomerId", "Name");
            return View();
        }

        // POST: Admin/AdmOrder/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("DayOrder,DayDelivery,PaidState,DeliveryState,TotalMoney,CustomerId")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(
                _context.Customers.Select(c => new { c.CustomerId, c.CustomerFullName }).ToList(),
                "CustomerId", "Name", order.CustomerId);
            return View(order);
        }

        // GET: Admin/AdmOrder/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            // Lấy entity gốc, có sẵn CustomerId, OrderDetails
            var orderInDb = await _context.Orders
                                  .Include(o => o.OrderDetails)
                                  .FirstOrDefaultAsync(o => o.OrderId == id);
            if (orderInDb == null) return NotFound();

            // Tạo dropdown để show tên khách (nếu bạn muốn hiển thị)
            ViewData["CustomerId"] = new SelectList(
                _context.Customers.Select(c => new { c.CustomerId, c.CustomerFullName }).ToList(),
                "CustomerId", "Name", orderInDb.CustomerId);

            return View(orderInDb);
        }

        // POST: Admin/AdmOrder/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("OrderId,DayOrder,DayDelivery,PaidState,DeliveryState,TotalMoney")] Order postedOrder)
        {
            if (id != postedOrder.OrderId)
                return NotFound();

         

            // 1) Lấy entity gốc từ DB
            var orderInDb = await _context.Orders
                                  .Include(o => o.OrderDetails)
                                  .FirstOrDefaultAsync(o => o.OrderId == id);
            if (orderInDb == null)
                return NotFound();

            // 2) Gán lại từng trường được edit
            orderInDb.DayOrder = postedOrder.DayOrder;
            orderInDb.DayDelivery = postedOrder.DayDelivery;
            orderInDb.PaidState = postedOrder.PaidState;
            orderInDb.DeliveryState = postedOrder.DeliveryState;
            orderInDb.TotalMoney = postedOrder.TotalMoney;
            // → KHÔNG động gì đến orderInDb.CustomerId hoặc orderInDb.OrderDetails

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Orders.Any(e => e.OrderId == id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/AdmOrder/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var order = await _context.Orders
                              .Include(o => o.Customer)
                              .FirstOrDefaultAsync(o => o.OrderId == id);
            if (order == null) return NotFound();

            return View(order);
        }

        // POST: Admin/AdmOrder/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }
    }
}
