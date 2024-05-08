using Domain.Categories.Entity;
using Domain.Categories.Error;
using Xunit;

namespace Unit;

public class CategoryTest
{
    [Fact]
    public void Should_Create_Category()
    {
        var category = Category.Create("minha categoria", "minha descricao");

        Assert.True(category.IsSuccess);
        Assert.False(category.IsFailure);
    }

    [Fact]
    public void Should_Not_Create_Category_Due_Null_Name()
    {
        var category = Category.Create(null, "minha descricao");

        Assert.False(category.IsSuccess);
        Assert.True(category.IsFailure);
        Assert.Equal(CategoryErrors.CategoryNameEmpty, category.Error);
    }

    [Fact]
    public void Should_Not_Create_Category_Due_Null_Description()
    {
        var category = Category.Create("minha categoria", null);

        Assert.False(category.IsSuccess);
        Assert.True(category.IsFailure);
        Assert.Equal(CategoryErrors.CategoryDescriptionEmpty, category.Error);
    }
}
