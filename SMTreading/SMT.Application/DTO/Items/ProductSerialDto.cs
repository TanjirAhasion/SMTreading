using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMT.Application.DTO.Items
{
    public record ProductSerialDto(long Id, string SerialNumber, long ProductId, string ProductName, string Model, string BrandName, string Status, decimal PurchaseCost, decimal SellingCost, decimal RentalCost, bool IsSerialNumberLinkToProduct, string? LinkedProductSerialNumberImageUrl);
    public record CreateProductSerialDto(string SerialNumber, long ProductId, decimal PurchaseCost, decimal SellingCost, decimal RentalCost, int Status);
    public record UpdateProductSerialDto(long Id, string SerialNumber, long ProductId, string Status, decimal PurchaseCost, decimal SellingCost, decimal RentalCost);
    
    public record UpdateProductSerialLinkedDto(long Id, string LinkedUrl, IFormFile File);

//    public record UpdateProductSerialLinkedDto(
//    bool IsSerialNumberLinkToProduct,
//    string linkedUrl,
//    int? imageId
//);
}
