using NUnit.Framework;
using Microservices.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microservices.Web.Services.Abstractions;
using Microservices.CouponAPITests;

namespace Microservices.Web.Services.Tests
{
    [TestFixture()]
    public class CouponServiceTests
    {
        private IServiceProvider _couponService = null!;

        [SetUp]
        public void Setup()
        {
            //var couponServiceCollection = 
            //_couponService = new TestSetup();
        }
        [TearDown]
        public void Teardown() { }


        [Test()]
        public void CouponServiceTest()
        {
            Assert.Fail();
        }

        [Test()]
        public void AddEntityAsyncTest()
        {
            Assert.Fail();
        }

        [Test()]
        public void GetAllEntitiesAsyncTest()
        {
            Assert.Fail();
        }

        [Test()]
        public void GetEntityByIdAsyncTest()
        {
            Assert.Fail();
        }

        [Test()]
        public void RemoveEntityAsyncTest()
        {
            Assert.Fail();
        }

        [Test()]
        public void UpdateEntityAsyncTest()
        {
            Assert.Fail();
        }
    }
}