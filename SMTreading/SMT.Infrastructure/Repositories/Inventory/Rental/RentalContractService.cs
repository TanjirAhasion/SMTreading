using SMT.Application.DTO.Accounts;
using SMT.Application.DTO.CashManagement;
using SMT.Application.DTO.Inventory;
using SMT.Application.Helper;
using SMT.Application.Interfaces.Accounts;
using SMT.Application.Interfaces.CashManagement;
using SMT.Application.Interfaces.Inventory.Rental;
using SMT.Application.Interfaces.Items;
using SMT.Domain.Entities.Accounts;
using SMT.Domain.Entities.Inventory.Rental;
using SMT.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Infrastructure.Repositories.Inventory.Rental
{
    public class RentalContractService : IRentalContractService
    {
        private readonly IRentalContractRepository _contractRepo;
        private readonly IRentalContractItemRepository _itemRepo;
        private readonly IProductSerialRepository _serialRepo;
        private readonly ICustomerLedgerRepository _customerLedgerRepo;
        private readonly ICashTransactionService _cashTransactionService;
        public RentalContractService(
            IRentalContractRepository contractRepo,
            IRentalContractItemRepository itemRepo,
            IProductSerialRepository serialRepo,
            ICustomerLedgerRepository ledgerRepo,
            ICashTransactionService cashTransactionService)
        {
            _contractRepo = contractRepo;
            _itemRepo = itemRepo;
            _serialRepo = serialRepo;
            _customerLedgerRepo = ledgerRepo;
            _cashTransactionService = cashTransactionService;
        }

        public async Task<long> CreateAsync(CreateRentalContractRequest request)
        {
            using var tx = await _contractRepo.BeginTransactionAsync();

            try
            {
                // ============================
                // VALIDATION
                // ============================

                if (request.Items == null || !request.Items.Any())
                    throw new Exception("No rental items found.");

                // Prevent duplicate serial scan
                var duplicateSerials = request.Items
                    .SelectMany(x => x.SerialNumbers)
                    .GroupBy(x => x.Trim())
                    .Where(x => x.Count() > 1)
                    .Select(x => x.Key)
                    .ToList();

                if (duplicateSerials.Any())
                {
                    throw new Exception(
                        $"Duplicate serial found: {string.Join(", ", duplicateSerials)}");
                }
                // ============================
                // CREATE CONTRACT
                // ============================

                var contract = new RentalContract
                {
                    ContractNumber = await GenerateContractNumberAsync(),
                    CustomerId = request.CustomerId,
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    BillingCycle = (BillingCycle)request.BillingCycle,
                    NextBillingDate = request.StartDate,
                    Status = RentalContractStatus.Active,
                    Note = request.Note
                };

                await _contractRepo.CreateAsync(contract);

                // ============================
                // FETCH SERIALS
                // ============================

                var serialNumbers = request.Items.SelectMany(x => x.SerialNumbers)
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Select(x => x.Trim())
                    .Distinct()
                    .ToList();

                var serials = await _serialRepo
                    .GetBySerialNumbersAsync(serialNumbers);

                if (serials.Count != serialNumbers.Count)
                    throw new Exception("Some serials not found.");

                // ============================
                // VALIDATE SERIAL STATUS
                // ============================

                if (serials.Any(x => x.Status != ProductSerialStatus.InStock))
                    throw new Exception("Some serials already sold/rented.");

                // ============================
                // CREATE CONTRACT ITEMS
                // ============================

                var items = new List<RentalContractItem>();

                foreach (var reqItem in request.Items)
                {
                    foreach (var serialNumber in reqItem.SerialNumbers)
                    {
                        var serial = serials.First(x =>
                            x.SerialNumber == serialNumber);

                        var item = new RentalContractItem
                        {
                            RentalContractId = contract.Id,
                            ProductId = serial.ProductId,
                            ProductSerialId = serial.Id,
                            Rate = reqItem.Rent,
                            Quantity = 1
                        };

                        items.Add(item);

                        // Update serial status
                        serial.Status = ProductSerialStatus.InRent;
                    }
                }
                await _itemRepo.CreateRangeAsync(items);

                // ============================
                // UPDATE SERIAL STATUS
                // ============================

                await _serialRepo.UpdateRangeAsync(serials);

                // ============================
                // OPTIONAL:
                // SECURITY DEPOSIT LEDGER
                // ============================

                if (request.SecurityDeposit > 0)
                {
                    await _cashTransactionService.CreateAsync(new CashTransactionDto
                    {
                        CashAccountId = request.CashAccountId,
                        Amount = request.SecurityDeposit,
                        TransactionDate = DateTime.UtcNow,
                        TransactionType = (int)TransactionType.CashIn,
                        SourceType = (int)TransactionSource.RentalSecurityDeposit,
                        ReferenceId = contract.Id,
                        Note = $"Security Deposit for Rental Contract #{contract.ContractNumber}"
                    });

                    await _customerLedgerRepo.CreateCustomerLedger(new CustomerLedgerDto
                    {
                        CustomerId = request.CustomerId,
                        SourceType = CustomerLedgerSourceType.RentalContractSecurityDeposit,
                        SourceId = contract.Id,
                        Credit = request.SecurityDeposit,
                        Debit = 0,
                        Description = $"Security Deposit for Rental Contract # {contract.ContractNumber}"
                    });
                }

                await tx.CommitAsync();

                return contract.Id;
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        public async Task CloseAsync(long contractId)
        {
            using var tx = await _contractRepo.BeginTransactionAsync();

            try
            {
                var contract = await _contractRepo
                    .GetByIdAsync(contractId);

                if (contract == null)
                    throw new Exception("Contract not found.");

                if (contract.Status != RentalContractStatus.Active &&
                    contract.Status != RentalContractStatus.Overdue)
                {
                    throw new Exception("Contract already closed.");
                }

                // ============================
                // RETURN SERIALS TO STOCK
                // ============================

                var serialIds = contract.Items
                    .Select(x => x.ProductSerialId)
                    .ToList();

                var serials = await _serialRepo
                    .GetByProductSerialIdsAsync(serialIds);

                foreach (var serial in serials)
                {
                    serial.Status = ProductSerialStatus.InStock;
                }

                await _serialRepo.UpdateRangeAsync(serials);

                // ============================
                // CLOSE CONTRACT
                // ============================

                contract.Status = RentalContractStatus.Completed;

                await _contractRepo.UpdateAsync(contract);

                await tx.CommitAsync();
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        private async Task<string> GenerateContractNumberAsync()
        {
            return $"RC-{DateTime.UtcNow.Ticks}";
        }

        public Task<PagedResult<RentalContractDto>> GetPagedAsync(SearchRentalContractDto searchSaleDto)
        {
           return _contractRepo.GetPagedAsync(searchSaleDto);
        }

        public async Task<RentalContractInvoiceDto> GetRentalContractByIdAsync(long id)
        {
            return await _contractRepo.GetRentalContractByIdAsync(id);
        }
    }
}
