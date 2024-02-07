using Application.Products.Command;
using Application.Products.Model;
using Application.Products.Query;
using Domain.Products;
using Infra.Repositories.Memory;
using Xunit;

namespace Integration;

public class ProductTest
{
    private readonly ProductRepositoryMemory _repository = new();

    public ProductTest() 
    {
        _repository.Add(new Product(Guid.Parse("55b86726-d9fb-4745-b64a-66923b584cf2"), "Nome do produto", "Desricao", "Imagem", new Money("BRL", 50.00), Sku.Create("0001"), Guid.Parse("a7efe841-8e19-4fe8-afcd-e3742a3dacf4"), new Category(Guid.Parse("a7efe841-8e19-4fe8-afcd-e3742a3dacf4"), "categoria nome", "categoria descricao")));
    }
    [Fact]
    public async Task Should_Create_Product() 
    { 
        var catagoryId = Guid.Parse("de1ab44a-ef05-42da-a0e8-6137368018fc");
        var request = new CreateProductModel("Produto1", "Meu produto", "BRL", 70.0, "path", "sku", catagoryId);
        var command = new CreateProductCommand(request);
        var commandHandler = new CreateProductCommandHandler(_repository);

        var result = await commandHandler.Handle(command, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
    }

    [Fact]
    public async Task Should_Get_Product_By_Id()
    {
        var productId = Guid.Parse("55b86726-d9fb-4745-b64a-66923b584cf2");
        var query = new GetProductByIdQuery(productId);
        var queryHandler = new GetProductByIdQueryHandler(_repository);

        var result = await queryHandler.Handle(query, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.NotNull(result.Data);
    }

    [Fact]
    public async Task Should_Not_Get_Product_By_Id()
    {
        var productId = Guid.Parse("ee5ad79f-7593-4e23-9968-4380a545ee35");
        var query = new GetProductByIdQuery(productId);
        var queryHandler = new GetProductByIdQueryHandler(_repository);

        var result = await queryHandler.Handle(query, CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
    }
}
