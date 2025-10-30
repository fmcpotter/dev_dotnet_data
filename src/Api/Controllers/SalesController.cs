using System;
using System.Linq;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.Models;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Api.Dtos;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalesController : ControllerBase
    {
        private readonly ISaleRepository _repo;
        private readonly IEventPublisher _events;
        private readonly ILogger<SalesController> _logger;

        public SalesController(ISaleRepository repo, IEventPublisher events, ILogger<SalesController> logger)
        {
            _repo = repo;
            _events = events;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var sales = await _repo.GetAllAsync();
            return Ok(sales.Select(s => SaleDto.FromDomain(s)));
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var sale = await _repo.GetByIdAsync(id);
            if (sale == null) return NotFound();
            return Ok(SaleDto.FromDomain(sale));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSaleDto dto)
        {
            var sale = new Sale(dto.SaleNumber, dto.Date, dto.ClientExternalId, dto.ClientDescription, dto.BranchExternalId, dto.BranchDescription);
            foreach (var it in dto.Items)
            {
                var item = new SaleItem(it.ProductExternalId, it.ProductDescription, it.Quantity, it.UnitPrice, it.Discount);
                sale.AddItem(item);
            }

            await _repo.AddAsync(sale);
            _events.Publish(new Domain.Events.PurchaseCreated(sale.Id, sale.SaleNumber, sale.Date, sale.TotalAmount));
            _logger.LogInformation("Sale created {SaleId}", sale.Id);
            return CreatedAtAction(nameof(Get), new { id = sale.Id }, SaleDto.FromDomain(sale));
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateSaleDto dto)
        {
            var sale = await _repo.GetByIdAsync(id);
            if (sale == null) return NotFound();

            sale.UpdateHeader(dto.ClientExternalId, dto.ClientDescription, dto.BranchExternalId, dto.BranchDescription, dto.Date);

            // replace/update items (simple approach)
            foreach (var up in dto.Items)
            {
                if (up.Id == Guid.Empty)
                {
                    var newItem = new SaleItem(up.ProductExternalId, up.ProductDescription, up.Quantity, up.UnitPrice, up.Discount);
                    sale.AddItem(newItem);
                }
                else
                {
                    var item = new SaleItem(up.ProductExternalId, up.ProductDescription, up.Quantity, up.UnitPrice, up.Discount)
                    {
                        // ensure same id to update
                    };
                    // create temporary with id using reflection (for shortness)
                    typeof(SaleItem).GetProperty("Id")?.SetValue(item, up.Id);
                    sale.UpdateItem(item);
                }
            }

            await _repo.UpdateAsync(sale);
            _events.Publish(new Domain.Events.PurchaseUpdated(sale.Id, sale.SaleNumber, sale.Date, sale.TotalAmount));
            return Ok(SaleDto.FromDomain(sale));
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Cancel(Guid id)
        {
            var sale = await _repo.GetByIdAsync(id);
            if (sale == null) return NotFound();
            await _repo.DeleteAsync(id);
            _events.Publish(new Domain.Events.PurchaseCancelled(sale.Id, sale.SaleNumber, sale.Date));
            return NoContent();
        }

        [HttpPost("{id:guid}/items/{itemId:guid}/cancel")]
        public async Task<IActionResult> CancelItem(Guid id, Guid itemId)
        {
            var sale = await _repo.GetByIdAsync(id);
            if (sale == null) return NotFound();
            sale.CancelItem(itemId);
            await _repo.UpdateAsync(sale);
            _events.Publish(new Domain.Events.ItemCancelled(sale.Id, itemId, sale.Items.First(i => i.Id == itemId).ProductExternalId));
            return Ok(SaleDto.FromDomain(sale));
        }
    }
}