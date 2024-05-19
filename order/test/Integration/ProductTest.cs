using Application.Categories.Create;
using Application.Categories.Model;
using Application.Data;
using Application.Products.Create;
using Application.Products.Delete;
using Application.Products.GetAll;
using Application.Products.GetById;
using Application.Products.Model;
using Application.Products.Update;
using Domain.Categories.Error;
using Domain.Categories.Repository;
using Domain.Products.Entity;
using Domain.Products.Error;
using Domain.Products.Repository;
using Infra.Data;
using Infra.Repositories.Database;
using Infra.Repositories.Memory;
using Xunit;

namespace Integration;

public class ProductTest
{
    private readonly IProductRepository productRepository = new ProductRepositoryMemory();
    private readonly IUnitOfWork unitOfWork = new UnitOfWorkMemory();
    private readonly ICategoryRepository categoryRepository = new CategoryRepositoryMemory();


    [Fact]
    public async Task Should_Create_Product()
    {
        var inputCreateCategory = new CreateCategoryCommand("Category", "Category Description");

        var createCategoryCommandHandler = new CreateCatagoryCommandHandler(categoryRepository, unitOfWork);

        var outputCreateCategory = await createCategoryCommandHandler.Handle(inputCreateCategory, CancellationToken.None);

        var inputCreateProduct = new CreateProductCommand("Product 1", "Description Product 1", "BRL", 50.00, "Image", "0001", outputCreateCategory.Value);
       
        var createProductCommandHandler = new CreateProductCommandHandler(productRepository, categoryRepository, unitOfWork);

        var outputCreateProduct = await createProductCommandHandler.Handle(inputCreateProduct, CancellationToken.None);

        var getProductQueryHandler = new GetProductByIdQueryHandler(productRepository);

        var outputGetProduct = await getProductQueryHandler.Handle(new GetProductByIdQuery(outputCreateProduct.Value), CancellationToken.None);

        Assert.Equal("Product 1" ,outputGetProduct.Value.Name);
        Assert.Equal("Description Product 1", outputGetProduct.Value.Description);
        Assert.Equal(50.00, outputGetProduct.Value.Price);
    }

    [Fact]
    public async Task Should_List_All_Products()
    {
        var inputCreateCategory = new CreateCategoryCommand("Category", "Category Description");

        var createCategoryCommandHandler = new CreateCatagoryCommandHandler(categoryRepository, unitOfWork);

        var outputCreateCategory = await createCategoryCommandHandler.Handle(inputCreateCategory, CancellationToken.None);

        var inputCreateProduct1 = new CreateProductCommand("Product 1", "Description Product 1", "BRL", 50.00, "Image", "0001", outputCreateCategory.Value);
        var inputCreateProduct2 = new CreateProductCommand("Product 2", "Description Product 2", "BRL", 60.00, "Image", "0002", outputCreateCategory.Value);
        var inputCreateProduct3 = new CreateProductCommand("Product 3", "Description Product 3", "BRL", 70.00, "Image", "0003", outputCreateCategory.Value);

        var createProductCommandHandler = new CreateProductCommandHandler(productRepository, categoryRepository, unitOfWork);

        await createProductCommandHandler.Handle(inputCreateProduct1, CancellationToken.None);
        await createProductCommandHandler.Handle(inputCreateProduct2, CancellationToken.None);
        await createProductCommandHandler.Handle(inputCreateProduct3, CancellationToken.None);

        var getProductsQueryHandler = new GetAllProductsQueryHandler(productRepository);

        var outputGetProducts = await getProductsQueryHandler.Handle(new GetAllProductsQuery(), CancellationToken.None);

        Assert.Equal(3, outputGetProducts.Value.Count());
        Assert.Equal("Product 1", outputGetProducts.Value.ToList()[0].Name);
        Assert.Equal("Description Product 1", outputGetProducts.Value.ToList()[0].Description);
        Assert.Equal(50.00, outputGetProducts.Value.ToList()[0].Price);

        Assert.Equal("Product 2", outputGetProducts.Value.ToList()[1].Name);
        Assert.Equal("Description Product 2", outputGetProducts.Value.ToList()[1].Description);
        Assert.Equal(60.00, outputGetProducts.Value.ToList()[1].Price);

        Assert.Equal("Product 3", outputGetProducts.Value.ToList()[2].Name);
        Assert.Equal("Description Product 3", outputGetProducts.Value.ToList()[2].Description);
        Assert.Equal(70.00, outputGetProducts.Value.ToList()[2].Price);
    }

    [Fact]
    public async Task Should_Update_Product() 
    {
        var inputCreateCategory = new CreateCategoryCommand("Category", "Category Description");

        var createCategoryCommandHandler = new CreateCatagoryCommandHandler(categoryRepository, unitOfWork);

        var outputCreateCategory = await createCategoryCommandHandler.Handle(inputCreateCategory, CancellationToken.None);

        var inputCreateProduct = new CreateProductCommand("Product 1", "Description Product 1", "BRL", 50, "Image", "0001", outputCreateCategory.Value);

        var createProductCommandHandler = new CreateProductCommandHandler(productRepository, categoryRepository, unitOfWork);

        var outputCreateProduct = await createProductCommandHandler.Handle(inputCreateProduct, CancellationToken.None);

        var inputUpdateProduct = new UpdateProductCommand(outputCreateProduct.Value, "Nome Atualizado", "Descricao atualizando", "BRL", 90.0, "path", "sku", outputCreateCategory.Value);

        var updateProductCommandHandler = new UpdateProductCommandHandler(productRepository, categoryRepository ,unitOfWork);

        await updateProductCommandHandler.Handle(inputUpdateProduct, CancellationToken.None);

        var getProductQueryHandler = new GetProductByIdQueryHandler(productRepository);

        var outputGetProduct = await getProductQueryHandler.Handle(new GetProductByIdQuery(outputCreateProduct.Value), CancellationToken.None);

        Assert.Equal("Nome Atualizado", outputGetProduct.Value.Name);
        Assert.Equal("Descricao atualizando", outputGetProduct.Value.Description);
        Assert.Equal(90.00, outputGetProduct.Value.Price);
    }


    [Fact]
    public async Task Should_Delete_Product() 
    {
        var inputCreateCategory = new CreateCategoryCommand("Category", "Category Description");

        var createCategoryCommandHandler = new CreateCatagoryCommandHandler(categoryRepository, unitOfWork);

        var outputCreateCategory = await createCategoryCommandHandler.Handle(inputCreateCategory, CancellationToken.None);

        var inputCreateProduct = new CreateProductCommand("Product 1", "Description Product 1", "BRL", 50.00, "Image", "0001", outputCreateCategory.Value);

        var createProductCommandHandler = new CreateProductCommandHandler(productRepository, categoryRepository, unitOfWork);

        var outputCreateProduct = await createProductCommandHandler.Handle(inputCreateProduct, CancellationToken.None);
      
        var deleteOrderCommandHandler = new DeleteProductCommandHandler(productRepository, unitOfWork);

        await deleteOrderCommandHandler.Handle(new DeleteProductCommand(outputCreateProduct.Value), CancellationToken.None);
        
        var getProductQueryHandler = new GetProductByIdQueryHandler(productRepository);

        var outputGetProduct = await getProductQueryHandler.Handle(new GetProductByIdQuery(outputCreateProduct.Value), CancellationToken.None);

        Assert.False(outputGetProduct.IsSuccess);
        Assert.True(outputGetProduct.IsFailure);
        Assert.Equal(ProductErrors.ProductNotFound, outputGetProduct.Error);
    }
}
