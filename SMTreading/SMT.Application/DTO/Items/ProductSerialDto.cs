using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMT.Application.DTO.Items
{
    public record ProductSerialDto(long Id, string SerialNumber, long ProductId, string ProductName, string Model, string BrandName, string Status, decimal PurchaseCost, decimal SellingCost, decimal RentalCost, bool IsSerialNumberLinkToProduct, string? LinkedProductSerialNumberImageUrl);
    public record CreateProductSerialDto(string SerialNumber, string? LegacySerial, long ProductId, decimal PurchaseCost, decimal SellingCost, decimal RentalCost, int Status, bool IsOpeningStock, string? Note);
    public record UpdateProductSerialDto(long Id, string? LegacySerial, string Status, decimal PurchaseCost, decimal SellingCost, decimal RentalCost, string? Note);
    public record UpdateProductSerialLinkedDto(long Id, string LinkedUrl, IFormFile File);

//    public record UpdateProductSerialLinkedDto(
//    bool IsSerialNumberLinkToProduct,
//    string linkedUrl,
//    int? imageId
//);
}
