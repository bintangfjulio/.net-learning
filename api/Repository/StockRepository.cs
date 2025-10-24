using api.Data;
using api.Dtos.Stock;
using api.Helpers;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDBContext _context;

        public StockRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Stock> CreateAsync(Stock stockModel)
        {
            await _context.Stocks.AddAsync(stockModel);
            await _context.SaveChangesAsync();
            return stockModel;
        }

        public async Task<Stock?> DeleteAsync(int id)
        {
            var stockModel = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);
            
            if (stockModel == null)
            {
                return null;
            }
            
            _context.Stocks.Remove(stockModel!);
            await _context.SaveChangesAsync();

            return stockModel;
        }

        public async Task<List<Stock>> GetAllAsync(QueryObject query)
        {
            // return await _context.Stocks.Include(x => x.Comments).ToListAsync();
            var stocks = _context.Stocks.Include(x => x.Comments).AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.CompanyName))
            {
                stocks = stocks.Where(x => x.CompanyName.Contains(query.CompanyName));
            }

            if (!string.IsNullOrWhiteSpace(query.Symbol))
            {
                stocks = stocks.Where(x => x.Symbol.Contains(query.Symbol));
            }

            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                if (query.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase))
                {
                    stocks = query.IsDescending ? stocks.OrderByDescending(x => x.Symbol) : stocks.OrderBy(x => x.Symbol);
                }

                if (query.SortBy.Equals("CompanyName", StringComparison.OrdinalIgnoreCase))
                {
                    stocks = query.IsDescending ? stocks.OrderByDescending(x => x.CompanyName) : stocks.OrderBy(x => x.CompanyName);
                }
            }
            
            var skipNumber = (query.PageNumber - 1) * query.PageSize;
            stocks = stocks.Skip(skipNumber).Take(query.PageSize); 

            return await stocks.ToListAsync();
        }

        public async Task<Stock?> GetByIdAsync(int id)
        {
            return await _context.Stocks.Include(x => x.Comments).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto stockDto)
        {
            var existingStock = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);
            if (existingStock == null)
            {
                return null;
            }

            existingStock.Symbol = stockDto.Symbol;
            existingStock.CompanyName = stockDto.CompanyName;
            existingStock.Purchase = stockDto.Purchase;
            existingStock.LastDiv = stockDto.LastDiv;
            existingStock.Industry = stockDto.Industry;
            existingStock.MarketCap = stockDto.MarketCap;
            await _context.SaveChangesAsync();

            return existingStock;
        }
        
        public Task<bool> StockExist(int id)
        {
            return _context.Stocks.AnyAsync(x => x.Id == id);
        }
    }
}