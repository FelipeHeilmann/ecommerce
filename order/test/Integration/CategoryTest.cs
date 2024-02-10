using Application.Categories.Create;
using Application.Categories.Model;
using Application.Data;
using Domain.Categories;
using Infra.Data;
using Infra.Repositories.Memory;
using Xunit;

namespace Integration;

public class CategoryTest
{
    private ICategoryRepository _categoryRepository = new CategoryRepositoryMemory();
    private IUnitOfWork _unitOfWork = new UnitOfWorkMemory();

    public CategoryTest() 
    {
        RepositorySetup.PopulateCategoryRepository(_categoryRepository);
    }

    [Fact]
    public async Task Should_Create_Category() 
    {
        var request = new CategoryModel("minha categoria", "descricao da minha categoria");

        var command = new CreateCategoryCommand(request);

        var commandHandler = new CreateCatagoryCommandHandler(_categoryRepository, _unitOfWork);

        var result = await commandHandler.Handle(command, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
    }
}
