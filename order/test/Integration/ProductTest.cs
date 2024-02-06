using Application.Products.Command;
using Application.Products.Model;
using Infra.Repositories;
using Xunit;

namespace Integration;

public class ProductTest
{
    private readonly ProductRepositoryMemory _repository = new();

    [Fact]
    public async Task Should_Create_Project() 
    { 
        var catagoryId = Guid.Parse("de1ab44a-ef05-42da-a0e8-6137368018fc");
        var request = new CreateProductModel("Produto1", "Meu produto", "BRL", 70.0, "path", "sku", catagoryId);
        var command = new CreateProductCommand(request);
        var commandHandler = new CreateProductCommandHandler(_repository);

        var result = await commandHandler.Handle(command, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
    }
}
