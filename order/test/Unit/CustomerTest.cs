using Domain.Customers.Entity;
using Domain.Customers.Error;
using Domain.Customers.VO;
using Xunit;

namespace Unit;

public class CustomerTest
{
    [Fact]
    public void Should_Create_Valid_Name()
    {
        var name = new Name("Felipe Heilmann");
        Assert.Equal("Felipe Heilmann", name.Value);
    }

    [Fact]
    public void Should_Not_Create_Valid_Name_Due_Invalid_Format()
    {
        Assert.Throws<InvalidName>(() => new Name("Felipe"));
    }

    [Fact]
    public void Should_Create_Valid_Email()
    {
        var email = new Email("felipeheilmannm@gmail.com");
        Assert.Equal("felipeheilmannm@gmail.com", email.Value);
    }

    [Fact]
    public void Should_Not_Create_Valid_Email_Due_Invalid_Format()
    {
        Assert.Throws<InvalidEmail>(() => new Email("felipe.com"));
    }

    [Fact]
    public void Should_Create_Valid_CPF()
    {
        var cpf = new CPF("460.200.040-15");
        Assert.Equal("46020004015", cpf.Value);
    }

    [Fact]
    public void Should_Not_Create_Valid_CPF()
    {
        Assert.Throws<InvalidCPF>(() => new CPF("44444444444"));
    }

    [Fact]
    public void Should_Not_Create_Valid_CPF_Format()
    {
  
        Assert.Throws<InvalidCPF>(() => new CPF("873905240068"));
    }

    [Fact]
    public void Should_Create_Valid_Phone()
    {
        var phone = new Phone("11 97414-6507");

        Assert.Equal("11974146507", phone.Value);
    }

    [Fact]
    public void Should_Not_Create_Valid_Phone()
    {
        Assert.Throws<InvalidPhone>(() => new Phone("11 9741465070"));
    }


    [Fact]
    public void Should_Not_Create_Custumer_Due_Invalid_Age()
    {
        var name = "Felipe Heilmann";
        var email = "felipeheilmannm@gmail.com";
        var birthDate = new DateTime(2020, 6, 2);
        var cpf = "44444444444";
        var phone = "11 97414-6507";
        var password = "senha";

        Assert.Throws<UnderAge>(() => Customer.Create(name, email, password ,birthDate, cpf, phone));

    }
}
