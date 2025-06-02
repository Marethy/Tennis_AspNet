using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tennis.Data;
using Tennis.Models;

namespace Tennis.Areas.Admin.Views.AdmHome.Components;

public class ActivityComponent : ViewComponent
{
	private readonly TennisWebMVCContext _context;

	public ActivityComponent(TennisWebMVCContext context)
	{
		_context = context;
	}

	public async Task<IViewComponentResult> InvokeAsync(string order)
	{
		var color = new List<string>
	{
		"text-danger",
		"text-primary",
		"text-secondary",
		"text-info",
		"text-muted",
		"text-warning",
		"text-success"
	};
		var random = new Random();

		IQueryable<OrderDetail> query = _context.OrderDetails;

		switch (order)
		{
			case "today":
				query = query.Where(od => od.Order.DayOrder.Date == DateTime.Now.Date);
				ViewBag.ActivityTime = "Hôm nay";
				break;
			case "month":
				query = query.Where(od =>
					od.Order.DayOrder.Month == DateTime.Now.Month &&
					od.Order.DayOrder.Year == DateTime.Now.Year);
				ViewBag.ActivityTime = "Tháng này";
				break;
			case "year":
				query = query.Where(od => od.Order.DayOrder.Year == DateTime.Now.Year);
				ViewBag.ActivityTime = "Năm nay";
				break;
			default:
				ViewBag.ActivityTime = "";
				break;
		}

		// Lấy dữ liệu từ database trước (chỉ các thuộc tính cần thiết)
		var data = await query
			.OrderByDescending(od => od.Order.DayDelivery)
			.ThenByDescending(od => od.Order.DayOrder)
			.Select(od => new
			{
				od.Order.DayOrder,
				UserFullName = od.Order.Customer.CustomerFullName,
				od.Quantity,
				ProductName = od.Product.ProductName
			}).Take(7).ToListAsync();

		// Thực hiện xử lý ngẫu nhiên trong bộ nhớ
		var result = data.Select(item => new ActivityViewModel
		{
			BuyTime = item.DayOrder,
			UserFullName = item.UserFullName,
			Quantity = item.Quantity,
			ProductName = item.ProductName,
			TextColor = color[random.Next(color.Count)]
		}).ToList();

		return View(result);
	}
}