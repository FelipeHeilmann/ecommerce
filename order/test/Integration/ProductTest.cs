using Application.Products.Command;
using Application.Products.Model;
using Application.Products.Query;
using Domain.Products;
using Domain.Shared;
using Infra.Data;
using Infra.Repositories.Memory;
using Xunit;

namespace Integration;

public class ProductTest
{
    private readonly ProductRepositoryMemory _repository = new();
    private readonly UnitOfWorkMemory _unitOfWork =  new();

    public ProductTest() 
    {
        _repository.Add(new Product(Guid.Parse("55b86726-d9fb-4745-b64a-66923b584cf2"), "Nome do produto", "Desricao", "Imagem", new Money("BRL", 50.00), Sku.Create("0001"), Guid.Parse("a7efe841-8e19-4fe8-afcd-e3742a3dacf4"), new Category(Guid.Parse("a7efe841-8e19-4fe8-afcd-e3742a3dacf4"), "categoria nome", "categoria descricao")));
        _repository.Add(new Product(Guid.Parse("88a98a97-882d-4194-a03f-0e804bea5ff5"), "Nome do produto", "Desricao", "Imagem", new Money("BRL", 50.00), Sku.Create("0001"), Guid.Parse("a7efe841-8e19-4fe8-afcd-e3742a3dacf4"), new Category(Guid.Parse("a7efe841-8e19-4fe8-afcd-e3742a3dacf4"), "categoria nome", "categoria descricao")));
        _repository.Add(new Product(Guid.Parse("d8872746-afce-471b-a0d8-3f2fd05eba87"), "Nome do produto", "Desricao", "Imagem", new Money("BRL", 50.00), Sku.Create("0001"), Guid.Parse("a7efe841-8e19-4fe8-afcd-e3742a3dacf4"), new Category(Guid.Parse("a7efe841-8e19-4fe8-afcd-e3742a3dacf4"), "categoria nome", "categoria descricao")));
        _repository.Add(new Product(Guid.Parse("c65b5fab-018b-4471-a5a9-cd09af34b48c"), "Nome do produto", "Desricao", "Imagem", new Money("BRL", 50.00), Sku.Create("0001"), Guid.Parse("a7efe841-8e19-4fe8-afcd-e3742a3dacf4"), new Category(Guid.Parse("a7efe841-8e19-4fe8-afcd-e3742a3dacf4"), "categoria nome", "categoria descricao")));
    }

    [Fact]
    public async Task Should_Create_Product() 
    { 
        var catagoryId = Guid.Parse("de1ab44a-ef05-42da-a0e8-6137368018fc");
        var request = new CreateProductModel("Produto1", "Meu produto", "BRL", 70.0, "path", "sku", catagoryId);
        var command = new CreateProductCommand(request);
        var commandHandler = new CreateProductCommandHandler(_repository, _unitOfWork);

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

    [Fact]
    public async Task Should_Update_Product() 
    {
        var catagoryId = Guid.Parse("de1ab44a-ef05-42da-a0e8-6137368018fc");
        var productId = Guid.Parse("d8872746-afce-471b-a0d8-3f2fd05eba87");
        var request = new UpdateProductModel(productId, "Nome Atualizado", "Descricap atualizando", "BRL", 90.0, "path", "sku", catagoryId);
        var command = new UpdateProductCommand(request);
        var commandHandler = new UpdateProductCommandHandler(_repository, _unitOfWork);

        var result = await commandHandler.Handle(command, CancellationToken.None);

        var product = result.Data;

        Assert.True(result.IsSuccess);
        Assert.Equal(90.0, product.Price.Amount);
    }

    [Fact]
    public async Task Should_Not_Update_Product_Due_Not_Found_Product()
    {
        var catagoryId = Guid.Parse("de1ab44a-ef05-42da-a0e8-6137368018fc");
        var productId = Guid.Parse("79f792d3-a213-4acc-8f78-266c1b666a56");
        var request = new UpdateProductModel(productId, "Nome Atualizado", "Descricap atualizando", "BRL", 90.0, "path", "sku", catagoryId);
        var command = new UpdateProductCommand(request);
        var commandHandler = new UpdateProductCommandHandler(_repository, _unitOfWork);

        var result = await commandHandler.Handle(command, CancellationToken.None);


        Assert.False (result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal(ProductErrors.ProductNotFound, result.Error);
    }

    [Fact]
    public async Task Should_Not_Update_Product_Due_Not_Found_Category()
    {
        var catagoryId = Guid.Parse("79f792d3-a213-4acc-8f78-266c1b666a56");
        var productId = Guid.Parse("d8872746-afce-471b-a0d8-3f2fd05eba87");
        var request = new UpdateProductModel(productId, "Nome Atualizado", "Descricap atualizando", "BRL", 90.0, "path", "sku", catagoryId);
        var command = new UpdateProductCommand(request);
        var commandHandler = new UpdateProductCommandHandler(_repository, _unitOfWork);

        var result = await commandHandler.Handle(command, CancellationToken.None);


        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal(ProductErrors.CategoryNotFound, result.Error);
    }

    [Fact]
    public async Task Should_Delete_Product() 
    {
        var productId = Guid.Parse("c65b5fab-018b-4471-a5a9-cd09af34b48c");
        var command = new DeleteProductCommand(productId);
        var commandHandler = new DeleteProductCommandHandler(_repository, _unitOfWork);

        var result = await commandHandler.Handle(command, CancellationToken.None);

        var query = new GetProductByIdQuery(productId);
        var queryHandler = new GetProductByIdQueryHandler(_repository);

        var resultQuery = await queryHandler.Handle(query, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);

        Assert.False(resultQuery.IsSuccess);
        Assert.True(resultQuery.IsFailure);
        Assert.Equal(ProductErrors.ProductNotFound, resultQuery.Error);
    }

    [Fact]
    public async Task Should_Get_Produc_List() 
    {
        var query = new GetAllProductsQuery();

        var queryHandler = new GetAllProductsQueryHandler(_repository);

        var result = await queryHandler.Handle(query, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.True(result.Data.Count > 1);
    }
}
