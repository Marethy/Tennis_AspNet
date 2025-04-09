namespace Tennis.Repositories;

using Newtonsoft.Json;
using Tennis.Interfaces;
using Tennis.Models;

public class CartRepository : ICartRepository
{
    private const string CART = "Cart";

    public List<Item> Get(ISession session)
    {
        var value = session.GetString(CART);
        if (value == null)
        {
            return [];
        }

        var result = JsonConvert.DeserializeObject<List<Item>>(value);
        if (result == null)
        {
            return [];
        }

        return result;
    }

    public List<Item> Set(ISession session, List<Item> cart)
    {
        session.SetString(CART, JsonConvert.SerializeObject(cart));
        return cart;
    }
}
