using Application.Categories.Create;
using Application.Categories.GetAll;
using Application.Categories.GetById;
using Application.Categories.Model;
using Application.Categories.Update;
using Application.Data;
using Domain.Categories.Entity;
using Domain.Categories.Repository;
using Infra.Data;
using Infra.Repositories.Memory;
using Xunit;

namespace Integration;

public class CategoryTest
{
    private ICategoryRepository categoryRepository = new CategoryRepositoryMemory();
    private IUnitOfWork unitOfWork = new UnitOfWorkMemory();

    public CategoryTest() 
    {
    }

    [Fact]
    public async Task Should_Create_Category() 
    {
        var inputCreateCategory = new CreateCategoryRequest("minha categoria", "descricao da minha categoria");

        var createCategoryCommandHandler = new CreateCatagoryCommandHandler(categoryRepository, unitOfWork);

        var outputCreateCategory = await createCategoryCommandHandler.Handle(new CreateCategoryCommand(inputCreateCategory), CancellationToken.None);

        var getCategoryByIdQueryHandler = new GetCategoryByIdQueryHandler(categoryRepository);

        var outputGetCategory = await getCategoryByIdQueryHandler.Handle(new GetCategoryByIdQuery(outputCreateCategory.Value), CancellationToken.None);

        Assert.Equal("minha categoria", outputGetCategory.Value.Name);
        Assert.Equal("descricao da minha categoria", outputGetCategory.Value.Descrption);
    }

    [Fact]
    public async Task Should_Get_All_Categories()
    {
        var inputCreateCategory = new CreateCategoryRequest("minha categoria", "descricao da minha categoria");

        var commandHandler = new CreateCatagoryCommandHandler(categoryRepository, unitOfWork);

        await commandHandler.Handle(new CreateCategoryCommand(inputCreateCategory), CancellationToken.None);
        await commandHandler.Handle(new CreateCategoryCommand(inputCreateCategory), CancellationToken.None);
        await commandHandler.Handle(new CreateCategoryCommand(inputCreateCategory), CancellationToken.None);

        var query = new GetAllCategoriesQuery();

        var queryHandler = new GetAllCategoriesQueryHandler(categoryRepository);

        var result = await queryHandler.Handle(query, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Equal(3, result.Value.Count());
    }


    [Fact]
    public async Task Should_Update_Category()
    {
        var inputCreateCategory = new CreateCategoryRequest("minha categoria", "descricao da minha categoria");

        var commandHandler = new CreateCatagoryCommandHandler(categoryRepository, unitOfWork);

        var outputCreateCategory = await commandHandler.Handle(new CreateCategoryCommand(inputCreateCategory), CancellationToken.None);

        var inputUpdateCategory = new UpdateCategoryRequest("nome editado", "descricao editada", outputCreateCategory.Value);

        var updateCategoryCommandHandler = new UpdateCategoryCommandHandler(categoryRepository, unitOfWork);

        await updateCategoryCommandHandler.Handle(new UpdateCategoryCommand(inputUpdateCategory), CancellationToken.None);

        var getCategoryByIdQueryHandler = new GetCategoryByIdQueryHandler(categoryRepository);

        var outputGetCategory = await getCategoryByIdQueryHandler.Handle(new GetCategoryByIdQuery(outputCreateCategory.Value), CancellationToken.None);

        Assert.Equal("nome editado", outputGetCategory.Value.Name);
        Assert.Equal("descricao editada", outputGetCategory.Value.Descrption);
    }
}
