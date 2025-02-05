using Moq;
using StrongMindExercise.Application.Errors;
using StrongMindExercise.Application.Pizzas.DTOs;
using StrongMindExercise.Application.Pizzas.Services;
using StrongMindExercise.Domain.Pizzas;
using StrongMindExercise.Domain.Toppings;

namespace StrongMindExercise.Test.Pizzas;
public class PizzaServiceTests
{
    private readonly Mock<IPizzaRepository> _mockPizzaRepository;
    private readonly Mock<IToppingRepository> _mockToppingRepository;
    private readonly PizzaService _pizzaService;

    public PizzaServiceTests()
    {
        this._mockPizzaRepository = new Mock<IPizzaRepository>();
        this._mockToppingRepository = new Mock<IToppingRepository>();
        this._pizzaService = new PizzaService(_mockPizzaRepository.Object, _mockToppingRepository.Object);
    }

    [Fact]
    public async Task GetAllPizzasAsync_ShouldReturnAllPizzas()
    {
        // Arrange
        var pizzas = new List<Pizza>
        {
            new Pizza { Id = 1, Name = "Mushroom", Toppings = new List<Topping>() { new() { Id = 1, Name = "Mushroom" } } },
            new Pizza { Id = 2, Name = "Pepperoni", Toppings = new List<Topping>() { new() { Id = 1, Name = "Pepperoni" } } }
        };
        _mockPizzaRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(pizzas);

        // Act
        var result = await _pizzaService.GetAllPizzasAsync();

        // Assert
        Assert.Equal(2, result.Data.Count());
        Assert.Contains(result.Data, p => p.Name == "Mushroom");
        Assert.Contains(result.Data, p => p.Name == "Pepperoni");
    }

    [Fact]
    public async Task CreatePizzaAsync_ShouldCreatePizza_WhenNotDuplicate()
    {
        // Arrange
        string pizzaName = "Pepperoni";
        _mockPizzaRepository.Setup(repo => repo.GetByNameAsync(pizzaName))
                        .ReturnsAsync((Pizza)null);
        var topping = new Topping { Id = 1, Name = "Pepperoni" };
        _mockToppingRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(topping);

        PizzaCreateDTO pizzaCreateDTO = new PizzaCreateDTO() { Name = pizzaName, ToppingIds = new List<int>() { 1 } };

        // Act
        var pizza = await _pizzaService.CreatePizzaAsync(pizzaCreateDTO);

        // Assert
        Assert.NotNull(pizza);
        Assert.Equal(pizzaName, pizza.Data.Name);
        _mockPizzaRepository.Verify(repo => repo.AddAsync(It.IsAny<Pizza>()), Times.Once);
    }

    [Fact]
    public async Task CreatePizzaAsync_ShouldReturnError_WhenDuplicate()
    {
        // Arrange
        string pizzaName = "Pepperoni";
        _mockPizzaRepository.Setup(repo => repo.GetByNameAsync(pizzaName))
                        .ReturnsAsync(new Pizza() { Name = pizzaName });
        PizzaCreateDTO pizzaCreateDTO = new PizzaCreateDTO() { Name = pizzaName };

        // Act
        var pizza = await _pizzaService.CreatePizzaAsync(pizzaCreateDTO);

        // Assert
        Assert.Equal(CommonErrors.NameCannotBeDuplicate.Code, pizza.Error.Code);
    }

    [Fact]
    public async Task CreatePizzaAsync_ShouldReturnError_WhenZeroChildren()
    {
        // Arrange
        string pizzaName = "Pepperoni";
        _mockPizzaRepository.Setup(repo => repo.GetByNameAsync(pizzaName))
                        .ReturnsAsync((Pizza)null);
        var topping = new Topping { Id = 1, Name = "Pepperoni" };
        _mockToppingRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(topping);

        PizzaCreateDTO pizzaCreateDTO = new PizzaCreateDTO() { Name = pizzaName, ToppingIds = new List<int>() { } };

        // Act
        var pizza = await _pizzaService.CreatePizzaAsync(pizzaCreateDTO);

        // Assert
        Assert.Equal(CommonErrors.MustHaveAtLeastOneChild.Code, pizza.Error.Code);
    }

    [Fact]
    public async Task CreatePizzaAsync_ShouldReturnError_WhenDuplicateChildren()
    {
        // Arrange
        string pizzaName = "Pepperoni";
        _mockPizzaRepository.Setup(repo => repo.GetByNameAsync(pizzaName))
                        .ReturnsAsync((Pizza)null);
        var topping = new Topping { Id = 1, Name = "Pepperoni" };
        _mockToppingRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(topping);

        PizzaCreateDTO pizzaCreateDTO = new PizzaCreateDTO() { Name = pizzaName, ToppingIds = new List<int>() { 1, 1 } };

        // Act
        var pizza = await _pizzaService.CreatePizzaAsync(pizzaCreateDTO);

        // Assert
        Assert.Equal(CommonErrors.CannotHaveDuplicateChildren.Code, pizza.Error.Code);
    }

    [Fact]
    public async Task UpdatePizzaAsync_ShouldUpdatePizza_WhenValid()
    {
        // Arrange
        var pizza = new Pizza { Id = 1, Name = "Margherita", Toppings = new List<Topping>() { new() { Id = 1, Name = "Pepperoni" } } };
        _mockPizzaRepository.Setup(repo => repo.GetByIdAsync(pizza.Id)).ReturnsAsync(pizza);
        var topping = new Topping { Id = 1, Name = "Pepperoni" };
        _mockToppingRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(topping);
        PizzaUpdateDTO pizzaUpdateDTO = new PizzaUpdateDTO { Id = pizza.Id, Name = "Pepperoni", ToppingIds = new List<int>() { 1 } };

        // Act
        var result = await _pizzaService.UpdatePizzaAsync(pizzaUpdateDTO);

        // Assert
        Assert.True(result.IsSuccess);
        _mockPizzaRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Pizza>()), Times.Once);
    }

    [Fact]
    public async Task UpdatePizzaAsync_ShouldReturnError_WhenUpdateToDuplicateName()
    {
        // Arrange
        var pizza = new Pizza { Id = 1, Name = "Margherita", Toppings = new List<Topping>() { new() { Id = 1, Name = "Pepperoni" } } };
        _mockPizzaRepository.Setup(repo => repo.GetByIdAsync(pizza.Id)).ReturnsAsync(pizza);
        string pizzaName = "Pepperoni";
        _mockPizzaRepository.Setup(repo => repo.GetByNameAsync(pizzaName))
                        .ReturnsAsync(new Pizza() { Name = pizzaName });
        var topping = new Topping { Id = 1, Name = "Pepperoni" };
        _mockToppingRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(topping);
        PizzaUpdateDTO pizzaUpdateDTO = new PizzaUpdateDTO { Id = pizza.Id, Name = "Pepperoni", ToppingIds = new List<int>() { 1 } };

        // Act
        var result = await _pizzaService.UpdatePizzaAsync(pizzaUpdateDTO);

        // Assert
        Assert.Equal(CommonErrors.NameCannotBeDuplicate.Code, result.Error.Code);
    }

    [Fact]
    public async Task UpdatePizzaAsync_ShouldReturnError_WhenZeroChildren()
    {
        // Arrange
        var pizza = new Pizza { Id = 1, Name = "Pepperoni", Toppings = new List<Topping>() { new() { Id = 1, Name = "Pepperoni" } } };
        _mockPizzaRepository.Setup(repo => repo.GetByIdAsync(pizza.Id)).ReturnsAsync(pizza);
        var topping = new Topping { Id = 1, Name = "Pepperoni" };
        _mockToppingRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(topping);
        PizzaUpdateDTO pizzaUpdateDTO = new PizzaUpdateDTO { Id = pizza.Id, Name = "Pepperoni", ToppingIds = new List<int>() { } };

        // Act
        var result = await _pizzaService.UpdatePizzaAsync(pizzaUpdateDTO);

        // Assert
        Assert.Equal(CommonErrors.MustHaveAtLeastOneChild.Code, result.Error.Code);
    }

    [Fact]
    public async Task UpdatePizzaAsync_ShouldReturnError_WhenDuplicateChildren()
    {
        // Arrange
        var pizza = new Pizza { Id = 1, Name = "Pepperoni", Toppings = new List<Topping>() { new() { Id = 1, Name = "Pepperoni" } } };
        _mockPizzaRepository.Setup(repo => repo.GetByIdAsync(pizza.Id)).ReturnsAsync(pizza);
        var topping = new Topping { Id = 1, Name = "Pepperoni" };
        _mockToppingRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(topping);
        PizzaUpdateDTO pizzaUpdateDTO = new PizzaUpdateDTO { Id = pizza.Id, Name = "Pepperoni", ToppingIds = new List<int>() { 1, 1 } };

        // Act
        var result = await _pizzaService.UpdatePizzaAsync(pizzaUpdateDTO);

        // Assert
        Assert.Equal(CommonErrors.CannotHaveDuplicateChildren.Code, result.Error.Code);
    }

    [Fact]
    public async Task UpdatePizzaAsync_ShouldReturnFailure_WhenNotExists()
    {
        // Arrange
        int pizzaId = 1;
        _mockPizzaRepository.Setup(repo => repo.GetByIdAsync(pizzaId)).ReturnsAsync((Pizza)null);
        PizzaUpdateDTO pizzaUpdateDTO = new PizzaUpdateDTO { Id = pizzaId, Name = "Pepperoni", ToppingIds = new List<int>() { 1 } };

        // Act
        var result = await _pizzaService.UpdatePizzaAsync(pizzaUpdateDTO);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(CommonErrors.ObjectCannotBeFound("Pizza").Code, result.Error.Code);
    }

    [Fact]
    public async Task DeletePizzaAsync_ShouldDeletePizza_WhenExists()
    {
        // Arrange
        var pizza = new Pizza { Id = 1, Name = "Margherita" };
        _mockPizzaRepository.Setup(repo => repo.GetByIdAsync(pizza.Id)).ReturnsAsync(pizza);

        // Act
        var result = await _pizzaService.DeletePizzaAsync(pizza.Id);

        // Assert
        Assert.True(result.IsSuccess);
        _mockPizzaRepository.Verify(repo => repo.DeleteAsync(It.IsAny<Pizza>()), Times.Once);
    }

    [Fact]
    public async Task DeletePizzaAsync_ShouldReturnFailure_WhenNotExists()
    {
        // Arrange
        int pizzaId = 1;
        _mockPizzaRepository.Setup(repo => repo.GetByIdAsync(pizzaId)).ReturnsAsync((Pizza)null);

        // Act
        var result = await _pizzaService.DeletePizzaAsync(pizzaId);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(CommonErrors.ObjectCannotBeFound("Pizza").Code, result.Error.Code);
    }
}
