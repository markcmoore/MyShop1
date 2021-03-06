﻿using System;
using System.Linq;
using System.Security.Principal;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyShop.Core.Contracts;
using MyShop.Core.models;
using MyShop.Core.ViewModels;
using MyShop.Services;
using MyShop.WebUI.Controllers;
using MyShop.WebUI.Tests.Mocks;

namespace MyShop.WebUI.Tests.Controllers
{
    [TestClass]
    public class BasketControllerTests
    {
        [TestMethod]
        public void CanAddBasketItem()
        {
            //SETUP
            IRepository<Basket> baskets = new MockContext<Basket>();
            IRepository<Product> products = new MockContext<Product>();
            IRepository<Order> orders = new MockContext<Order>();
            IRepository<Customer> customers = new MockContext<Customer>();

            var httpContext = new MockHttpContext();

            IBasketService basketService = new BasketService(products, baskets);
            IOrderService orderService = new OrderService(orders);

            var controller = new BasketController(basketService, orderService, customers);
            controller.ControllerContext = new System.Web.Mvc.ControllerContext(httpContext, new System.Web.Routing.RouteData(), controller);

            //ACT
            //basketService.AddToBasket(httpContext, "1");
            controller.AddToBasket("1");
            Basket basket = baskets.Collection().FirstOrDefault();

            //ASSERT
            Assert.IsNotNull(basket);
            Assert.AreEqual(1, basket.BasketItems.Count);
            Assert.AreEqual("1", basket.BasketItems.ToList().FirstOrDefault().ProductId);
        }

        [TestMethod]
        public void CanGetSummaryViewModel()
        {
            //SETUP
            IRepository<Basket>  baskets  = new MockContext<Basket>();
            IRepository<Product> products = new MockContext<Product>();
            IRepository<Order>   orders   = new MockContext<Order>();
            IRepository<Customer> customers = new MockContext<Customer>();

            //create some products
            products.Insert(new Product() { Id = "1", Price = 10.01m });
            products.Insert(new Product() { Id = "2", Price = 11.11m });

            //create a basket to put the products in
            Basket basket = new Basket();

            //put the products and their quantities in the basket
            basket.BasketItems.Add(new BasketItem() { ProductId = "1", Quantity = 2 });
            basket.BasketItems.Add(new BasketItem() { ProductId = "2", Quantity = 5 });

            //put the basket into the List<Basket>
            baskets.Insert(basket);

            //create the basketService (interface) context to put the baskets into.
            IBasketService basketService = new BasketService(products, baskets);
            IOrderService orderService = new OrderService(orders);

            var controller = new BasketController(basketService, orderService, customers);
            var httpContext = new MockHttpContext();

            httpContext.Request.Cookies.Add(new System.Web.HttpCookie("eCommerceBasket") { Value = basket.Id });
            controller.ControllerContext = new System.Web.Mvc.ControllerContext(httpContext, new System.Web.Routing.RouteData(), controller);

            var result = controller.BasketSummary() as PartialViewResult;
            var basketSummary = (BasketSummaryViewModel)result.ViewData.Model;

            Assert.AreEqual(7, basketSummary.BasketCount);
            Assert.AreEqual(75.57m, basketSummary.BasketTotal);
        }

        [TestMethod]
        public void CanCheckoutAndCreateOrder()
        {
            //SETUP
            IRepository<Basket> baskets = new MockContext<Basket>();
            IRepository<Product> products = new MockContext<Product>();
            IRepository<Order> orders = new MockContext<Order>();
            IRepository<Customer> customers = new MockContext<Customer>();

            //create some products
            products.Insert(new Product() { Id = "1", Price = 10.01m });
            products.Insert(new Product() { Id = "2", Price = 11.11m });

            //create a basket to put the products in
            Basket basket = new Basket();

            //put the products and their quantities in the basket
            basket.BasketItems.Add(new BasketItem() { ProductId = "1", Quantity = 2 });
            basket.BasketItems.Add(new BasketItem() { ProductId = "2", Quantity = 5 });

            //put the basket into the List<Basket>
            baskets.Insert(basket);

            //create the basketService (interface) context to put the baskets into.
            IBasketService basketService = new BasketService(products, baskets);
            IOrderService orderService = new OrderService(orders);

            customers.Insert(new Customer() 
            { 
                Id = "1", 
                Email = "mark.c.moore@hotmail.com", 
                ZipCode = "76036" 
            });

            IPrincipal FakeUser = new GenericPrincipal(new GenericIdentity("mark.c.moore@hotmail.com", "Forms"), null);

            var controller = new BasketController(basketService, orderService, customers);
            var httpContext = new MockHttpContext();
            httpContext.User = FakeUser;

            httpContext.Request.Cookies.Add(new System.Web.HttpCookie("eCommerceBasket") { Value = basket.Id });
            controller.ControllerContext = new System.Web.Mvc.ControllerContext(httpContext, new System.Web.Routing.RouteData(), controller);

            //ACT
            Order order = new Order(); //create an order object
            // pass in the empty order to the controller. The controller already has the 'httpContext'
            //Checkout() will plave the order, process payment, and empty the basket
            controller.Checkout(order);

            //ASSERT
            Assert.AreEqual(2, order.OrderItems.Count());//2 items... quantity will be 7
            Assert.AreEqual(0, basket.BasketItems.Count());//basket should now be empty

            Order orderInRep = orders.Find(order.Id);
            Assert.AreEqual(2, orderInRep.OrderItems.Count());
        }
    }
}
