using System.Security.Claims;
using AutoMapper;
using Backend.DTO;
using Backend.Enum;
using Backend.Helper;
using Backend.HttpResponse;
using Backend.Models;
using Backend.Repository.UserRepository;
using Backend.Service.UserServices;
using BackendTest.Context;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Xunit.Abstractions;

namespace BackendTest.Service;

public class UserServiceTest : IClassFixture<DataContextTest>
{
    private readonly IUserService _userService, _fakedUserService;
    private readonly IUserRepository _fakeUserRepository;
    private readonly DataContextTest _context;
    private readonly ITestOutputHelper _output;
    
    private readonly IHttpContextAccessor _fakeHttpContextAccessor = A.Fake<IHttpContextAccessor>();
    private readonly HttpContext _fakeHttpContext = A.Fake<HttpContext>();
    
    public UserServiceTest(DataContextTest context, ITestOutputHelper output)
    {
        _fakeUserRepository = A.Fake<IUserRepository>();
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<CreateUserDto, User>();
        });
        _context = context;
        var mapper = mapperConfig.CreateMapper();
        _userService = new UserService(context.UserRepository, mapper);
        _fakedUserService = new UserService(_fakeUserRepository, mapper);
        _output = output;
    }

    [Theory]
    [InlineData("john", "doe", "john.doe@gmail.com", "Password123.", "doejohn")]
    [InlineData("john","doe","johndoe@outlook.fr", "Password123.", "doejohn")]
    [InlineData("john", "doe", "john-doe@127.0.0.1", "Password123.", "doejohn")]
    [InlineData("john", "doe", "john@doe.toto", "Password123.", "doejohn")]
    public async Task CreateUserAsync_Test(string firstname, string lastname, string email, string password, string pseudo)
    {
        var createUserDto = new CreateUserDto{Firstname = firstname, Lastname = lastname, Email = email, Password = password, Pseudo = pseudo};
        
        var newUser = await _userService.CreateUser(createUserDto);
        
        Assert.Equal(newUser.Firstname, createUserDto.Firstname);
        Assert.Equal(newUser.Lastname, createUserDto.Lastname);
        Assert.Equal(newUser.Email, createUserDto.Email);
    }
    [Theory]
    [InlineData("john", "doe", "johndoe@user.com", "Password123.", "doejohn")]
    public async Task CreateUserAsync_EmailExists_Test(string firstname, string lastname, string email, string password, string pseudo)
    {
        var createUserDto = new CreateUserDto
        {
            Firstname = firstname,
            Lastname = lastname,
            Email = email,
            Password = password,
            Pseudo = pseudo
        };
        var returnedUser = new User(firstname, lastname, email, password, pseudo);
        
        A.CallTo(() => _fakeUserRepository.GetUserByEmail(A<string>._)).Returns(Task.FromResult(returnedUser));
        
        var exception = await Assert.ThrowsAsync<HttpResponseException>(() => _fakedUserService.CreateUser(createUserDto));
        Assert.True(exception.StatusCode.Equals(401));
        Assert.Equal(exception.Message, ErrorHelper.GetErrorMessage(ErrorMessageEnum.Sup401EmailTaken));
    }
    [Theory]
    [InlineData("john", "doe","johnn@doe.com", "Password123.", "doejohn")]
    public async Task CreateUser_ErrorDb_test(string firstname, string lastname, string email, string password, string pseudo)
    {
        var createUserDto = new CreateUserDto{Firstname = firstname, Lastname = lastname, Email = email, Password = password, Pseudo = pseudo};
        
        User? nullUser = null;
        A.CallTo(() => _fakeUserRepository.GetUserByEmail(A<string>._)).Returns(Task.FromResult<User>(null));
        A.CallTo(() => _fakeUserRepository.CreateUser(A<User>._)).Throws<Exception>();
        
        var exception = await Assert.ThrowsAsync<HttpResponseException>(() => _fakedUserService.CreateUser(createUserDto));
        Assert.True(exception.StatusCode.Equals(500));
        Assert.Equal(exception.Message, ErrorHelper.GetErrorMessage(ErrorMessageEnum.Sup500UnknownError)); 
    }

    [Theory]
    [InlineData("john", "doe", "john@doe.a", "Password123.", "doejohn")]
    [InlineData("john", "doe", "john@doe+12.com", "Password123.", "doejohn")]
    [InlineData("john", "doe", "john+test@doe.com", "Password123.", "doejohn")]
    public async Task CreateUser_InvalidEmail_Test(string firstname,string lastname,string email,string password, string pseudo)
    {
        var createdUserDto = new CreateUserDto{Firstname = firstname, Lastname = lastname, Email = email, Password = password, Pseudo = pseudo};
        var exception = await Assert.ThrowsAsync<HttpResponseException>(() => _userService.CreateUser(createdUserDto));
        Assert.True(exception.StatusCode.Equals(400));
        Assert.Equal(exception.Message, ErrorHelper.GetErrorMessage(ErrorMessageEnum.Sup400EmailNotAccepted));
    }


    [Theory]
    [InlineData("john", "doe",
        "johnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnn@doe.com",
        "Password123.", "doejohn")]
    public async Task CreateUser_TooLongEmail_Test(string firstname, string lastname, string email, string password, string pseudo)
    {
        var createUserDto = new CreateUserDto{Firstname = firstname, Lastname = lastname, Email = email, Password = password, Pseudo = pseudo};
        var exception = await Assert.ThrowsAsync<HttpResponseException>(() => _userService.CreateUser(createUserDto));
        Assert.True(exception.StatusCode.Equals(400));
        Assert.Equal(exception.Message, ErrorHelper.GetErrorMessage(ErrorMessageEnum.Sup400TooLongEmail));
    }

    [Theory]
    [InlineData("johnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnn",
        "doe", "azdqsd@doe.com", "Password123.", "doejohn")]
    public async Task CreateUser_InvalidFirstName_test(string firstname, string lastname, string email, string password, string pseudo)
    {
        var createUserDto = new CreateUserDto{Firstname = firstname, Lastname = lastname, Email = email, Password = password, Pseudo = pseudo};
        var exception = await Assert.ThrowsAsync<HttpResponseException>(() => _userService.CreateUser(createUserDto));
        Assert.True(exception.StatusCode.Equals(400));
        Assert.Equal(exception.Message, ErrorHelper.GetErrorMessage(ErrorMessageEnum.Sup400TooLongFirstname));
    }

    [Theory]
    [InlineData("john",
        "doeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee",
        "john.test@doe.com", "Password123.", "doejohn")]
    public async Task CreateUser_InvalidLastName_Test(string firstname, string lastname, string email, string password, string pseudo)
    {
        var createUserDto = new CreateUserDto{Firstname = firstname, Lastname = lastname, Email = email, Password = password, Pseudo = pseudo};
        var exception = await Assert.ThrowsAsync<HttpResponseException>(() => _userService.CreateUser(createUserDto));
        Assert.True(exception.StatusCode.Equals(400));
        Assert.Equal(exception.Message, ErrorHelper.GetErrorMessage(ErrorMessageEnum.Sup400TooLongLastname));
    }

    [Theory]
    [InlineData("john", "doe", "invalidpassword1@doe.com", "password123.", "doejohn")]
    [InlineData("john", "doe", "invalidpassword2@doe.com", "Passworddddddd.", "doejohn")]
    [InlineData("john", "doe", "invalidpassword3@doe.com", "Password1234", "doejohn")]
    public async Task CreateUser_InvalidPassword_Test(string firstname, string lastname, string email, string password, string pseudo)
    {
        var createUserDto = new CreateUserDto{Firstname = firstname, Lastname = lastname, Email = email, Password = password, Pseudo = pseudo};
        var exception = await Assert.ThrowsAsync<HttpResponseException>(() => _userService.CreateUser(createUserDto));
        Assert.True(exception.StatusCode.Equals(400));
        Assert.Equal(exception.Message, ErrorHelper.GetErrorMessage(ErrorMessageEnum.Sup400PasswordFormat));
    }

    [Theory]
    [InlineData("john", "doe", "tooloogpassword@doe.com", "password123password123password123password123password123password123password123password123password123password123password123password123password123password123password123password123password123password123password123password123password123password123password123password123password123password123.", "doejohn")]
    public async Task CreateUser_TooLongPassword_Test(string firstname, string lastname, string email, string password, string pseudo)
    {
        var createUserDto = new CreateUserDto{Firstname = firstname, Lastname = lastname, Email = email, Password = password, Pseudo = pseudo};
        var exception = await Assert.ThrowsAsync<HttpResponseException>(() => _userService.CreateUser(createUserDto));
        Assert.True(exception.StatusCode.Equals(400));
        Assert.Equal(exception.Message, ErrorHelper.GetErrorMessage(ErrorMessageEnum.Sup400TooLongPassword));
    }

    [Theory]
    [InlineData("john", "doe", "tooShostPassword@doe.com", "Pd123.", "doejohn")]
    public async Task CreateUser_TooShortPassword_Test(string firstname, string lastname, string email, string password, string pseudo)
    {
        var createUserDto = new CreateUserDto{Firstname = firstname, Lastname = lastname, Password = password, Email = email, Pseudo = pseudo};
        var exception = await Assert.ThrowsAsync<HttpResponseException>(() => _userService.CreateUser(createUserDto));
        Assert.True(exception.StatusCode.Equals(400));
        Assert.Equal(exception.Message, ErrorHelper.GetErrorMessage(ErrorMessageEnum.Sup400TooShortPassword));
    }

    [Theory]
    [InlineData("john", "doe", "getbyid@gmail.com", "Password123.", "doejohn")]
    public async Task GetUserById_Test(string firstname, string lastname, string email, string password, string pseudo)
    {
        
        var createUserDto = new CreateUserDto{Firstname = firstname, Lastname = lastname, Email = email, Password = password, Pseudo = pseudo};
        var user = await _userService.CreateUser(createUserDto);

        var appUserDto = CreateFakeAppUserDto(email, user.Id.ToString(), user.Role);

        
        var fetchedUser = await _userService.GetUserById(user.Id.ToString(), appUserDto);

        Assert.Equal(user, fetchedUser);
    }

    [Fact]
    public async Task GetUserById_NotFound_Test()
    {
        var appAdminDto = CreateFakeAppUserDto("admin@gmail.com", _context.AdminId.ToString(), RoleEnum.Admin);
        
        await Assert.ThrowsAsync<HttpResponseException>(() => _userService.GetUserById(Guid.NewGuid().ToString(), appAdminDto));
    }

    [Theory]
    [InlineData("john", "doe", "updateUserTest@doe.com", "Password123.", "doejohn")]
    public async Task UpdateUser_Test(string firstname, string lastname, string email, string password, string pseudo)
    {
        var appUserDto = CreateFakeAppUserDto(email, _context.UserId.ToString(), RoleEnum.User);
        
        var user = await _userService.GetUserById(_context.UserId.ToString(), appUserDto);
        var originalFirstname = user.Firstname;
        var originalLastname = user.Lastname;
        var originalEmail = user.Email;
        var originalPassword = user.Password;
        var originalPseudo = user.Pseudo;
        
        var updatedUser = new UpdatedUserDto{Firstname = firstname, Email = email, Lastname = lastname, Password = password, Id = _context.UserId.ToString(), Pseudo = pseudo};
        var updatedUserDto = await _userService.UpdateUser(updatedUser, appUserDto);
        
        Assert.NotEqual(originalFirstname, updatedUserDto.Firstname);
        Assert.NotEqual(originalLastname, updatedUserDto.Lastname);
        Assert.NotEqual(originalEmail, updatedUserDto.Email);
        Assert.NotEqual(originalPseudo, updatedUserDto.Pseudo);
        Assert.True(BCrypt.Net.BCrypt.Verify(password, originalPassword));
    }

    [Fact]
    public async Task DeleteUser_Test()
    {
        var appUserDto = CreateFakeAppUserDto("test@user.com", _context.UserId.ToString(), RoleEnum.User);
        
        var user = await _userService.DeleteUserById(_context.UserId.ToString(), appUserDto);
        Assert.NotNull(user);
        
        var appAdminDto = CreateFakeAppUserDto("admin@gmail.com", _context.AdminId.ToString(), RoleEnum.Admin);
        
        var exception = await Assert.ThrowsAsync<HttpResponseException>(() =>  _userService.GetUserById(_context.UserId.ToString(), appAdminDto));
        Assert.True(exception.StatusCode.Equals(404));
        Assert.Equal(exception.Message, ErrorHelper.GetErrorMessage(ErrorMessageEnum.Sup404UserNotFound));
    }

    private AppUserDto CreateFakeAppUserDto(string email, string id, RoleEnum role)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Sid, id),
            new Claim(ClaimTypes.Role, role.ToString()),
        };
        var identity = new ClaimsIdentity(claims);
        var claimsPrincipal = new ClaimsPrincipal(identity);
        A.CallTo(() => _fakeHttpContext.User).Returns(claimsPrincipal);
        A.CallTo(() => _fakeHttpContextAccessor.HttpContext).Returns(_fakeHttpContext);
        return new AppUserDto(_fakeHttpContextAccessor);
    }
}