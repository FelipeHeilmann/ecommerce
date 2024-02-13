using Domain.Address;
using Domain.Categories;
using Domain.Customer;
using Domain.Orders;
using Domain.Products;

namespace Integration;

public class RepositorySetup
{
    public static void PopulateProductRepository(IProductRepository _repository)
    {
        _repository.Add(new Product(
                Guid.Parse("55b86726-d9fb-4745-b64a-66923b584cf2"), //Id
                "Nome do produto", //Name
                "Desricao", //Description
                "Imagem", //Image,
                DateTime.Now, //CreatedAt
                new Money("BRL", 50.00), //Price 
                Sku.Create("0001").Value, //Sku
                Guid.Parse("a7efe841-8e19-4fe8-afcd-e3742a3dacf4"),//CategoryId
                new Category(Guid.Parse("a7efe841-8e19-4fe8-afcd-e3742a3dacf4"), //Category 
                "categoria nome", 
                "categoria descricao")
            ));
        _repository.Add(new Product(
                Guid.Parse("88a98a97-882d-4194-a03f-0e804bea5ff5"), 
                "Nome do produto", 
                "Desricao", 
                "Imagem",
                DateTime.Now,
                new Money("BRL", 50.00), 
                Sku.Create("0001").Value, 
                Guid.Parse("a7efe841-8e19-4fe8-afcd-e3742a3dacf4"),
                new Category(Guid.Parse("a7efe841-8e19-4fe8-afcd-e3742a3dacf4"), 
                "categoria nome", "categoria descricao")
            ));
        _repository.Add(new Product(
                Guid.Parse("d8872746-afce-471b-a0d8-3f2fd05eba87"),
                "Nome do produto",
                "Desricao",
                "Imagem",
                DateTime.Now,
                new Money("BRL", 50.00),
                Sku.Create("0001").Value, 
                Guid.Parse("a7efe841-8e19-4fe8-afcd-e3742a3dacf4"), 
                new Category(Guid.Parse("a7efe841-8e19-4fe8-afcd-e3742a3dacf4"), 
                "categoria nome", "categoria descricao")
            ));
        _repository.Add(new Product(
                Guid.Parse("c65b5fab-018b-4471-a5a9-cd09af34b48c"), 
                "Nome do produto", 
                "Desricao", 
                "Imagem", 
                DateTime.Now,
                new Money("BRL", 50.00),
                Sku.Create("0001").Value, 
                Guid.Parse("a7efe841-8e19-4fe8-afcd-e3742a3dacf4"),
                new Category(Guid.Parse("a7efe841-8e19-4fe8-afcd-e3742a3dacf4"), 
                "categoria nome", "categoria descricao")
            ));

        _repository.Add(new Product(
                Guid.Parse("55b86726-d9fb-4745-b64a-66923b584cf2"), 
                "Nome do produto", 
                "Desricao", 
                "Imagem",
                DateTime.Now,
                new Money("BRL", 50.00), 
                Sku.Create("0001").Value,
                Guid.Parse("a7efe841-8e19-4fe8-afcd-e3742a3dacf4"), 
                new Category(Guid.Parse("a7efe841-8e19-4fe8-afcd-e3742a3dacf4"),
                "categoria nome", "categoria descricao")
            ));
        _repository.Add(new Product(
                Guid.Parse("6021dc49-a9f5-43bb-9602-c1689c5549e3"), 
                "Nome do produto", 
                "Desricao", 
                "Imagem", 
                DateTime.Now,
                new Money("BRL", 70.00), 
                Sku.Create("0002").Value,
                Guid.Parse("538f1d08-9f31-4058-9538-55f388cde724"), 
                new Category(Guid.Parse("538f1d08-9f31-4058-9538-55f388cde724"), 
                "categoria nome", "categoria descricao")
            ));
        _repository.Add(new Product(
                Guid.Parse("cb67d960-af04-40c8-92da-9d4ff28da6f8"), 
                "Nome do produto", 
                "Desricao", 
                "Imagem",
                DateTime.Now,
                new Money("BRL", 60.00), 
                Sku.Create("0003").Value, 
                Guid.Parse("f3b205c3-552d-4fd9-b10e-6414086910b0"), 
                new Category(Guid.Parse("f3b205c3-552d-4fd9-b10e-6414086910b0"), 
                "categoria nome", 
                "categoria descricao")
            ));

        _repository.Add(new Product
            (Guid.Parse("9d9d284f-6b19-4b34-9e29-45c6d8f45bfa"), 
            "Nome do produto", 
            "Desricao", 
            "Imagem",
            DateTime.Now,
            new Money("BRL", 60.00), 
            Sku.Create("0003").Value, 
            Guid.Parse("f3b205c3-552d-4fd9-b10e-6414086910b0"), 
            new Category(Guid.Parse("f3b205c3-552d-4fd9-b10e-6414086910b0"), 
            "categoria nome", 
            "categoria descricao")
            ));
        _repository.Add(new Product(
                Guid.Parse("20caccf1-6a2b-40fb-b9da-1efbca3f734f"), 
                "Nome do produto", 
                "Desricao", 
                "Imagem", 
                DateTime.Now,
                new Money("BRL", 100.00), 
                Sku.Create("0003").Value, 
                Guid.Parse("f3b205c3-552d-4fd9-b10e-6414086910b0"), 
                new Category(Guid.Parse("f3b205c3-552d-4fd9-b10e-6414086910b0"), 
                "categoria nome", "categoria descricao")
            ));
    }

    public static void PopulateCategoryRepository(ICategoryRepository _repository)
    {
        _repository.Add(new Category(
                Guid.Parse("a7efe841-8e19-4fe8-afcd-e3742a3dacf4"),
                "categoria nome", 
                "categoria descricao"
            ));

        _repository.Add(new Category(
                Guid.Parse("de1ab44a-ef05-42da-a0e8-6137368018fc"),
                "categoria nome", 
                "categoria descricao"
            ));
    }

    public static void PopulateCustomerRepository(ICustomerRepository _repository)
    {
        _repository.Add(new Customer(
                Guid.Parse("f3b205c3-552d-4fd9-b10e-6414086910b0"), 
                Name.Create("Felipe Heilmann").Value,
                Email.Create("felipeheilmannm@gmail.com").Value, 
                "senha", 
                new DateOnly(2004, 6, 11), 
                DateTime.Now)
            );
    }

    public static void PopulateOrderRepository(IOrderRepository _repository)
    {
        var order = new Order(
            Guid.Parse("c3a9083c-a259-4516-8842-a80b40f8c39f"), 
            Guid.Parse("f3b205c3-552d-4fd9-b10e-6414086910b0"), 
            OrderStatus.Created, 
            DateTime.Now, 
            DateTime.Now,
            null,
            null
        );

        var order2 = new Order(
            Guid.Parse("8f34a311-f1cd-40b6-9e5d-1b9f639369e9"),
            Guid.Parse("f3b205c3-552d-4fd9-b10e-6414086910b0"),
            OrderStatus.Created,
            DateTime.Now,
            DateTime.Now,
            null,
            null
        );

        var lineItens = new List<LineItem>()
        {
            new LineItem(
                Guid.Parse("efd7d188-b573-46ba-aa2f-6fd139d1813a"), 
                Guid.Parse("c3a9083c-a259-4516-8842-a80b40f8c39f"), 
                Guid.Parse("9d9d284f-6b19-4b34-9e29-45c6d8f45bfa"), 
                new Money("BRL", 60.00), 
                2
            ),
            new LineItem(
                Guid.Parse("27046ada-aee4-4325-8bc5-2affe5cf9627"), 
                Guid.Parse("c3a9083c-a259-4516-8842-a80b40f8c39f"),
                Guid.Parse("20caccf1-6a2b-40fb-b9da-1efbca3f734f"), 
                new Money("BRL", 100.00),
                1
           )
        };

        order.RestoreLineItens(lineItens);
        order2.RestoreLineItens(lineItens);

        _repository.Add(order);
        _repository.Add(order2);
    }
    
    public static void PopulateAddressRepository(IAddressRepository _repository)
    {
        _repository.Add(new Address(
            Guid.Parse("2b169c76-acee-4ddf-86c4-37af9fbb07ea"),
            Guid.Parse("f3b205c3-552d-4fd9-b10e-6414086910b0"),
            ZipCode.Create("04182-135").Value,
            "rua",
            "bairro",
            "numero",
            null,
            "cidade",
            "estado",
            "pais")
        );

        _repository.Add(new Address(
            Guid.Parse("fa2c85e2-3778-43d5-b3c7-89d1e5bc155d"),
            Guid.Parse("f3b205c3-552d-4fd9-b10e-6414086910b0"),
            ZipCode.Create("04182-135").Value,
            "rua 2",
            "bairro 2",
            "numero 2",
            null,
            "cidade 2",
            "estado 2",
            "pais 2")
        );

    }
}
