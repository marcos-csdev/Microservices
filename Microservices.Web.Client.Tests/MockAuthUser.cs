using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Microservices.Web.Client.Tests
{
    public class MockAuthUser
    {
        public List<Claim> Claims { get; private set; } = new();

        public MockAuthUser(params Claim[] claims)
            => Claims = claims.ToList();
    }
}
