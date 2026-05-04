using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMT.Application.DTO.Items
{
    public record ProductImageDto(long Id, string Title, long ProductSerialId, string ImageUrl);
    public record ProductImageWithLinkedDto(long Id, string Title, long ProductSerialId, string ImageUrl, bool IsLinked);
    public record CreateProductImageDto(string Title, long ProductSerialId, IFormFile File);
    public record UpdateProductImageDto(long Id, string Title, long ProductSerialId);
}
