using Humanizer;
using Microsoft.AspNetCore.Mvc;
using SMT.Application.DTO.Items;
using SMT.Application.Helper;
using SMT.Application.Interfaces.Items;
using SMT.Domain.Entities.Items;
using SMT.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMT.Infrastructure.Repositories.Items
{
    public class ProductSerialService(IProductSerialRepository repo, IProductRepository productRepo) : IProductSerialService
    {
        public async Task<List<ProductSerialDto>> GetAllAsync()
        {
            var list = await repo.GetAllWithProductsAsync();
            return list.Select(x => Map(x)).ToList();
        }

        public async Task<ProductSerialDto?> GetByIdAsync(long id)
        {
            var entity = await repo.GetByIdAsync(id);
            return entity is null ? null : Map(entity);
        }

        public async Task<ProductSerialDto> CreateAsync(CreateProductSerialDto dto)
        {
            var product = await productRepo.GetByIdAsync(dto.ProductId);
            if (product == null)
                throw new Exception("Product not found. Cannot generate serial.");
            string generatedSerial = await repo.GenerateUniqueSerialAsync(product.Model);

            var entity = new ProductSerial
            {
                SerialNumber = generatedSerial,
                ProductId = dto.ProductId,
                PurchaseCost = dto.PurchaseCost,
                SellingCost = dto.SellingCost,
                RentalCost = dto.RentalCost,
                Status = (ProductSerialStatus)dto.Status, //ProductSerialStatus.InStock,
                Product = default!
            };

            await repo.CreateAsync(entity);
            return Map(entity);
        }

        public async Task<long?> UpdateAsync(long id, UpdateProductSerialDto dto)
        {
            var entity = await repo.GetByIdAsync(id);
            if (entity is null) return null;

            entity.SerialNumber = dto.SerialNumber;
            entity.ProductId = dto.ProductId;
            entity.PurchaseCost = dto.PurchaseCost;
            entity.SellingCost = dto.SellingCost;
            entity.RentalCost = dto.RentalCost;
            entity.Status = Enum.Parse<ProductSerialStatus>(dto.Status);

            await repo.UpdateAsync(entity);
            return entity.Id;
        }

        public Task<bool> DeleteAsync(long id) => repo.DeleteAsync(id);

        private static ProductSerialDto Map(ProductSerial x)
            => new(x.Id, x.SerialNumber, x.ProductId, x.Product.Name, x.Product.Model, x.Product.Brand.Name, x.Status.ToString(), x.PurchaseCost, x.SellingCost, x.RentalCost, x.IsSerialNumberLinkToProduct, x.LinkedProductSerialNumberImageUrl);

        public async Task<bool> UpdateProducSerialLinkedStatusWithImage(
     [FromForm] UpdateProductSerialLinkedDto dto)
        {
            if (dto.File == null || dto.File.Length == 0)
                throw new Exception("File is required but was not provided.");

            var folderPath = Path.Combine("wwwroot/images/product-serial/");

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(dto.File.FileName)}";
            var fullPath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await dto.File.CopyToAsync(stream);
            }

            var imageUrl = $"/images/product-serial/{fileName}";

            return await repo.UpdateProducSerialLinkedStatusWithImage(dto.Id, imageUrl);
        }

        public Task<PagedResult<ProductSerialDto>> GetPagedAsync(int page, int pageSize, string? search)
        {
            return repo.GetPagedAsync(page, pageSize, search);
        }

        public async Task<bool> UnLinkedStatusAndRemoveLinkedImage(long id)
        {
            var entity = await repo.GetByIdAsync(id);
            if (entity == null) return false;

            if (!string.IsNullOrEmpty(entity.LinkedProductSerialNumberImageUrl))
            {
                var filePath = Path.Combine("wwwroot", entity.LinkedProductSerialNumberImageUrl.TrimStart('/'));
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }

            entity.IsSerialNumberLinkToProduct = false;
            entity.LinkedProductSerialNumberImageUrl = null;

            await repo.UpdateAsync(entity);
            return true;
        }
    }
}
