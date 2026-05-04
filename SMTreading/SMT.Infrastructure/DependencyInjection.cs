using Microsoft.Extensions.DependencyInjection;
using SMT.Application.Auth.Interfaces;
using SMT.Application.Common;
using SMT.Application.Interfaces.Accounts;
using SMT.Application.Interfaces.Contacts;
using SMT.Application.Interfaces.Inventory;
using SMT.Application.Interfaces.Items;
using SMT.Infrastructure.Auth.Services;
using SMT.Infrastructure.Repositories.Accounts;
using SMT.Infrastructure.Repositories.Contacts;
using SMT.Infrastructure.Repositories.Inventory;
using SMT.Infrastructure.Repositories.Items;
using System;
using System.Collections.Generic;
using System.Text;


namespace SMT.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IPasswordHasher, BcryptPasswordHasher>();

            services.AddScoped<IBrandRepository, BrandRepository>();
            services.AddScoped<IBrandService, BrandService>();

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductService, ProductService>();

            services.AddScoped<IProductSerialRepository, ProductSerialRepository>();
            services.AddScoped<IProductSerialService, ProductSerialService>();

            services.AddScoped<IProductImageRepository, ProductImageRepository>();
            services.AddScoped<IProductImageService, ProductImageService>();

            services.AddScoped<IVendorRepository, VendorRepository>();
            services.AddScoped<IVendorService, VendorService>();
            services.AddScoped<IVendorPaymentRepository, VendorPaymentRepository>();
            services.AddScoped<IVendorPaymentService, VendorPaymentService>();
            services.AddScoped<IVendorLedgerRepository, VendorLedgerRepository>();
            services.AddScoped<IVendorLedgerService, VendorLedgerService>();

            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ICustomerService, CustomerService>();

            services.AddScoped<IPurchaseRepository, PurchaseRepository>();
            services.AddScoped<IPurchaseItemRepository, PurchaseItemRepository>();
            services.AddScoped<IPurchaseItemProductSerialRepository, PurchaseItemProductSerialRepository>();
            services.AddScoped<IPurchaseService, PurchaseService>();
            //services.AddScoped<ISaleRepository, SaleRepository>();
            return services;
        }

    }
}
