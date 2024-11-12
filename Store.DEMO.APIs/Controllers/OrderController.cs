using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.DEMO.APIs.Errors;
using Store.DEMO.Core;
using Store.DEMO.Core.Dtos.Orders;
using Store.DEMO.Core.Entites.Order;
using Store.DEMO.Core.Services.Contract;
using System.Security.Claims;

namespace Store.DEMO.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public OrderController(IOrderService orderService, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _orderService = orderService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateOrder(OrderDto model)
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            if (buyerEmail is null) return Unauthorized(new ApiErrorResponse(StatusCodes.Status401Unauthorized));
            var address = _mapper.Map<Address>(model.shipToAddress);
            var order = await _orderService.CreateOrderAsync(buyerEmail, model.basketId, model.deliveryMethodId, address);
            if (order is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));
            return Ok(_mapper.Map<OrderToReturnDto>(order));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetOrdersForSpecificUser()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (userEmail is null) return Unauthorized(new ApiErrorResponse(StatusCodes.Status401Unauthorized));
            var orders = await _orderService.GetOrdersForSpecificAsync(userEmail);
            if (orders is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));
            return Ok(_mapper.Map<IEnumerable<OrderToReturnDto>>(orders));
        }
        [HttpGet("{orderId}")]
        [Authorize]
        public async Task<IActionResult> GetOrderForSpecificUser(int orderId)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (userEmail is null) return Unauthorized(new ApiErrorResponse(StatusCodes.Status401Unauthorized));
            var orders = await _orderService.GetOrderByIdForSpecificAsync(userEmail,orderId);
            if (orders is null) return NotFound(new ApiErrorResponse(StatusCodes.Status404NotFound));
            return Ok(_mapper.Map<OrderToReturnDto>(orders));
        }
        [HttpGet("DeliveryMethods")]
        public async Task<IActionResult> GetDeliveryMethods()
        {
            var deliveryMethods = await _unitOfWork.Repository<DeliveryMethod, int>().GetAllAsync();
            if (deliveryMethods is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));
            return Ok(deliveryMethods);
        }
    }
}
