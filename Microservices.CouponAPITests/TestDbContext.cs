using Microservices.CouponAPI.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microservices.CouponAPITests
{
    public class TestDbContext : MsDbContext
    {
        private readonly ITestContext _testContext;

        public TestDbContext(DbContextOptions<MsDbContext> options, ITestContext testContext) : base(options)
        {
            _testContext = testContext;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            Action updateEntityChanges = () => { };
            var entries = ChangeTracker.Entries();
            foreach (var entry in entries)
            {
                var state = entry.State;
                updateEntityChanges += () => _testContext.AddEntityChange(entry.Entity, state);
            }

            var result = await base.SaveChangesAsync(cancellationToken);
            updateEntityChanges();
            return result;
        }
    }
}
