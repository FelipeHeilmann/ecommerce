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

        Assert.Equal("minha categoria", category.Name);
    }

    [Fact]
    public void Should_Not_Create_Category_Due_Name_Invalid()
    {
        Assert.Throws<InvalidCategoryName>(() => Category.Create("", "minha descricao"));
    }

    [Fact]
    public void Should_Not_Create_Category_Due_Null_Description()
    {
        Assert.Throws<InvalidCategoryName>(() => Category.Create("minha categoria", ""));
    }
}
