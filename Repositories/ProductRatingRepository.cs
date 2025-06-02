namespace Tennis.Repositories;

using Tennis.Data;
using Tennis.Interfaces;
using Tennis.Models;

public class ProductRatingRepository(TennisWebMVCContext context)
    : RepositoryBase<ProductRating>(context), IProductRatingRepository
{
    //public async Task<ProductRating> GetProductRatingAsync(int id) 
    //    => await _context.ProductRatings.FindAsync(id);
    //public async Task<List<ProductRating>> GetAllProductRatingsAsync(int id) 
    //    => await _context.ProductRatings
    //        .Where(x => x.ProductId == id)
    //        .ToListAsync();

    //public async Task Save()=> await _context.SaveChangesAsync();

    //public async Task Add(ProductRating pd) => await _context.ProductRatings.AddAsync(pd);
}
