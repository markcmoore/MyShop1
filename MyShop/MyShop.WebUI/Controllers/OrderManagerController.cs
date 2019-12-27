using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyShop.Core.Contracts;
using MyShop.Core.models;

namespace MyShop.WebUI.Controllers
{
    [Authorize (Roles = "Admin")]
    public class OrderManagerController : Controller
    {
        IOrderService orderContext;

        public OrderManagerController(IOrderService orderContext)
        {
            this.orderContext = orderContext;
        }

        // GET: OrderManager
        public ActionResult Index()
        {
            List<Order> orders = orderContext.GetOrderList();
            return View(orders);
        }

        public ActionResult UpdateOrder(string Id)
        {
            ViewBag.StatusList = new List<string>()
            {
                "Order Created",
                "Payment Processed",
                "Order Shipped",
                "Order Complete"
            };
            Order order = orderContext.GetOrder(Id);
            return View(order);
        }

        [HttpPost]
        public ActionResult UpdateOrder(Order updatedOrder, string Id)
        {
            Order order = orderContext.GetOrder(Id);
            order.OrderStatus = updatedOrder.OrderStatus;

            orderContext.UpdateOrder(order);

            return RedirectToAction("Index");
        }


    }
}