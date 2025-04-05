using Tennis.Interfaces;
using Tennis.Models;
using Newtonsoft.Json;

namespace Tennis.Repositories;

public class CartRepository : ICartRepository
{
	private const string CART = "Cart";

	public List<Item> Get(ISession session)
	{
		var value = session.GetString(CART);
		if (value == null)
			return new List<Item>();
		var result = JsonConvert.DeserializeObject<List<Item>>(value);
		if (result == null)
			return new List<Item>();
		return result;
	}

	public List<Item> Set(ISession session, List<Item> cart)
	{
		session.SetString(CART, JsonConvert.SerializeObject(cart));
		return cart;
	}
}