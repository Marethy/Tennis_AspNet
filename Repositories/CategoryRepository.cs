namespace Tennis.Repositories;

using Tennis.Data;
using Tennis.Interfaces;
using Tennis.Models;

public class CategoryRepository(TennisWebMVCContext context)
    : RepositoryBase<Category>(context), ICategoryRepository
{
    public Table[] GetRevenueStructure(int year)
    {
        return _context.OrderDetails
            .Where(o => o.Order.DayOrder.Year == year)
            .GroupBy(d => new { d.Product.Category.CategoryId, d.Product.Category.CategoryName })
            .Select(t => new Table
            {
                Key = t.Key.CategoryName,
                Value = (int)t.Sum(k => k.Quantity * k.UnitPrice)
            }).ToArray();
    }

    //public async Task<List<Category>> Get() 
    //    => await _context.Categories.ToListAsync();

    //public async Task<Category> GetById(int id) 
    //    => await _context.Categories.FindAsync(id);
}
