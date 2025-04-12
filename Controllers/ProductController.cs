
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Tennis.Interfaces;
using Tennis.Models;
using X.PagedList;
using X.PagedList.Extensions;

namespace Tennis.Controllers;

public class ProductController : Controller
{
	private readonly ICategoryRepository _repoCategory;
	private readonly IProductRepository _repoProduct;
	private readonly IProductRatingRepository _repoProductRating;

	public ProductController(IProductRepository repoProduct, ICategoryRepository repoCategory,
		IProductRatingRepository repoProductRating)
	{
		_repoProduct = repoProduct;
		_repoCategory = repoCategory;
		_repoProductRating = repoProductRating;
	}

	public async Task<IActionResult> Index(int page = 1)
	{
		var obj = await _repoProduct.GetListAsync();
		return View(obj.ToPagedList(page, 9));
	}

	// List view
	public async Task<IActionResult> ListView(int page = 1)
	{
		var obj = await _repoProduct.GetListAsync();
		return View(obj.ToPagedList(page, 5));
	}

	// Product Details page
	public async Task<IActionResult> ProductDetail(int id)
	{
		var obj = await _repoProduct.GetByIdAsync(id);
		ViewBag.Review = _repoProductRating.Count(x => x.ProductId == id);
		return View(obj);
	}

	// Write review for PD 
	public async Task<IActionResult> LeaveReview(int id)
	{
		var obj = await _repoProduct.GetByIdAsync(id);
		return View(obj);
	}

	[HttpPost]
	public async Task<IActionResult> LeaveReview(IFormCollection form, int id)
	{
		if (!User.Identity.IsAuthenticated || User.IsInRole("Admin"))
			return RedirectToAction("Login", "User");
		var pd = new ProductRating();
		var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
		pd.CustomerId = userId;
		pd.ProductId = id;
		pd.Stars = Convert.ToInt32(form["rating-input"]);
		pd.RatingContent = form["RatingContent"];
		pd.PRDateCreated = DateTime.Now;
		await _repoProductRating.AddAsync(pd);
		await _repoProductRating.SaveAsync();
		await _repoProduct.UpdateRating(id);
		return RedirectToAction("Confirm", "Product");
	}

	// Confirm feature
	public IActionResult Confirm()
	{
		return View();
	}

	// Search feature
	public async Task<IActionResult> SearchByFilter(string name, int? categoryId, string sort, int page = 1)
	{
		IEnumerable<Product> obj = await _repoProduct.GetListAsync();

		// Gửi lại các tham số vào ViewData để giữ lại trong URL khi chuyển trang
		ViewData["SearchName"] = name;
		ViewData["CategoryId"] = categoryId;
		ViewData["Sort"] = sort;

		// Lọc theo tên sản phẩm nếu có
		if (!string.IsNullOrEmpty(name))
			obj = await _repoProduct.GetListAsync(x => x.ProductName.Contains(name));

		// Lọc theo categoryId nếu có
		if (categoryId.HasValue)
			obj = await _repoProduct.GetListAsync(x => x.CategoryId == categoryId);

		// Sắp xếp theo các lựa chọn
		switch (sort)
		{
			case "date_desc":
				obj = await _repoProduct.GetListAsync(orderBy: x => x.OrderBy(x => x.ProductDateCreated));
				break;
			case "date_asce":
				obj = await _repoProduct.GetListAsync(orderBy: x => x.OrderByDescending(x => x.ProductDateCreated));
				break;
			case "price_desc":
				obj = await _repoProduct.GetListAsync(orderBy: x => x.OrderBy(x => x.ProductPrice));
				break;
			case "price_asce":
				obj = await _repoProduct.GetListAsync(orderBy: x => x.OrderByDescending(x => x.ProductPrice));
				break;
			case "discount":
				obj = await _repoProduct.GetListAsync(orderBy: x => x.OrderByDescending(x => x.ProductDiscount));
				break;
		}

		return View(obj.ToPagedList(page, 9)); // Trả về kết quả đã phân trang
	}

	public async Task<IActionResult> Filter()
	{
		var objCategory = await _repoCategory.GetListAsync();
		ViewData["ListCategory"] = objCategory;
		//@foreach(var item in ViewData["ListCategory"] as List<Category>)
		//{
		//    < li >
		//        < label class="container_check">
		//            @item.CategoryName
		//            <input type="checkbox" value="@item.CategoryId = @ViewData["Test"]" id="item_@item.CategoryId">
		//            <span class="checkmark"></span>
		//        </label>
		//    </li>
		//}
		return View();
	}
}