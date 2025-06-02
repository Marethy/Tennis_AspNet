using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tennis.Data;     // namespace chứa TennisWebMVCContext
using Tennis.Models;

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
                                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null) return NotFound();

            return View(order);
        }

        // GET: Admin/AdmOrder/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "Name");
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
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "Name", order.CustomerId);
            return View(order);
        }

        // GET: Admin/AdmOrder/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            if (_context.Orders == null) return NotFound();

            var order = await _context.Orders.FindAsync(id);
            if (order == null) return NotFound();

            // Tạo dropdown cho CustomerId
            ViewData["CustomerId"] = new SelectList(
                await _context.Customers
                    .Select(c => new { c.CustomerId, c.CustomerFullName })
                    .ToListAsync(),
                "CustomerId",
                "Name",
                order.CustomerId);

            return View(order);
        }

        // POST: Admin/AdmOrder/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("OrderId,DayOrder,DayDelivery,PaidState,DeliveryState,TotalMoney,CustomerId")] Order order)
        {
            if (id != order.OrderId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // Cập nhật toàn bộ 7 trường mà bạn đã bind:
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId))
                        return NotFound();
                    else
                        throw;
                }
                catch (DbUpdateException ex)
                {
                    // Nếu vẫn vi phạm FK (dù đã bind CustomerId hợp lệ), bạn có thể log ex.InnerException
                    ModelState.AddModelError("", "Cập nhật thất bại do ràng buộc cơ sở dữ liệu.");
                    // Giữ lại dropdown
                    ViewData["CustomerId"] = new SelectList(
                        await _context.Customers
                            .Select(c => new { c.CustomerId, c.CustomerFullName })
                            .ToListAsync(),
                        "CustomerId",
                        "Name",
                        order.CustomerId);
                    return View(order);
                }
                return RedirectToAction(nameof(Index));
            }

            // Nếu ModelState không hợp lệ (ví dụ nhập thiếu ngày hoặc chưa chọn khách), trả lại dropdown
            ViewData["CustomerId"] = new SelectList(
                await _context.Customers
                    .Select(c => new { c.CustomerId, c.CustomerFullName })
                    .ToListAsync(),
                "CustomerId",
                "Name",
                order.CustomerId);
            return View(order);
        }

        // GET: Admin/AdmOrder/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Orders == null) return NotFound();

            var order = await _context.Orders
                                .Include(o => o.Customer)
                                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null) return NotFound();

            return View(order);
        }

        // POST: Admin/AdmOrder/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Orders == null) return Problem("Entity set 'TennisWebMVCContext.Orders' is null.");
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
