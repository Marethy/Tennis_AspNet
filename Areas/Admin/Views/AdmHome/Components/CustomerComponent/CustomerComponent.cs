using Microsoft.AspNetCore.Mvc;
using Tennis.Data;

namespace Tennis.Areas.Admin.Views.AdmHome.Components;

public class CustomerComponent : ViewComponent
{
	private readonly TennisWebMVCContext _context;

	public CustomerComponent(TennisWebMVCContext context)
	{
		_context = context;
	}

	public IViewComponentResult Invoke(string customerOrder)
	{
		var objCustomer = 0;
		var objLastCustomer = 0;
		switch (customerOrder)
		{
			case "today":
				var lastDate = DateTime.Now.Date.AddDays(-1);
				objCustomer = _context.Customers.Where(
					x => x.CustomerDateCreated.Date == DateTime.Now.Date
				).Count();
				objLastCustomer = _context.Customers.Where(
					x => x.CustomerDateCreated.Date == lastDate
				).Count();
				ViewBag.CustomerDay = "Hôm nay";
				break;
			case "thisMonth":
				var lastMonthDate = DateTime.Now.Date.AddMonths(-1);
				objCustomer = _context.Customers.Where(
					x => x.CustomerDateCreated.Year == DateTime.Now.Year
					     && x.CustomerDateCreated.Month == DateTime.Now.Month
				).Count();
				objLastCustomer = _context.Customers.Where(
					x => x.CustomerDateCreated.Year == lastMonthDate.Year
					     && x.CustomerDateCreated.Month == lastMonthDate.Month
				).Count();
				ViewBag.CustomerDay = "Tháng này";
				break;
			case "thisYear":
				var lastYearDate = DateTime.Now.Date.AddYears(-1);
				objCustomer = _context.Customers.Where(x => x.CustomerDateCreated.Year == DateTime.Now.Year).Count();
				objLastCustomer = _context.Customers.Where(x => x.CustomerDateCreated.Year == lastYearDate.Year)
					.Count();
				ViewBag.CustomerDay = "Năm này";
				break;
		}

		ViewBag.CustomerPercent = objLastCustomer > 0
			? (objCustomer - objLastCustomer) * 100 / objLastCustomer
			: objCustomer * 100;
		ViewBag.CustomerColor = objCustomer == objLastCustomer ? "text-muted" :
			objCustomer > objLastCustomer ? "text-success" : "text-danger";
		ViewBag.CustomerStatus = objCustomer == objLastCustomer ? "" : objCustomer > objLastCustomer ? "tăng" : "giảm";
		ViewBag.CustomerNumbers = objCustomer;
		return View();
	}
}