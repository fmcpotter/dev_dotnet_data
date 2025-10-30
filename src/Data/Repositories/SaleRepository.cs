using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.Models;
using Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private readonly SalesContext _context;

        public SaleRepository(SalesContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Sale sale)
        {
            _context.Add(sale);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var sale = await _context.Sales.Include(s => s.Items).FirstOrDefaultAsync(s => s.Id == id);
            if (sale == null) return;
            sale.Cancel();
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Sale>> GetAllAsync()
        {
            return await _context.Sales.Include(s => s.Items).AsNoTracking().ToListAsync();
        }

        public async Task<Sale?> GetByIdAsync(Guid id)
        {
            return await _context.Sales.Include(s => s.Items).FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task UpdateAsync(Sale sale)
        {
            _context.Update(sale);
            await _context.SaveChangesAsync();
        }
    }
}