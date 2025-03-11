using System.Net;
using AutoMapper;
using Backend.Context;
using Backend.DTO;
using Backend.Enum;
using Backend.Helper;
using Backend.HttpResponse;
using Backend.Models;
using Backend.Repository.HotelRepository;
using Backend.Repository.UserRepository;
using Backend.Service.HotelService;
using BackendTest.Context;
using Bogus;
using FakeItEasy;
using FluentAssertions;

namespace BackendTest.Service;

public class HotelServiceTest : IClassFixture<DataContextTest>
{
    private readonly HotelService _hotelService, _fakeHotelService;
    private readonly IHotelRepository _fakeHotelRepository = A.Fake<IHotelRepository>();
    private readonly DataContextTest _context;
    private readonly Faker _faker = new Faker();
    
    public HotelServiceTest(DataContextTest context)
    {
        _context = context;
        
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<CreateHotelDto, Hotel>();
        });
        var mapper = mapperConfig.CreateMapper();

        _fakeHotelService = new HotelService(_fakeHotelRepository, mapper);
        _hotelService = new HotelService(context.HotelRepository, mapper);
    }

    //Create
    [Fact]
    public async Task CreateHotel_Test()
    {
        var createHotelDto = CreateRandomHotel();
        var hotel = await _hotelService.CreateHotel(createHotelDto);
        Assert.NotNull(hotel);
        Assert.Equal(createHotelDto.Name, hotel.Name);
        Assert.Equal(createHotelDto.Location, hotel.Location);
        Assert.Equal(createHotelDto.Description, hotel.Description);
    }

    [Fact]
    public async Task CreateHotel_TooLongName_Test()
    {
        var createHotelDto = CreateRandomHotel();
        createHotelDto.Name = _faker.Random.AlphaNumeric(51);
        var exception = await Assert.ThrowsAsync<HttpResponseException>(() => _hotelService.CreateHotel(createHotelDto));
        Assert.True(exception.StatusCode.Equals(400));
        Assert.Equal(exception.Message, ErrorHelper.GetErrorMessage(ErrorMessageEnum.Sup400TooLongHotelName));
    }

    [Fact]
    public async Task CreateHotel_TooLongLocation_Test()
    {
        var createHotelDto = CreateRandomHotel();
        createHotelDto.Location = _faker.Random.AlphaNumeric(101);
        var exception = await Assert.ThrowsAsync<HttpResponseException>(() => _hotelService.CreateHotel(createHotelDto));
        Assert.True(exception.StatusCode.Equals(400));
        Assert.Equal(exception.Message, ErrorHelper.GetErrorMessage(ErrorMessageEnum.Sup400TooLongHotelLocation));
    }

    [Fact]
    public async Task CreateHotel_TooLongDescription_Test()
    {
        var createHotelDto = CreateRandomHotel();
        createHotelDto.Description = _faker.Random.AlphaNumeric(256);
        var exception = await Assert.ThrowsAsync<HttpResponseException>(() => _hotelService.CreateHotel(createHotelDto));
        Assert.True(exception.StatusCode.Equals(400));
        Assert.Equal(exception.Message, ErrorHelper.GetErrorMessage(ErrorMessageEnum.Sup400TooLongHotelDescription));
    }

    [Fact]
    private async Task CreateHotel_UnexpectedError_Test()
    {
        var createHotelDto = CreateRandomHotel();
        A.CallTo(() => _fakeHotelRepository.CreateHotel(A<Hotel>._)).Throws<Exception>();
        var exception = await Assert.ThrowsAsync<HttpResponseException>(() => _fakeHotelService.CreateHotel(createHotelDto));
        Assert.True(exception.StatusCode.Equals(500));
        Assert.Equal(exception.Message, ErrorHelper.GetErrorMessage(ErrorMessageEnum.Sup500UnknownError));
    }
    
    //Get

    [Fact]
    private async Task GetHotel_Test()
    {
        var hotel = await _hotelService.GetHotelById(_context.HotelId.ToString());
        Assert.NotNull(hotel);
        Assert.True(hotel.Id.Equals(_context.HotelId));
    }

    [Fact]
    private async Task GetHotels_HotelNotFound_Test()
    {
        var exception = await Assert.ThrowsAsync<HttpResponseException>(() => _hotelService.GetHotelById(Guid.NewGuid().ToString()));
        Assert.True(exception.StatusCode.Equals(404));
        Assert.Equal(exception.Message, ErrorHelper.GetErrorMessage(ErrorMessageEnum.Sup404HotelNotFound));
    }

    //Update
    [Fact]
    private async Task UpdateHotel_Test()
    {
        var updateHotelDto = UpdateRandomHotel();
        var hotel = await _hotelService.GetHotelById(updateHotelDto.Id.ToString());
        var originalHotelName = hotel.Name;
        var originalLocation = hotel.Location;
        var originalDescription = hotel.Description;
        updateHotelDto.Id = hotel.Id;
    
        var updatedHotel =  await _hotelService.UpdateHotel(updateHotelDto);
        Assert.NotNull(updatedHotel);
        Assert.NotEqual(originalHotelName, updatedHotel.Name);
        Assert.NotEqual(originalLocation, updatedHotel.Location);
        Assert.NotEqual(originalDescription, updatedHotel.Description);
    }
    
    [Fact]
    private async Task UpdateHotel_HotelNotFound_Test()
    {
        var updateHotelDto = UpdateRandomHotel();
        A.CallTo(() => _fakeHotelRepository.GetHotelById(A<string>._)).Returns((Hotel)null);
        var exception = await Assert.ThrowsAsync<HttpResponseException>(() => _fakeHotelService.UpdateHotel(updateHotelDto));
        Assert.True(exception.StatusCode.Equals(404));
        Assert.Equal(exception.Message, ErrorHelper.GetErrorMessage(ErrorMessageEnum.Sup404HotelNotFound));
    }
    
    [Fact]
    private async Task UpdateHotel_UnexpectedError_Test()
    {
        var updateHotelDto = UpdateRandomHotel();
        updateHotelDto.Id = _context.HotelId;
        A.CallTo(() => _fakeHotelRepository.UpdateHotel(A<Hotel>._)).Throws<Exception>();
        var exception = await Assert.ThrowsAsync<HttpResponseException>(() => _fakeHotelService.UpdateHotel(updateHotelDto));
        Assert.True(exception.StatusCode.Equals(500));
        Assert.Equal(exception.Message, ErrorHelper.GetErrorMessage(ErrorMessageEnum.Sup500UnknownError));
    }

    private CreateHotelDto CreateRandomHotel()
    {
        return new CreateHotelDto
        {
            Name = _faker.Company.CompanyName(),
            Location = _faker.Address.FullAddress(),
            Description = _faker.Lorem.Sentence(),
        };
    }

    private UpdateHotelDto UpdateRandomHotel()
    {
        return new UpdateHotelDto
        {
            Id = _context.HotelId,
            Name = _faker.Company.CompanyName(),
            Location = _faker.Address.FullAddress(),
            Description = _faker.Lorem.Sentence(),
        };
    }
    
}