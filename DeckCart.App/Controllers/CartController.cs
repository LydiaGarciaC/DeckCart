using DeckCart.Business.Handlers.Interfaces;
using DeckCart.App.Facade;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using DeckCart.Business.Models;

[Route("api/[controller]")]
[ApiController]
public class CartController : ControllerBase
{
    private readonly ICartHandler _cartHandler;
    private readonly IMapper _mapper;

    public CartController(ICartHandler cartHandler, IMapper mapper)
    {
        _cartHandler = cartHandler;
        _mapper = mapper;
    }

    [HttpGet("{userId}")]
    public async Task<ActionResult<UserCartFacade>> GetCart(int userId)
    {
        if (userId <= 0)
        {
            return BadRequest("Invalid User ID.");
        }
        return Ok(_mapper.Map<UserCartFacade>(await _cartHandler.GetUserCartAsync(userId)));
    }

    [HttpPut]
    public async Task<IActionResult> ReplaceCart([FromBody] ReplaceCartRequestFacade request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await _cartHandler.ReplaceUserCartAsync(_mapper.Map<ReplaceCartRequest>(request));
        return Ok();
    }
}