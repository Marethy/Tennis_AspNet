﻿using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Security.Claims;
using System.Text.Json;
using Tennis.Data;

namespace Tennis.Areas.Admin.Controllers;

[Area("Admin")]
public class AdmHomeController : Controller
{
    private readonly TennisWebMVCContext _context;
    private CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");

    public AdmHomeController(TennisWebMVCContext context)
    {
        _context = context;
    }

    public IActionResult GetSale(string saleOrder)
    {
        return ViewComponent("SaleComponent", saleOrder);
    }

    public IActionResult GetRevenue(string revenueOrder)
    {
        return ViewComponent("RevenueComponent", revenueOrder);
    }

    public IActionResult GetCustomer(string customerOrder)
    {
        return ViewComponent("CustomerComponent", customerOrder);
    }

    public IActionResult GetRecentActivity(string order)
    {
        return ViewComponent("ActivityComponent", order);
    }

    public IActionResult Index()
    {
        if (!User.Identity.IsAuthenticated && User.FindFirstValue(ClaimTypes.Role) != "Admin")
            return RedirectToAction("Login", "AdmAccount");

        var saleChart1 = _context.Orders.Where(
            x => x.DayOrder.Month >= 1 && x.DayOrder.Month <= 4
                                       && x.DayOrder.Year == DateTime.Now.Year
        ).Count();
        var saleChart2 = _context.Orders.Where(
            x => x.DayOrder.Month >= 4 && x.DayOrder.Month <= 8
                                       && x.DayOrder.Year == DateTime.Now.Year
        ).Count();
        var saleChart3 = _context.Orders.Where(
            x => x.DayOrder.Month >= 8 && x.DayOrder.Month <= 12
                                       && x.DayOrder.Year == DateTime.Now.Year
        ).Count();
        ViewBag.saleChart = JsonSerializer.Serialize(new List<int>
        {
            saleChart1,
            saleChart2,
            saleChart3,
            saleChart1,
            saleChart2,
            saleChart3
        });
        var revenueChart1 = _context.Orders.Where(
            x => x.DayOrder.Month >= 1 && x.DayOrder.Month <= 4
                                       && x.DayOrder.Year == DateTime.Now.Year
        ).Sum(x => x.TotalMoney) / 100000;
        var revenueChart2 = _context.Orders.Where(
            x => x.DayOrder.Month >= 4 && x.DayOrder.Month <= 8
                                       && x.DayOrder.Year == DateTime.Now.Year
        ).Sum(x => x.TotalMoney) / 100000;
        var revenueChart3 = _context.Orders.Where(
            x => x.DayOrder.Month >= 8 && x.DayOrder.Month <= 12
                                       && x.DayOrder.Year == DateTime.Now.Year
        ).Sum(x => x.TotalMoney) / 100000;

        ViewBag.revenueChart = JsonSerializer.Serialize(new List<decimal>
        {
            revenueChart2,
            revenueChart2,
            revenueChart1,
            revenueChart2,
            revenueChart3,
            revenueChart1
        });

        var customerChart1 = _context.Customers.Where(
            x => x.CustomerDateCreated.Month >= 1 && x.CustomerDateCreated.Month <= 4
                                                  && x.CustomerDateCreated.Year == DateTime.Now.Year
        ).Count();
        var customerChart2 = _context.Customers.Where(
            x => x.CustomerDateCreated.Month >= 4 && x.CustomerDateCreated.Month <= 8
                                                  && x.CustomerDateCreated.Year == DateTime.Now.Year
        ).Count();
        var customerChart3 = _context.Customers.Where(
            x => x.CustomerDateCreated.Month >= 8 && x.CustomerDateCreated.Month <= 12
                                                  && x.CustomerDateCreated.Year == DateTime.Now.Year
        ).Count();
        ViewBag.customerChart = JsonSerializer.Serialize(new List<int>
        {
            customerChart1,
            customerChart2,
            customerChart3,
            customerChart3,
            customerChart2,
            customerChart1
        });
        return View();
    }
}