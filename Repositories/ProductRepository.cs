﻿using Microsoft.EntityFrameworkCore;
using Tennis.Data;
using Tennis.Interfaces;
using Tennis.Models;

namespace Tennis.Repositories;

public class ProductRepository(TennisWebMVCContext context)
    : RepositoryBase<Product>(context), IProductRepository
{
    // get list all product
    public List<Table> GetListProduct()
    {
        return _context.Products.Select(x => new Table
        {
            Key = x.ProductName,
            Value = x.ProductId
        }).ToList();
    }

    public Table[] GetProductAmountInYear(int productId)
    {
        Table[] data = new Table[13];
        for (var month = 1; month <= 12; month++)
        {
            //data[month] 
            var product = _context.Products.Include(x => x.OrderDetails).ThenInclude(y => y.Order)
                .FirstOrDefault(x => x.ProductId == productId);

            if (product != null)
            {
                var dataInMonth = product.OrderDetails
                    .Where(x => x.Order.DayOrder.Month == month && x.Order.DayOrder.Year == DateTime.Now.Year);
                if (dataInMonth.Count() != 0)
                    data[month] = dataInMonth
                        .Select(od => new Table
                        {
                            Key = "Tháng " + month,
                            Value = od.Quantity
                        }).FirstOrDefault();
                else
                    data[month] = new Table
                    {
                        Key = "Tháng " + month,
                        Value = 0
                    };
            }
            else
            {
                data[month] = new Table
                {
                    Key = "Tháng " + month,
                    Value = 0
                };
            }
        }

        return data;
    }

    public Table[] GetProductRevenueInYear(int productId)
    {
        Table[] data = new Table[13];
        for (var month = 1; month <= 12; month++)
        {
            var product = _context.Products.Include(x => x.OrderDetails).ThenInclude(y => y.Order)
                .FirstOrDefault(x => x.ProductId == productId);
            if (product != null)
            {
                var dataInMonth = product.OrderDetails
                    .Where(x => x.Order.DayOrder.Month == month && x.Order.DayOrder.Year == DateTime.Now.Year);
                if (dataInMonth.Count() != 0)
                    data[month] =
                        dataInMonth
                            .Select(od => new Table
                            {
                                Key = "Tháng " + month,
                                Value = (int)(od.Quantity * od.UnitPrice)
                            }).FirstOrDefault();
                else
                    data[month] = new Table
                    {
                        Key = "Tháng " + month,
                        Value = 0
                    };
            }
            else
            {
                data[month] = new Table
                {
                    Key = "Tháng " + month,
                    Value = 0
                };
            }
        }

        return data;
    }

    public Table[] GetProductWithAmount(DateTime startDate, DateTime endDate)
    {
        var product = _context.OrderDetails
            .Where(x => x.Order.DayOrder >= startDate && x.Order.DayOrder < endDate)
            .GroupBy(x => new { x.Product.ProductId, x.Product.ProductName })
            .Select(t => new Table
            {
                Key = t.Key.ProductName,
                Value = t.Sum(k => k.Quantity)
            })
            .ToArray();
        //.Join(_context.OrderDetails, p => p.ProductId, d => d.ProductId, (p, d) => new { p, d })
        //.Join(_context.Orders, g => g.d.OrderId, o => o.OrderId, (g, o) => new { g, o })
        //.Where(y => y.o.DayOrder >= startDate && y.o.DayOrder < endDate)
        //.GroupBy(x => new { x.g.p.ProductId, x.g.p.ProductName })
        //.Select(t => new Table
        //{
        //    Key = t.Key.ProductName,
        //    Value = t.Sum(x => x.g.d.Quantity),
        //}).ToArray();
        return product;
    }

    public Table[] GetProductWithRevenue(DateTime startDate, DateTime endDate)
    {
        var product = _context.OrderDetails
            .Where(x => x.Order.DayOrder >= startDate && x.Order.DayOrder < endDate)
            .GroupBy(x => new { x.Product.ProductId, x.Product.ProductName })
            .Select(t => new Table
            {
                Key = t.Key.ProductName,
                Value = (int)t.Sum(k => k.Quantity * k.UnitPrice)
            })
            .ToArray();

        //.Join(_context.OrderDetails, p => p.ProductId, d => d.ProductId, (p, d) => new { p, d })
        //.Join(_context.Orders, g => g.d.OrderId, o => o.OrderId, (g, o) => new { g, o })
        //.Where(y => y.o.DayOrder >= startDate && y.o.DayOrder < endDate)
        //.GroupBy(x => new { x.g.p.ProductId, x.g.p.ProductName })
        //.Select(t => new Table
        //{
        //    Key = t.Key.ProductName,
        //    Value = Convert.ToInt32(t.Sum(k => k.g.d.Quantity * k.g.d.UnitPrice)),
        //}).ToArray();
        return product;
    }

    public async Task UpdateAmount(int id)
    {
        var product = await _context.Products.FirstOrDefaultAsync(x => x.ProductId == id);
        product.ProductAmount = product.ProductAmount - 1;
        _context.Update(product);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateRating(int id)
    {
        var countStar = (int)_context.ProductRatings.Where(x => x.ProductId == id).Select(x => x.Stars).Average();
        var product = await _context.Products.FirstOrDefaultAsync(x => x.ProductId == id);
        product.ProductRating = countStar;
        _context.Update(product);
        await _context.SaveChangesAsync();
    }
}
