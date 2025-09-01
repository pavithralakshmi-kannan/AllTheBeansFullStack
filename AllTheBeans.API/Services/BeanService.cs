using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using AllTheBeans.API.Data;
using AllTheBeans.API.Models;
using Microsoft.EntityFrameworkCore;

namespace AllTheBeans.API.Services
{
    public sealed class BeanService : IBeanService
    {
        private readonly ApplicationDbContext _db;
        private static readonly Random _rng = new();

        public BeanService(ApplicationDbContext db) => _db = db;

        // SEARCH
        public async Task<(IReadOnlyList<Bean> Items, int Total)> SearchAsync(
            string? q, string? country, string? colour,
            decimal? minCost, decimal? maxCost,
            int page = 1, int pageSize = 20, string? sort = null)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 20;

            IQueryable<Bean> query = _db.Beans.AsNoTracking();

            // country filter
            if (!string.IsNullOrWhiteSpace(country))
            {
                var lowerCountry = country.Trim().ToLower();
                query = query.Where(b => b.Country != null && b.Country.ToLower() == lowerCountry);
            }

            // colour filter
            if (!string.IsNullOrWhiteSpace(colour))
            {
                var lowerColour = colour.Trim().ToLower();
                query = query.Where(b => b.Colour != null && b.Colour.ToLower() == lowerColour);
            }

            if (minCost.HasValue)
                query = query.Where(b => b.Cost >= minCost.Value);

            if (maxCost.HasValue)
                query = query.Where(b => b.Cost <= maxCost.Value);

            // search for q across multiple fields
            if (!string.IsNullOrWhiteSpace(q))
            {
                var term = q.Trim().ToLower();
                query = query.Where(b =>
                    (b.Name != null && b.Name.ToLower().Contains(term)) ||
                    (b.Description != null && b.Description.ToLower().Contains(term)) ||
                    (b.Country != null && b.Country.ToLower().Contains(term)) ||
                    (b.Colour != null && b.Colour.ToLower().Contains(term))
                );
            }

            // Sorting
            query = sort?.ToLowerInvariant() switch
            {
                "name"  => query.OrderBy(b => b.Name),
                "-name" => query.OrderByDescending(b => b.Name),
                "cost"  => query.OrderBy(b => b.Cost),
                "-cost" => query.OrderByDescending(b => b.Cost),
                _       => query.OrderBy(b => b.Id)
            };

            var total = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, total);
        }

        // CRUD
        public Task<Bean?> GetByIdAsync(int id) =>
            _db.Beans.AsNoTracking().FirstOrDefaultAsync(b => b.Id == id)!;

        public async Task<Bean> CreateAsync(Bean input)
        {
            _db.Beans.Add(input);
            await _db.SaveChangesAsync();
            return input;
        }

        public async Task<Bean?> UpdateAsync(int id, Bean input)
        {
            var existing = await _db.Beans.FirstOrDefaultAsync(b => b.Id == id);
            if (existing is null) return null;

            existing.Name = input.Name;
            existing.Cost = input.Cost;
            existing.Colour = input.Colour;
            existing.Country = input.Country;
            existing.Description = input.Description;
            existing.Image = input.Image;

            await _db.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _db.Beans.FirstOrDefaultAsync(b => b.Id == id);
            if (existing is null) return false;

            _db.Beans.Remove(existing);
            await _db.SaveChangesAsync();
            return true;
        }

        // Bean of the Day (avoids repeating yesterday)
        public async Task<Bean?> GetBeanOfTheDayAsync(DateTime? today = null)
        {
            var date = (today ?? DateTime.UtcNow).Date;

            // existing for today
            var todays = await _db.DailySelections.AsNoTracking()
                .FirstOrDefaultAsync(d => d.Date == date);

            if (todays != null)
                return await _db.Beans.AsNoTracking().FirstOrDefaultAsync(b => b.Id == todays.BeanId);

            // avoid yesterday's bean
            var yesterday = date.AddDays(-1);
            var yId = await _db.DailySelections.AsNoTracking()
                .Where(d => d.Date == yesterday)
                .Select(d => d.BeanId)
                .FirstOrDefaultAsync();

            var candidateIds = await _db.Beans.AsNoTracking()
                .Where(b => b.Id != yId)
                .Select(b => b.Id)
                .ToListAsync();

            if (candidateIds.Count == 0) return null;

            var pickedId = candidateIds[_rng.Next(candidateIds.Count)];
            _db.DailySelections.Add(new DailySelection { Date = date, BeanId = pickedId });
            await _db.SaveChangesAsync();

            return await _db.Beans.AsNoTracking().FirstOrDefaultAsync(b => b.Id == pickedId);
        }
    }
}