using Microsoft.AspNetCore.Mvc;
using SMT.Application.DTO.Items;
using SMT.Application.Interfaces.Items;
using SMT.Domain.Entities.Items;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMT.Infrastructure.Repositories.Items
{
    public class ProductImageService(IProductImageRepository repo) : IProductImageService
    {
        public async Task<List<ProductImageDto>> GetAllAsync()
        {
            var list = await repo.GetAllAsync();
            return list.Select(x => Map(x)).ToList();
        }

        public async Task<ProductImageDto?> GetByIdAsync(long id)
        {
            var entity = await repo.GetByIdAsync(id);
            return entity is null ? null : Map(entity);
        }

        public async Task<ProductImageDto> CreateAsync([FromForm] CreateProductImageDto dto)
        {
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

            var entity = new ProductImage
            {
                ImageUrl = imageUrl,   // ✅ FIXED
                Title = dto.Title,
                ProductSerialId = dto.ProductSerialId,
                ProductSerial = default!
            };

            await repo.CreateAsync(entity);

            return Map(entity);
        }

        public async Task<ProductImageDto?> UpdateAsync(long id, UpdateProductImageDto dto)
        {
            var entity = await repo.GetByIdAsync(id);
            if (entity is null) return null;

            entity.Title = dto.Title;

            await repo.UpdateAsync(entity);
            return Map(entity);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            // 1. Fetch the image record from the database
            var image = await repo.GetByIdAsync(id);
            if (image == null) return false;

            // 2. Construct the full physical path
            // image.ImageUrl is likely "/images/product-serial/..."
            // We trim the leading slash to combine it correctly with wwwroot
            var relativePath = image.ImageUrl.TrimStart('/');
            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", relativePath);

            try
            {
                // 3. Delete the file from the physical storage
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
            }
            catch (Exception ex)
            {
                // Log the error but perhaps continue with DB deletion 
                // or throw depending on your business logic
                Console.WriteLine($"File deletion failed: {ex.Message}");
            }

            // 4. Delete the record from the database
            return await repo.DeleteAsync(id);
        }

        private static ProductImageDto Map(ProductImage x)
            => new(x.Id, x.Title, x.ProductSerialId, x.ImageUrl);

        public async Task<List<ProductImage>> GetBySerialIdAsync(long serialId)
        {
            return await repo.GetBySerialIdAsync(serialId);
        }
    }
}
