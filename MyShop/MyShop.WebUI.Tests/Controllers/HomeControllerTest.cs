using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyShop.Core.Contracts;
using MyShop.Core.models;
using MyShop.Core.ViewModels;
using MyShop.WebUI;
using MyShop.WebUI.Controllers;

namespace MyShop.WebUI.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void IndexPageDoesReturnProducts()
        {
            //DI the MOCK repositories
            IRepository<Product> productContext = new Mocks.MockContext<Product>();
            IRepository<ProductCategory> productCategoryContext = new Mocks.MockContext<ProductCategory>();

            //insert a product jsut so the number of produsts is 1
            productContext.Insert(new Product());
            HomeController controller = new HomeController(productContext, productCategoryContext);//this is the constructor

            //Index returns something... we will make is a 'ViewResult'
            var result = controller.Index() as ViewResult;

            //typeDef that results' viewdata.model as the correct model.
            var viewModel = (ProductListViewModel)result.ViewData.Model;

            //check if the correct number of products are present.
            Assert.AreEqual(1, viewModel.Products.Count());
        }
    }
}
