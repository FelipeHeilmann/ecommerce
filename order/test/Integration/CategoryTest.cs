﻿using Application.Categories.Create;
using Application.Categories.GetAll;
using Application.Categories.GetById;
using Application.Categories.Update;
using Domain.Categories.Repository;
using Infra.Repositories.Memory;
using Xunit;

namespace Integration;

public class CategoryTest
{
    private ICategoryRepository categoryRepository;

    public CategoryTest() 
    {
        categoryRepository =  new CategoryRepositoryMemory();
    }

    [Fact]
    public async Task Should_Create_Category() 
    {
        var inputCreateCategory = new CreateCategoryCommand("minha categoria", "descricao da minha categoria");

        var createCategoryCommandHandler = new CreateCatagoryCommandHandler(categoryRepository);

        var outputCreateCategory = await createCategoryCommandHandler.Handle(inputCreateCategory, CancellationToken.None);

        var getCategoryByIdQueryHandler = new GetCategoryByIdQueryHandler(categoryRepository);

        var outputGetCategory = await getCategoryByIdQueryHandler.Handle(new GetCategoryByIdQuery(outputCreateCategory.Value), CancellationToken.None);

        Assert.Equal("minha categoria", outputGetCategory.Value.Name);
        Assert.Equal("descricao da minha categoria", outputGetCategory.Value.Descrption);
    }

    [Fact]
    public async Task Should_Get_All_Categories()
    {
        var inputCreateCategory = new CreateCategoryCommand("minha categoria", "descricao da minha categoria");

        var commandHandler = new CreateCatagoryCommandHandler(categoryRepository);

        await commandHandler.Handle(inputCreateCategory, CancellationToken.None);
        await commandHandler.Handle(inputCreateCategory, CancellationToken.None);
        await commandHandler.Handle(inputCreateCategory, CancellationToken.None);

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
        var inputCreateCategory = new CreateCategoryCommand("minha categoria", "descricao da minha categoria");

        var commandHandler = new CreateCatagoryCommandHandler(categoryRepository);

        var outputCreateCategory = await commandHandler.Handle(inputCreateCategory, CancellationToken.None);

        var inputUpdateCategory = new UpdateCategoryCommand("nome editado", "descricao editada", outputCreateCategory.Value);

        var updateCategoryCommandHandler = new UpdateCategoryCommandHandler(categoryRepository);

        await updateCategoryCommandHandler.Handle(inputUpdateCategory, CancellationToken.None);

        var getCategoryByIdQueryHandler = new GetCategoryByIdQueryHandler(categoryRepository);

        var outputGetCategory = await getCategoryByIdQueryHandler.Handle(new GetCategoryByIdQuery(outputCreateCategory.Value), CancellationToken.None);

        Assert.Equal("nome editado", outputGetCategory.Value.Name);
        Assert.Equal("descricao editada", outputGetCategory.Value.Descrption);
    }
}
