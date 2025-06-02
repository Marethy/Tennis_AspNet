namespace Tennis.Repositories;

using Tennis.Data;
using Tennis.Interfaces;
using Tennis.Models;

public class CustomerRepository(TennisWebMVCContext context)
    : RepositoryBase<Customer>(context), ICustomerRepository
{
    public List<Table> GetTopOrder(DateTime startDate, DateTime endDate)
    {
        var toporders = _context.Orders
            .Where(y => y.DayOrder >= startDate && y.DayOrder < endDate)
            .GroupBy(y => new { y.Customer.CustomerId, y.Customer.CustomerFullName })
            .Select(t => new Table
            {
                Key = t.Key.CustomerFullName,
                Value = t.Count()
            }).ToList();

        return toporders;
    }

    public List<Table> GetTopRevenue(DateTime startDate, DateTime endDate)
    {
        var toprevenue = _context.Orders
            .Where(y => y.DayOrder >= startDate && y.DayOrder < endDate)
            .GroupBy(y => new { y.Customer.CustomerId, y.Customer.CustomerFullName })
            .Select(t => new Table
            {
                Key = t.Key.CustomerFullName,
                Value = (int)t.Sum(k => k.TotalMoney)
            }).ToList();


        return toprevenue;
    }
}
