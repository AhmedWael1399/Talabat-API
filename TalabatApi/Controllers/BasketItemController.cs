using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TalabatApi.Errors;
using TalabatCore.Entities;
using TalabatCore.Repositories;

namespace TalabatApi.Controllers
{

    public class BasketItemController : APIBaseController
    {
        private readonly IBasketIItemRepository _basketIItemRepository;

        public BasketItemController(IBasketIItemRepository basketIItemRepository)
        {
            _basketIItemRepository = basketIItemRepository;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerBasket>> GetCustomerBasket(string basketId)
        {
            var basket = await _basketIItemRepository.GetBasketAsync(basketId);
            return basket is null ? new CustomerBasket(basketId) : basket;
        }

        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasket basket)
        {
            var createdOrUpdated = await _basketIItemRepository.UpdateBasketAsync(basket);
            return createdOrUpdated is null ? BadRequest(new ApiResponse(400)) : Ok(createdOrUpdated);
        }

        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteBasket (string basketId)
        {
            return await _basketIItemRepository.DeleteBasketAsync(basketId);
        }
    }
}
