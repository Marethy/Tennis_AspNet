﻿namespace Tennis.Interfaces;

using Tennis.Models;

public interface ICartRepository
{
    //public Task<List<Item>> GetAll();

    public List<Item> Get(ISession session);

    public List<Item> Set(ISession session, List<Item> cart);

}