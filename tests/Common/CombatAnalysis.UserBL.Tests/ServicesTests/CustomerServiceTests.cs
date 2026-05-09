using AutoMapper;
using CombatAnalysis.UserBL.DTO;
using CombatAnalysis.UserBL.Mapping;
using CombatAnalysis.UserBL.Services;
using CombatAnalysis.UserDAL.Entities;
using CombatAnalysis.UserDAL.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;

namespace CombatAnalysis.UserBL.Tests.ServicesTests;

public class CustomerServiceTests
{
    [Fact]
    public async Task CreateAsync_ShouldCreateEntity()
    {
        // Arrange
        const string customerId = "uid-21";
        const string country = "Belarus";
        const string city = "Minsk";
        const int postalCode = 234234;
        const string appUserId = "uid-23";

        var customerDto = new CustomerDto(
            Id: customerId,
            Country: country,
            City: city,
            PostalCode: postalCode,
            AppUserId: appUserId
        );
        var customer = new Customer(
            Id: customerId,
            Country: country,
            City: city,
            PostalCode: postalCode,
            AppUserId: appUserId
        );

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<Customer, string>>();

        mockMapper.Setup(m => m.Map<Customer>(customerDto)).Returns(customer);
        mockMapper.Setup(m => m.Map<CustomerDto>(customer)).Returns(customerDto);

        mockRepository.Setup(m => m.CreateAsync(customer)).ReturnsAsync(customer);

        var service = new CustomerService(mockRepository.Object, mockMapper.Object);

        // Act
        var result = await service.CreateAsync(customerDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(customerDto.Id, result.Id);
        Assert.Equal(customerDto.Country, result.Country);
        Assert.Equal(customerDto.City, result.City);
        Assert.Equal(customerDto.PostalCode, result.PostalCode);
        Assert.Equal(customerDto.AppUserId, result.AppUserId);

        // Verify correct method calls
        mockMapper.Verify(m => m.Map<Customer>(It.IsAny<CustomerDto>()), Times.Once);
        mockRepository.Verify(r => r.CreateAsync(It.IsAny<Customer>()), Times.Once);
        mockMapper.Verify(m => m.Map<CustomerDto>(It.IsAny<Customer>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ThrowException_ShouldNotCreateEntityAsCountryEmpty()
    {
        // Arrange
        const string customerId = "uid-21";
        const string country = "";
        const string city = "Minsk";
        const int postalCode = 234234;
        const string appUserId = "uid-23";

        var customerDto = new CustomerDto(
            Id: customerId,
            Country: country,
            City: city,
            PostalCode: postalCode,
            AppUserId: appUserId
        );

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<Customer, string>>();

        var service = new CustomerService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentException>(nameof(CustomerDto.Country), () => service.CreateAsync(customerDto));

        // Verify correct method calls
        mockRepository.Verify(r => r.CreateAsync(It.IsAny<Customer>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_ThrowException_ShouldNotCreateEntityAsCityEmpty()
    {
        // Arrange
        const string customerId = "uid-21";
        const string country = "Belarus";
        const string city = "";
        const int postalCode = 234234;
        const string appUserId = "uid-23";

        var customerDto = new CustomerDto(
            Id: customerId,
            Country: country,
            City: city,
            PostalCode: postalCode,
            AppUserId: appUserId
        );

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<Customer, string>>();

        var service = new CustomerService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentException>(nameof(CustomerDto.City), () => service.CreateAsync(customerDto));

        // Verify correct method calls
        mockRepository.Verify(r => r.CreateAsync(It.IsAny<Customer>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_ThrowException_ShouldNotCreateEntityAsPostalCodeZero()
    {
        // Arrange
        const string customerId = "uid-21";
        const string country = "Belarus";
        const string city = "Minsk";
        const int postalCode = 0;
        const string appUserId = "uid-23";

        var customerDto = new CustomerDto(
            Id: customerId,
            Country: country,
            City: city,
            PostalCode: postalCode,
            AppUserId: appUserId
        );

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<Customer, string>>();

        var service = new CustomerService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(nameof(CustomerDto.PostalCode), () => service.CreateAsync(customerDto));

        // Verify correct method calls
        mockRepository.Verify(r => r.CreateAsync(It.IsAny<Customer>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_ThrowException_ShouldNotCreateEntityAsPostalCodeNegative()
    {
        // Arrange
        const string customerId = "uid-21";
        const string country = "Belarus";
        const string city = "Minsk";
        const int postalCode = -1;
        const string appUserId = "uid-23";

        var customerDto = new CustomerDto(
            Id: customerId,
            Country: country,
            City: city,
            PostalCode: postalCode,
            AppUserId: appUserId
        );

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<Customer, string>>();

        var service = new CustomerService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(nameof(CustomerDto.PostalCode), () => service.CreateAsync(customerDto));

        // Verify correct method calls
        mockRepository.Verify(r => r.CreateAsync(It.IsAny<Customer>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateEntity()
    {
        // Arrange
        const string customerId = "uid-21";

        const string country = "Belarus";
        const string city = "Grodno";
        const int postalCode = 234234;
        const string appUserId = "uid-23";

        var customerDto = new CustomerDto(
            Id: customerId,
            Country: country,
            City: city,
            PostalCode: postalCode,
            AppUserId: appUserId
        );
        var customer = new Customer(
            Id: customerId,
            Country: country,
            City: city,
            PostalCode: postalCode,
            AppUserId: appUserId
        );

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<Customer, string>>();

        mockMapper.Setup(m => m.Map<Customer>(customerDto)).Returns(customer);

        mockRepository.Setup(m => m.UpdateAsync(customerId, customer));

        var service = new CustomerService(mockRepository.Object, mockMapper.Object);

        // Act
        await service.UpdateAsync(customerId, customerDto);

        // Assert and Verify correct method calls
        mockMapper.Verify(m => m.Map<Customer>(It.IsAny<CustomerDto>()), Times.Once);
        mockRepository.Verify(r => r.UpdateAsync(It.IsAny<string>(), It.IsAny<Customer>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ThrowException_ShouldNotUpdateEntityAsIdEmpty()
    {
        // Arrange
        const string customerId = "uid-22";

        const string country = "";
        const string city = "Minsk";
        const int postalCode = 234234;
        const string appUserId = "uid-23";

        var customerDto = new CustomerDto(
            Id: customerId,
            Country: country,
            City: city,
            PostalCode: postalCode,
            AppUserId: appUserId
        );

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<Customer, string>>();

        var service = new CustomerService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentException>(() => service.UpdateAsync(string.Empty, customerDto));

        // Verify correct method calls
        mockRepository.Verify(r => r.UpdateAsync(It.IsAny<string>(), It.IsAny<Customer>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_ThrowException_ShouldNotUpdateEntityAsCountryEmpty()
    {
        // Arrange
        const string customerId = "uid-21";

        const string country = "";
        const string city = "Minsk";
        const int postalCode = 234234;
        const string appUserId = "uid-23";

        var customerDto = new CustomerDto(
            Id: customerId,
            Country: country,
            City: city,
            PostalCode: postalCode,
            AppUserId: appUserId
        );

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<Customer, string>>();

        var service = new CustomerService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentException>(nameof(CustomerDto.Country), () => service.UpdateAsync(customerId, customerDto));

        // Verify correct method calls
        mockRepository.Verify(r => r.UpdateAsync(It.IsAny<string>(), It.IsAny<Customer>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_ThrowException_ShouldNotUpdateEntityAsCityEmpty()
    {
        // Arrange
        const string customerId = "uid-21";

        const string country = "Belarus";
        const string city = "";
        const int postalCode = 234234;
        const string appUserId = "uid-23";

        var customerDto = new CustomerDto(
            Id: customerId,
            Country: country,
            City: city,
            PostalCode: postalCode,
            AppUserId: appUserId
        );

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<Customer, string>>();

        var service = new CustomerService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentException>(nameof(CustomerDto.City), () => service.UpdateAsync(customerId, customerDto));

        // Verify correct method calls
        mockRepository.Verify(r => r.UpdateAsync(It.IsAny<string>(), It.IsAny<Customer>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_ThrowException_ShouldNotUpdateEntityAsPostlaCodeZero()
    {
        // Arrange
        const string customerId = "uid-21";

        const string country = "Belarus";
        const string city = "Minsk";
        const int postalCode = 0;
        const string appUserId = "uid-23";

        var customerDto = new CustomerDto(
            Id: customerId,
            Country: country,
            City: city,
            PostalCode: postalCode,
            AppUserId: appUserId
        );

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<Customer, string>>();

        var service = new CustomerService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(nameof(CustomerDto.PostalCode), () => service.UpdateAsync(customerId, customerDto));

        // Verify correct method calls
        mockRepository.Verify(r => r.UpdateAsync(It.IsAny<string>(), It.IsAny<Customer>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_ThrowException_ShouldNotCreateEntityAsPostlaCodeNegative()
    {
        // Arrange
        const string customerId = "uid-21";

        const string country = "Belarus";
        const string city = "Minsk";
        const int postalCode = -1;
        const string appUserId = "uid-23";

        var customerDto = new CustomerDto(
            Id: customerId,
            Country: country,
            City: city,
            PostalCode: postalCode,
            AppUserId: appUserId
        );

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<Customer, string>>();

        var service = new CustomerService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(nameof(CustomerDto.PostalCode), () => service.UpdateAsync(customerId, customerDto));

        // Verify correct method calls
        mockRepository.Verify(r => r.UpdateAsync(It.IsAny<string>(), It.IsAny<Customer>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteEntity()
    {
        // Arrange
        const string customerId = "uid-22";

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<Customer, string>>();

        var service = new CustomerService(mockRepository.Object, mockMapper.Object);

        // Act
        await service.DeleteAsync(customerId);

        // Verify correct method calls
        mockRepository.Verify(r => r.DeleteAsync(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ThrowException_ShouldNotDeleteEntity()
    {
        // Arrange
        const string customerId = "";

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<Customer, string>>();

        var service = new CustomerService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentException>(() => service.DeleteAsync(customerId));

        // Verify correct method calls
        mockRepository.Verify(r => r.DeleteAsync(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task GetAllAsync_NotEmptyCollection_ShouldReturnFewElementsInCollection()
    {
        // Arrange
        const string customerId = "uid-21";
        const string country = "Belarus";
        const string city = "Minsk";
        const int postalCode = 234234;
        const string appUserId = "uid-23";

        var customersDto = new List<CustomerDto> {
            new(
                Id: customerId,
                Country: country,
                City: city,
                PostalCode: postalCode,
                AppUserId: appUserId
            )
        };
        var customers = new List<Customer> {
            new(
                Id: customerId,
                Country: country,
                City: city,
                PostalCode: postalCode,
                AppUserId: appUserId
            ) 
        };

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<Customer, string>>();

        mockMapper.Setup(m => m.Map<IEnumerable<CustomerDto>>(customers)).Returns(customersDto);

        mockRepository.Setup(m => m.GetAllAsync()).ReturnsAsync(customers);

        var service = new CustomerService(mockRepository.Object, mockMapper.Object);

        // Act
        var result = await service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Single(result);

        // Verify correct method calls
        mockRepository.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_EmptyCollection_ShouldReturnEmptyCollection()
    {
        // Arrange
        var customers = new List<Customer>();
        var customersDto = new List<CustomerDto>();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<Customer, string>>();

        mockMapper.Setup(m => m.Map<IEnumerable<CustomerDto>>(customers)).Returns(customersDto);

        mockRepository.Setup(m => m.GetAllAsync()).ReturnsAsync(customers);

        var service = new CustomerService(mockRepository.Object, mockMapper.Object);

        // Act
        var result = await service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);

        // Verify correct method calls
        mockRepository.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_OneEntity_ShouldReturnOneEntity()
    {
        // Arrange
        const string customerId = "uid-21";
        const string country = "Belarus";
        const string city = "Minsk";
        const int postalCode = 234234;
        const string appUserId = "uid-23";

        var customerDto = new CustomerDto(
            Id: customerId,
            Country: country,
            City: city,
            PostalCode: postalCode,
            AppUserId: appUserId
        );
        var customer = new Customer(
            Id: customerId,
            Country: country,
            City: city,
            PostalCode: postalCode,
            AppUserId: appUserId
        );

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<Customer, string>>();

        mockMapper.Setup(m => m.Map<CustomerDto>(customer)).Returns(customerDto);

        mockRepository.Setup(m => m.GetByIdAsync(customerId)).ReturnsAsync(customer);

        var service = new CustomerService(mockRepository.Object, mockMapper.Object);

        // Act
        var result = await service.GetByIdAsync(customerId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(customerId, result.Id);

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByIdAsync(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_NoAnyEntity_ShouldReturnNoAnyEntity()
    {
        // Arrange
        const string customerId = "uid-21";
        const string country = "Belarus";
        const string city = "Minsk";
        const int postalCode = 234234;
        const string appUserId = "uid-23";

        var customerDto = new CustomerDto(
            Id: customerId,
            Country: country,
            City: city,
            PostalCode: postalCode,
            AppUserId: appUserId
        );
        var customer = new Customer(
            Id: customerId,
            Country: country,
            City: city,
            PostalCode: postalCode,
            AppUserId: appUserId
        );

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<Customer, string>>();

        mockMapper.Setup(m => m.Map<CustomerDto>(customer)).Returns(customerDto);

        var service = new CustomerService(mockRepository.Object, mockMapper.Object);

        // Act
        var result = await service.GetByIdAsync(customerId);

        // Assert
        Assert.Null(result);

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByIdAsync(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ThrowExecption_ShouldNotReturnEntity()
    {
        // Arrange
        const string customerId = "";

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<Customer, string>>();

        var service = new CustomerService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentException>(() => service.GetByIdAsync(customerId));

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByIdAsync(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task GetByParamAsync_NotEmptyCollection_ShouldReturnFewElementsInCollection()
    {
        // Arrange
        const string customerId = "uid-21";
        const string country = "Belarus";
        const string city = "Minsk";
        const int postalCode = 234234;
        const string appUserId = "uid-23";

        var customersDto = new List<CustomerDto> {
            new(
                Id: customerId,
                Country: country,
                City: city,
                PostalCode: postalCode,
                AppUserId: appUserId
            )
        };
        var customers = new List<Customer> {
            new(
                Id: customerId,
                Country: country,
                City: city,
                PostalCode: postalCode,
                AppUserId: appUserId
            )
        };
        Expression<Func<Customer, string>> expression = c => c.Country;
        Expression<Func<CustomerDto, string>> expressionDto = c => c.Country;

        var mockRepository = new Mock<IGenericRepository<Customer, string>>();

        // Use real Automapper as method call MapperExpresiion extension
        var loggerFactory = LoggerFactory.Create(builder => { });
        var config = new MapperConfiguration(cfg => cfg.AddProfile<UserBLMapper>(), loggerFactory);
        var mapper = config.CreateMapper();

        mockRepository
            .Setup(r => r.GetByParamAsync(It.IsAny<Expression<Func<Customer, string>>>(), country))
            .ReturnsAsync(customers);

        var service = new CustomerService(mockRepository.Object, mapper);

        // Act
        var result = await service.GetByParamAsync(c => c.Country, country);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Single(result);

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByParamAsync(It.IsAny<Expression<Func<Customer, string>>>(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task GetByParamAsync_EmptyCollection_ShouldReturnEmptyCollection()
    {
        // Arrange
        const string calledCountry = "Belarus";

        var customerUsers = new List<Customer>();
        var customerUsersDto = new List<CustomerDto>();

        Expression<Func<Customer, string>> expression = c => c.Country;
        Expression<Func<CustomerDto, string>> expressionDto = c => c.Country;

        var mockRepository = new Mock<IGenericRepository<Customer, string>>();

        // Use real Automapper as method call MapperExpresiion extension
        var loggerFactory = LoggerFactory.Create(builder => { });
        var config = new MapperConfiguration(cfg => cfg.AddProfile<UserBLMapper>(), loggerFactory);
        var mapper = config.CreateMapper();

        mockRepository
            .Setup(r => r.GetByParamAsync(It.IsAny<Expression<Func<Customer, string>>>(), calledCountry))
            .ReturnsAsync(customerUsers);

        var service = new CustomerService(mockRepository.Object, mapper);

        // Act
        var result = await service.GetByParamAsync(c => c.Country, calledCountry);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByParamAsync(It.IsAny<Expression<Func<Customer, string>>>(), It.IsAny<string>()), Times.Once);
    }
}
