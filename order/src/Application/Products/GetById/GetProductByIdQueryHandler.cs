﻿using Application.Abstractions.Messaging;
using Domain.Products;
using Domain.Shared;

namespace Application.Products.GetById
{
    public class GetProductByIdQueryHandler : IQueryHandler<GetProductByIdQuery, Product>
    {
        private readonly IProductRepository _repository;

        public GetProductByIdQueryHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<Product>> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
        {
            var product = await _repository.GetByIdAsync(query.ProductId, cancellationToken, "Category");

            if (product == null) return Result.Failure<Product>(ProductErrors.ProductNotFound);

            return Result.Success(product);
        }
    }
}
