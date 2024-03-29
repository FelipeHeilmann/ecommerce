﻿using Application.Categories.Create;
using Application.Categories.GetAll;
using Application.Categories.GetById;
using Application.Categories.Model;
using Application.Categories.Update;
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
        var request = new CreateCategoryRequest("minha categoria", "descricao da minha categoria");

        var command = new CreateCategoryCommand(request);

        var commandHandler = new CreateCatagoryCommandHandler(_categoryRepository, _unitOfWork);

        var result = await commandHandler.Handle(command, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
    }

    [Fact]
    public async Task Should_Get_All_Categories()
    {
        var query = new GetAllCategoriesQuery();

        var queryHandler = new GetAllCategoriesQueryHandler(_categoryRepository, _unitOfWork);

        var result = await queryHandler.Handle(query, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.True(result.Value is List<Category>);
    }

    [Fact]
    public async Task Should_Get_Category_By_Id()
    {
        var categoryId = Guid.Parse("de1ab44a-ef05-42da-a0e8-6137368018fc");

        var query = new GetCategoryByIdQuery(categoryId);

        var queryHandler = new GetCategoryByIdQueryHandler(_categoryRepository, _unitOfWork);

        var result = await queryHandler.Handle(query, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
    }

    [Fact]
    public async Task Should_Update_Category()
    {
        var categoryId = Guid.Parse("de1ab44a-ef05-42da-a0e8-6137368018fc");

        var request = new UpdateCategoryRequest("nome editado", "descricao editada", categoryId);

        var command = new UpdateCategoryCommand(request);

        var commandHandler = new UpdateCategoryCommandHandler(_categoryRepository, _unitOfWork);

        var result = await commandHandler.Handle(command, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
    }
}
