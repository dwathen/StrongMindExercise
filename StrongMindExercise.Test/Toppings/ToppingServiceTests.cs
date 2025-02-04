using Moq;
using StrongMindExercise.Application.Errors;
using StrongMindExercise.Application.Toppings.DTOs;
using StrongMindExercise.Application.Toppings.Services;
using StrongMindExercise.Domain.Toppings;

namespace StrongMindExercise.Test.Toppings;
public class ToppingServiceTests
{

    private readonly Mock<IToppingRepository> _mockToppingRepository;
    private readonly ToppingService _toppingService;

    public ToppingServiceTests()
    {
        this._mockToppingRepository = new Mock<IToppingRepository>();
        this._toppingService = new ToppingService(_mockToppingRepository.Object);
    }

    [Fact]
    public async Task GetAllToppingsAsync_ShouldReturnAllToppings()
    {
        // Arrange
        var toppings = new List<Topping>
        {
            new Topping { Id = 1, Name = "Pepperoni" },
            new Topping { Id = 2, Name = "Mushrooms" }
        };
        _mockToppingRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(toppings);

        // Act
        var result = await _toppingService.GetAllToppingsAsync();

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Contains(result, t => t.Name == "Pepperoni");
        Assert.Contains(result, t => t.Name == "Mushrooms");
    }

    [Fact]
    public async Task CreateToppingAsync_ShouldCreateTopping_WhenNotDuplicate()
    {
        // Arrange
        string toppingName = "Pepperoni";
        _mockToppingRepository.Setup(repo => repo.GetByNameAsync(toppingName))
                        .ReturnsAsync((Topping)null);
        ToppingCreateDTO toppingCreateDTO = new ToppingCreateDTO() { Name = toppingName };

        // Act
        var topping = await _toppingService.CreateToppingAsync(toppingCreateDTO);

        // Assert
        Assert.NotNull(topping);
        Assert.Equal(toppingName, topping.Data.Name);
        _mockToppingRepository.Verify(repo => repo.AddAsync(It.IsAny<Topping>()), Times.Once);
    }

    [Fact]
    public async Task CreateToppingAsync_ShouldReturnError_WhenDuplicate()
    {
        // Arrange
        string toppingName = "Pepperoni";
        _mockToppingRepository.Setup(repo => repo.GetByNameAsync(toppingName))
                        .ReturnsAsync(new Topping() { Name = toppingName });
        ToppingCreateDTO toppingCreateDTO = new ToppingCreateDTO() { Name = toppingName };

        // Act
        var topping = await _toppingService.CreateToppingAsync(toppingCreateDTO);

        // Assert
        Assert.Equal(CommonErrors.NameCannotBeDuplicate.Code, topping.Error.Code);
    }

    [Fact]
    public async Task UpdateToppingAsync_ShouldUpdateTopping_WhenValid()
    {
        // Arrange
        var topping = new Topping { Id = 1, Name = "Pepperoni" };
        _mockToppingRepository.Setup(repo => repo.GetByIdAsync(topping.Id)).ReturnsAsync(topping);
        ToppingUpdateDTO toppingUpdateDTO = new ToppingUpdateDTO { Id = topping.Id, Name = "Mushrooms" };

        // Act
        var result = await _toppingService.UpdateToppingAsync(toppingUpdateDTO);

        // Assert
        Assert.True(result.IsSuccess);
        _mockToppingRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Topping>()), Times.Once);
    }

    [Fact]
    public async Task UpdateToppingAsync_ShouldReturnError_WhenUpdateToDuplicateName()
    {
        // Arrange
        string toppingName = "Pepperoni";
        _mockToppingRepository.Setup(repo => repo.GetByNameAsync(toppingName))
                        .ReturnsAsync(new Topping() { Name = toppingName });
        var topping = new Topping { Id = 1, Name = "Mushroom" };
        _mockToppingRepository.Setup(repo => repo.GetByIdAsync(topping.Id)).ReturnsAsync(topping);
        ToppingUpdateDTO toppingUpdateDTO = new ToppingUpdateDTO { Id = topping.Id, Name = "Pepperoni" };

        // Act
        var result = await _toppingService.UpdateToppingAsync(toppingUpdateDTO);

        // Assert
        Assert.Equal(CommonErrors.NameCannotBeDuplicate.Code, result.Error.Code);
    }

    [Fact]
    public async Task DeleteToppingAsync_ShouldDeleteTopping_WhenExists()
    {
        // Arrange
        var topping = new Topping { Id = 1, Name = "Pepperoni" };
        _mockToppingRepository.Setup(repo => repo.GetByIdAsync(topping.Id)).ReturnsAsync(topping);

        // Act
        var result = await _toppingService.DeleteToppingAsync(topping.Id);

        // Assert
        Assert.True(result.IsSuccess);
        _mockToppingRepository.Verify(repo => repo.DeleteAsync(It.IsAny<Topping>()), Times.Once);
    }

    [Fact]
    public async Task DeleteToppingAsync_ShouldReturnFailure_WhenNotExists()
    {
        // Arrange
        int toppingId = 1;
        _mockToppingRepository.Setup(repo => repo.GetByIdAsync(toppingId)).ReturnsAsync((Topping)null);

        // Act
        var result = await _toppingService.DeleteToppingAsync(toppingId);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(CommonErrors.ObjectCannotBeFound("Topping").Code, result.Error.Code);
    }
}
