using Domain.Customer;
using Xunit;

namespace Unit;

public class CustomerTest
{
    [Fact]
    public void Should_Create_Custumer()
    {
        var name = "Felipe Heilmann";
        var email = "felipeheilmannm@gmail.com";
        var birthDate = new DateTime(2004, 6, 2);
        var password = "senha";

        var custumer = Customer.Create(name, email, password, birthDate);

        Assert.True(custumer.IsSuccess);
        Assert.False(custumer.IsFailure);
    }

    [Fact]
    public void Should_Not_Create_Custumer_Due_Email_Null()
    {
        var name = "Felipe Heilmann";
        var email = "felipeheilmannm@gmail.com";
        var birthDate = new DateTime(2004, 6, 2);
        var password = "senha";

        var custumer = Customer.Create(null, email, password, birthDate);

        Assert.False(custumer.IsSuccess);
        Assert.True(custumer.IsFailure);
        Assert.Equal(CustomerErrors.NameNull, custumer.Error);
    }

    [Fact]
    public void Should_Not_Create_Custumer_Due_Name_Null()
    {
        var name = "Felipe Heilmann";
        var email = "felipeheilmannm@gmail.com";
        var birthDate = new DateTime(2004, 6, 2);
        var password = "senha";

        var custumer = Customer.Create(null, email,password,birthDate);

        Assert.False(custumer.IsSuccess);
        Assert.True(custumer.IsFailure);
        Assert.Equal(CustomerErrors.NameNull, custumer.Error);
    }

    [Fact]
    public void Should_Not_Create_Custumer_Due_Invalid_Name()
    {
        var name = "Felipe";
        var email = "felipeheilmannm@gmail.com";
        var birthDate = new DateTime(2004, 6, 2);
        var password = "senha";

        var custumer = Customer.Create(name, email, password ,birthDate);

        Assert.False(custumer.IsSuccess);
        Assert.True(custumer.IsFailure);
        Assert.Equal(CustomerErrors.NameFormat, custumer.Error);
    }

    [Fact]
    public void Should_Not_Create_Custumer_Due_Invalid_Email()
    {
        var name = "Felipe Heilmann";
        var email = "felipe.com";
        var birthDate = new DateTime(2004, 6, 2);
        var password = "senha";

        var custumer = Customer.Create(name, email, password ,birthDate);

        Assert.False(custumer.IsSuccess);
        Assert.True(custumer.IsFailure);
        Assert.Equal(CustomerErrors.EmailFormat, custumer.Error);
    }

    [Fact]
    public void Should_Not_Create_Custumer_Due_Invalid_Age()
    {
        var name = "Felipe Heilmann";
        var email = "felipeheilmannm@gmail.com";
        var birthDate = new DateTime(2020, 6, 2);
        var password = "senha";

        var custumer = Customer.Create(name, email, password ,birthDate);

        Assert.False(custumer.IsSuccess);
        Assert.True(custumer.IsFailure);
        Assert.Equal(CustomerErrors.InvalidAge, custumer.Error);
    }

}
