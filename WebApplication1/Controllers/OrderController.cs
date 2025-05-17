using Microsoft.AspNetCore.Mvc;
using Order.Services;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {

        private readonly IOrderProducer _orderProducer;

        public OrderController(IOrderProducer orderProducer)
        {
            _orderProducer = orderProducer;
        }




        /// <summary>
        /// 随机生成订单
        /// </summary>
        /// <param name="total">随机生成订单的数量</param>
        /// <returns></returns>
        [HttpPost]

        public IActionResult Send(int total)
        {


            _ = Task.Run(() => _orderProducer.SendBatchOrders(total));
            return Ok("");
        }

    }
}
