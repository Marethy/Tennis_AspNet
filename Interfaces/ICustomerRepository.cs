using Tennis.Models;

namespace Tennis.Interfaces;

public interface ICustomerRepository : IRepository<Customer>
{
	public List<Table> GetTopOrder(DateTime startDate, DateTime endDate);

	public List<Table> GetTopRevenue(DateTime startDate, DateTime endDate);
}