using Microsoft.AspNetCore.Mvc;
using SMT.Application.DTO.Items;
using SMT.Application.Interfaces.Items;
using SMT.Domain.Entities.Items;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMT.Infrastructure.Repositories.Items
{
    public class ProductImageService(IProductSerialRepository productSerialRepo, IProductImageRepository repo) : IProductImageService
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

        public async Task<List<ProductImageWithLinkedDto>> GetBySerialIdAsync(long serialId)
        {
            // Fetch the parent entity to check the linkage flag and URL
            var entities = await productSerialRepo.GetByIdAsync(serialId);

            var result = new List<ProductImageWithLinkedDto>();

            // 1. Determine if a linked machine image exists
            bool hasLinkedImage = entities != null &&
                                  entities.IsSerialNumberLinkToProduct &&
                                  !string.IsNullOrEmpty(entities.LinkedProductSerialNumberImageUrl);

            // 2. If the serial is linked and has an image, add the primary linked image as the first item
            if (hasLinkedImage)
            {
                result.Add(new ProductImageWithLinkedDto(
                    entities.Id,
                    "Linked Machine Image",
                    serialId,
                    entities.LinkedProductSerialNumberImageUrl,
                    IsLinked: true
                ));
            }

            // 3. Retrieve additional images from the repository
            var additionalImages = await repo.GetBySerialIdAsync(serialId);

            // 4. Map additional images and mark them with IsLinked = false
            result.AddRange(additionalImages.Select(x => new ProductImageWithLinkedDto(
                x.Id,
                x.Title,
                x.ProductSerialId,
                x.ImageUrl,
                IsLinked: false
            )));

            return result;
        }

        private static ProductImageDto Map(ProductImage x)=> new(x.Id, x.Title, x.ProductSerialId, x.ImageUrl);

        private static ProductImageWithLinkedDto MapWithLinked(ProductImage x, ProductImageDto linkImageDto, bool isLinked = false)
     => new(x.Id, x.Title, x.ProductSerialId, x.ImageUrl, isLinked);

    }
}
