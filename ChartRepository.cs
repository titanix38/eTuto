using Domain.SharedKernel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Charts.Domain.ChartAggregate;
using Charts.Domain.Fields;
using Charts.Domain.DashboardAggregate;
using System.Linq;
using System.Collections.Generic;

namespace Charts.Infrastructure.Repositories
{
    public class ChartRepository : IBaseChartRepository
    {

        private readonly ChartContext _context;

        public ChartRepository(ChartContext context)
        {

            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IUnitOfWork UnitOfWork => _context;

        public void Add(BaseChart chart)
        {
            _context.ChartElements.Add(chart);

            //FieldType  enums data should be already present inside database
            //added during seed with CharContext Seed
            foreach (var entityEntry in _context.ChangeTracker.Entries<FieldType>())
            {
                entityEntry.State = EntityState.Unchanged;
            }
        }


        private async Task LoadFieldTypeAsync()
        {
            if (_context.FieldTypes.Local.Count == 0)
                await _context.FieldTypes.ToListAsync();
        }

        public async Task<BaseChart> FindByIdAsync(int id)
        {
            await LoadFieldTypeAsync();

            var chart = await _context
                        .ChartElements
                        .Include(c => c.FieldGroups)
                            .ThenInclude(a => a.Fields)
                               .ThenInclude(a => a.Mappers)
                                    .ThenInclude(a => a.OriginFieldType)
                        .SingleOrDefaultAsync(a => a.Id == id);
            return chart;
        }

        public void Remove(BaseChart chart)
        {
            _context.ChartElements.Remove(chart);
            _context.FieldGroups.RemoveRange(chart.FieldGroups);
            _context.Fields.RemoveRange(chart.FieldGroups.SelectMany(a => a.Fields));
            _context.Mappers.RemoveRange(chart.FieldGroups.SelectMany(a => a.Fields.SelectMany( m => m.Mappers)));
        }
    }
}
