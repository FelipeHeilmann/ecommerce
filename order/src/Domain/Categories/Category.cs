﻿using Domain.Shared;

namespace Domain.Categories;

public class Category
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }

    public Category(Guid id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }   

    public static Result<Category> Create(string name, string description)
    {
        if(string.IsNullOrEmpty(name)) return Result.Failure<Category>(CategoryErrors.CategoryNameEmpty);
        if (string.IsNullOrEmpty(description)) return Result.Failure<Category>(CategoryErrors.CategoryDescriptionEmpty);
        return new Category(Guid.NewGuid(), name, description);
    }
    
    public void Update(string name, string description)
    {
        Name = name;
        Description = description;
    }
}
