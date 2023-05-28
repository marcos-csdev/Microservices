using Xunit;
using Microservices.CouponAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microservices.CouponAPITests;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Microservices.CouponAPI.Controllers.Tests
{
    public class CouponAPIControllerTests
    {
        public CouponAPIControllerTests() 
        {
            //var serviceCollection = ProviderFactory.SetService<CouponAPIController>();

        }

        

        [Fact()]
        public void GetAllTest()
        {


        }

    }

}