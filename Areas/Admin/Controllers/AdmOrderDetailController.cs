﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tennis.Data;
using Tennis.Models;

namespace Tennis.Areas.Admin.Controllers;

[Area("Admin")]
public class AdmOrderDetailController : Controller
{
	private readonly TennisWebMVCContext _context;

	public AdmOrderDetailController(TennisWebMVCContext context)
	{
		_context = context;
	}

	// GET: Admin/AdmOrderDetail
	public async Task<IActionResult> Index()
	{
		var FoodWebMVCDbContext = _context.OrderDetails.Include(o => o.Order).Include(o => o.Product);
		return View(await FoodWebMVCDbContext.ToListAsync());
	}

	// GET: Admin/AdmOrderDetail/Details/5
	public async Task<IActionResult> Details(int? id)
	{
		if (id == null || _context.OrderDetails == null) return NotFound();

		var orderDetail = await _context.OrderDetails
			.Include(o => o.Order)
			.Include(o => o.Product)
			.FirstOrDefaultAsync(m => m.OrderId == id);
		if (orderDetail == null) return NotFound();

		return View(orderDetail);
	}

	// GET: Admin/AdmOrderDetail/Create
	public IActionResult Create()
	{
		ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId");
		ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductName");
		return View();
	}

	// POST: Admin/AdmOrderDetail/Create
	// To protect from overposting attacks, enable the specific properties you want to bind to.
	// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Create([Bind("OrderId,ProductId,UnitPrice,Quantity")] OrderDetail orderDetail)
	{
		if (ModelState.IsValid)
		{
			_context.Add(orderDetail);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId", orderDetail.OrderId);
		ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductName", orderDetail.ProductId);
		return View(orderDetail);
	}

	// GET: Admin/AdmOrderDetail/Edit/5
	public async Task<IActionResult> Edit(int? id)
	{
		if (id == null || _context.OrderDetails == null) return NotFound();

		var orderDetail = await _context.OrderDetails.FindAsync(id);
		if (orderDetail == null) return NotFound();
		ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId", orderDetail.OrderId);
		ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductName", orderDetail.ProductId);
		return View(orderDetail);
	}

	// POST: Admin/AdmOrderDetail/Edit/5
	// To protect from overposting attacks, enable the specific properties you want to bind to.
	// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Edit(int id,
		[Bind("OrderId,ProductId,UnitPrice,Quantity")] OrderDetail orderDetail)
	{
		if (id != orderDetail.ProductId) return NotFound();

		if (ModelState.IsValid)
		{
			try
			{
				_context.Update(orderDetail);
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!OrderDetailExists(orderDetail.ProductId)) return NotFound();

				throw;
			}

			return RedirectToAction(nameof(Index));
		}

		ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId", orderDetail.OrderId);
		ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductName", orderDetail.ProductId);
		return View(orderDetail);
	}

	// GET: Admin/AdmOrderDetail/Delete/5
	public async Task<IActionResult> Delete(int? id)
	{
		if (id == null || _context.OrderDetails == null) return NotFound();

		var orderDetail = await _context.OrderDetails
			.Include(o => o.Order)
			.Include(o => o.Product)
			.FirstOrDefaultAsync(m => m.ProductId == id);
		if (orderDetail == null) return NotFound();

		return View(orderDetail);
	}

	// POST: Admin/AdmOrderDetail/Delete/5
	[HttpPost]
	[ActionName("Delete")]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> DeleteConfirmed(int id)
	{
		if (_context.OrderDetails == null) return Problem("Entity set 'FoodWebMVCDbContext.OrderDetails'  is null.");
		var orderDetail = await _context.OrderDetails.FindAsync(id);
		if (orderDetail != null) _context.OrderDetails.Remove(orderDetail);

		await _context.SaveChangesAsync();
		return RedirectToAction(nameof(Index));
	}

	private bool OrderDetailExists(int id)
	{
		return (_context.OrderDetails?.Any(e => e.ProductId == id)).GetValueOrDefault();
	}
}