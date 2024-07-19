namespace API.Requests;

public record CreateCategoryRequest(string Name, string Description);

public record UpdateCategoryRequest(string Name, string Description, Guid CustomerId);