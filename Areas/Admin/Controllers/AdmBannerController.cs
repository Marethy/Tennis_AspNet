﻿using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tennis.Data;
using Tennis.Models;

namespace Tennis.Areas.Admin.Controllers;

[Area("Admin")]
public class AdmBannerController : Controller
{
	private readonly IWebHostEnvironment _appEnvironment;
	private readonly TennisWebMVCContext _context;

	public AdmBannerController(TennisWebMVCContext context, IWebHostEnvironment appEnvironment)
	{
		_context = context;
		_appEnvironment = appEnvironment;
	}

	// GET: Admin/AdmBanner
	public async Task<IActionResult> Index()
	{
		if (!User.Identity.IsAuthenticated && User.FindFirstValue(ClaimTypes.Role) != "Admin")
			return RedirectToAction("Login", "AdmAccount");
		return _context.Banners != null
			? View(await _context.Banners.ToListAsync())
			: Problem("Entity set 'FoodWebMVCDbContext.Banners'  is null.");
	}

	// GET: Admin/AdmBanner/Details/5
	public async Task<IActionResult> Details(int? id)
	{
		if (!User.Identity.IsAuthenticated && User.FindFirstValue(ClaimTypes.Role) != "Admin")
			return RedirectToAction("Login", "AdmAccount");
		if (id == null || _context.Banners == null) return NotFound();

		var banner = await _context.Banners
			.FirstOrDefaultAsync(m => m.BannerId == id);
		if (banner == null) return NotFound();

		return View(banner);
	}

	// GET: Admin/AdmBanner/Create
	public IActionResult Create()
	{
		if (!User.Identity.IsAuthenticated && User.FindFirstValue(ClaimTypes.Role) != "Admin")
			return RedirectToAction("Login", "AdmAccount");
		return View();
	}

	// POST: Admin/AdmBanner/Create
	// To protect from overposting attacks, enable the specific properties you want to bind to.
	// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Create(
		[Bind("BannerId,BannerName,ProductDiscount,BannerPrice,BannerDescription,BannerImage,BannerDateCreated")]
		Banner banner)
	{
		if (ModelState.IsValid)
		{
			var files = HttpContext.Request.Form.Files;
			foreach (var Image in files)
				if (Image != null && Image.Length > 0)
				{
					var file = Image;
					var uploads = Path.Combine(_appEnvironment.WebRootPath, "Content\\img\\slides\\");
					if (file.Length > 0)
					{
						var fileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(file.FileName);
						using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
						{
							await file.CopyToAsync(fileStream);
							banner.BannerImage = fileName;
						}
					}
				}

			banner.BannerDateCreated = DateTime.Now;
			_context.Add(banner);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		return View(banner);
	}

	// GET: Admin/AdmBanner/Edit/5
	public async Task<IActionResult> Edit(int? id)
	{
		if (!User.Identity.IsAuthenticated && User.FindFirstValue(ClaimTypes.Role) != "Admin")
			return RedirectToAction("Login", "AdmAccount");
		if (id == null || _context.Banners == null) return NotFound();

		var banner = await _context.Banners.FindAsync(id);
		if (banner == null) return NotFound();
		return View(banner);
	}

	// POST: Admin/AdmBanner/Edit/5
	// To protect from overposting attacks, enable the specific properties you want to bind to.
	// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Edit(int id,
		[Bind("BannerId,BannerName,ProductDiscount,BannerPrice,BannerDescription,BannerImage,BannerDateCreated")]
		Banner banner)
	{
		if (id != banner.BannerId) return NotFound();

		if (ModelState.IsValid)
		{
			try
			{
				var files = HttpContext.Request.Form.Files;
				foreach (var Image in files)
					if (Image != null && Image.Length > 0)
					{
						var file = Image;
						var uploads = Path.Combine(_appEnvironment.WebRootPath, "Content\\img\\slides\\");
						if (file.Length > 0)
						{
							var fileName = Guid.NewGuid().ToString().Replace("-", "") +
							               Path.GetExtension(file.FileName);
							using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
							{
								await file.CopyToAsync(fileStream);
								banner.BannerImage = fileName;
							}
						}
					}

				banner.BannerDateCreated = DateTime.Now;
				_context.Update(banner);
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!BannerExists(banner.BannerId)) return NotFound();

				throw;
			}

			return RedirectToAction(nameof(Index));
		}

		return View(banner);
	}

	// GET: Admin/AdmBanner/Delete/5
	public async Task<IActionResult> Delete(int? id)
	{
		if (!User.Identity.IsAuthenticated && User.FindFirstValue(ClaimTypes.Role) != "Admin")
			return RedirectToAction("Login", "AdmAccount");
		if (id == null || _context.Banners == null) return NotFound();

		var banner = await _context.Banners
			.FirstOrDefaultAsync(m => m.BannerId == id);
		if (banner == null) return NotFound();

		return View(banner);
	}

	// POST: Admin/AdmBanner/Delete/5
	[HttpPost]
	[ActionName("Delete")]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> DeleteConfirmed(int id)
	{
		if (_context.Banners == null) return Problem("Entity set 'FoodWebMVCDbContext.Banners'  is null.");
		var banner = await _context.Banners.FindAsync(id);
		if (banner != null) _context.Banners.Remove(banner);

		await _context.SaveChangesAsync();
		return RedirectToAction(nameof(Index));
	}

	private bool BannerExists(int id)
	{
		return (_context.Banners?.Any(e => e.BannerId == id)).GetValueOrDefault();
	}
}