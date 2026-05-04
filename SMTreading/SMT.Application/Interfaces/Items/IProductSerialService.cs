using Microsoft.AspNetCore.Mvc;
using SMT.Application.DTO.Items;
using SMT.Application.Helper;
using SMT.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMT.Application.Interfaces.Items
{
    public interface IProductSerialService
    {
        Task<List<ProductSerialDto>> GetAllAsync(); 

        Task<ProductSerialDto?> GetByIdAsync(long id); 
        Task<ProductSerialDto> CreateAsync(CreateProductSerialDto dto); 
        Task<long?> UpdateAsync(long id, UpdateProductSerialDto dto); 
        Task<bool> DeleteAsync(long id);
        Task<bool> UpdateProducSerialLinkedStatusWithImage([FromForm] UpdateProductSerialLinkedDto dto);
        Task<PagedResult<ProductSerialDto>> GetPagedAsync(int page, int pageSize, string? search);
        Task<bool> UnLinkedStatusAndRemoveLinkedImage(long id);
    }
}
