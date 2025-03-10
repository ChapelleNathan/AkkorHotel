﻿using Backend.Models;

namespace Backend.Repository.HotelRepository;

public interface IHotelRepository : IRepository
{
    public Task<Hotel> CreateHotel(Hotel hotel);
    public Task<Hotel> GetHotelById(string id);
    public Task<IEnumerable<Hotel>> GetHotels();
    public Task<Hotel> UpdateHotel(Hotel hotel);
    public Hotel DeleteHotel(string id);
}