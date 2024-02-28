using Domain.Customers;
using Xunit;

namespace Unit;

public class CustomerTest
{
    [Fact]
    public void Should_Create_Valid_Name()
    {
        var name = Name.Create("Felipe Heilmann");
        Assert.True(name.IsSuccess);
        Assert.False(name.IsFailure);
    }

    [Fact]
    public void Should_Not_Create_Valid_Name_Due_Invalid_Format()
    {
        var name = Name.Create("Felipe");
        Assert.False(name.IsSuccess);
        Assert.True(name.IsFailure);
    }

    [Fact]
    public void Should_Not_Create_Valid_Name_Due_Null()
    {
        var name = Name.Create(null);
        Assert.False(name.IsSuccess);
        Assert.True(name.IsFailure);
    }
    [Fact]
    public void Should_Create_Valid_Email()
    {
        var email = Email.Create("felipeheilmannm@gmail.com");
        Assert.True(email.IsSuccess);
        Assert.False(email.IsFailure);
    }

    [Fact]
    public void Should_Not_Create_Valid_Email_Due_Invalid_Format()
    {
        var email = Email.Create("felipe.com");
        Assert.False(email.IsSuccess);
        Assert.True(email.IsFailure);
    }

    [Fact]
    public void Should_Not_Create_Valid_Email_Due_Null()
    { 
        var email = Email.Create(null);
        Assert.False(email.IsSuccess);
        Assert.True(email.IsFailure);
    }

    [Fact]
    public void Should_Create_Valid_CPF()
    {
        var cpf = CPF.Create("44444444444");

        Assert.True(cpf.IsSuccess );
        Assert.False(cpf.IsFailure);
    }

    [Fact]
    public void Should_Not_Create_Valid_CPF_Due_Length()
    {
        var cpf = CPF.Create("4444444444");

        Assert.False(cpf.IsSuccess);
        Assert.True(cpf.IsFailure);
    }

    [Fact]
    public void Should_Not_Create_Valid_CPF_Format()
    {
        var cpf = CPF.Create("873.905.240-06");

        Assert.False(cpf.IsSuccess);
        Assert.True(cpf.IsFailure);
    }


    [Fact]
    public void Should_Create_Custumer()
    {
        var name = "Felipe Heilmann";
        var email = "felipeheilmannm@gmail.com";
        var birthDate = new DateOnly(2004, 6, 2);
        var cpf = "44444444444";
        var password = "senha";

        var custumer = Customer.Create(name, email, password, birthDate, cpf);

        Assert.True(custumer.IsSuccess);
        Assert.False(custumer.IsFailure);
    }

    [Fact]
    public void Should_Not_Create_Custumer_Due_Invalid_Age()
    {
        var name = "Felipe Heilmann";
        var email = "felipeheilmannm@gmail.com";
        var birthDate = new DateOnly(2020, 6, 2);
        var cpf = "44444444444";
        var password = "senha";

        var custumer = Customer.Create(name, email, password ,birthDate, cpf);

        Assert.False(custumer.IsSuccess);
        Assert.True(custumer.IsFailure);
        Assert.Equal(CustomerErrors.InvalidAge, custumer.Error);
    }
}
