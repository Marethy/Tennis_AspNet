namespace Tennis.Repositories;

using Tennis.Data;
using Tennis.Interfaces;
using Tennis.Models;

public class OrderRepository(TennisWebMVCContext context)
    : RepositoryBase<Order>(context), IOrderRepository
{
    public async Task UpdatePaymentState(int orderId)
    {
        var order = await _context.Orders.FindAsync(orderId);

        if (order == null)
        {
            return;
        }

        order.PaidState = true;
        await _context.SaveChangesAsync();
    }
}