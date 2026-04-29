using System;
using System.Collections.Generic;
using System.Text;

namespace SMT.Application.DTO.Items
{
    public record ProductDto(long Id, string Name, string? Model, long BrandId, string BrandName, decimal DefaultSalePrice, decimal DefaultRentPrice,int? LowStockThreshold,  bool IsActive);
    public record CreateProductDto(string Name, string? Model, long BrandId, decimal DefaultSalePrice, decimal DefaultRentPrice, int? LowStockThreshold, string? Description);
    public record UpdateProductDto(long Id, string Name, string? Model, long BrandId, decimal DefaultSalePrice, decimal DefaultRentPrice, int? LowStockThreshold, string? Description, bool IsActive);
}
