using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Charts.Domain.DashboardAggregate;
using Domain.SharedKernel;
using Microsoft.EntityFrameworkCore;

namespace Charts.Infrastructure.Repositories
{
    public class DashboardRepository : IDashBoardRepository
    {
        private readonly ChartContext _context;

        public DashboardRepository(ChartContext context)
        {

            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IUnitOfWork UnitOfWork => _context;


        private async Task LoadResponsiveSizeAsync()
        {
            if (_context.Sizes.Local.Count == 0)
                await _context.Sizes.ToListAsync();
        }



        public async Task<Dashboard> FindByIdAsync(int id)
        {
            await LoadResponsiveSizeAsync();
            var dash = await _context.Dashboards.FindAsync(id);

            if (dash != null)
            {
                await LoadContainerAsync(id);
            }

            return dash;
        }

        private async Task LoadContainerAsync(int id)
        {
            var containers = await (
                    from c in _context.Containers
                        .Include(a => a.ChildContainers)
                        .Include(a => a.ChartContainers)
                    where EF.Property<int>(c, "DashboardId") == id
                    select c).ToArrayAsync();
        }


        public void Add(Dashboard dashboard)
        {
            _context.Dashboards.Add(dashboard);

            foreach (var entityEntry in _context.ChangeTracker.Entries<ResponsiveSize>())
            {
                entityEntry.State = EntityState.Unchanged;
            }
        }

        public void Remove(Dashboard dashboard)
        {
            _context.Dashboards.Remove(dashboard);
            //_context.FieldGroups.RemoveRange(chart.FieldGroups);
            //_context.Fields.RemoveRange(chart.FieldGroups.SelectMany(a => a.Fields));

            _context.Containers.RemoveRange(dashboard.Containers);
            _context.ChartContainers.RemoveRange(dashboard.Containers.SelectMany(d=>d.ChartContainers));


         }
    }
}