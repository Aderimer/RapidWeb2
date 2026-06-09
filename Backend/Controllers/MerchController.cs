using Backend.Data;
using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class MerchController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public MerchController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }


        [HttpGet]
        public async Task<IActionResult> GetMerch()
        {
            if (await _dataContext.Merch.FindAsync() == null)
            {
                return NotFound("No merch found in the database.");
            }

            var merch = await _dataContext.Merch.ToListAsync();
            return Ok(merch);
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateMerch([FromBody] Merch merch)
        {
            if (merch == null)
            {
                return BadRequest();
            }

            _dataContext.Merch.Add(merch);
            await _dataContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMerch), new { id = merch.MerchId }, merch);
        }


        [HttpPatch("merchId")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditMerch(int merchId, [FromBody] Merch updatedMerch)
        {
            if (updatedMerch == null)
            {
                return BadRequest();
            }

            var merch = await _dataContext.Merch.FindAsync(merchId);
            if (merch == null)
            {
                return NotFound("No merch with that ID found.");
            }

            merch.ProductName = updatedMerch.ProductName;
            merch.Price = updatedMerch.Price;
            merch.Size = updatedMerch.Size;
            merch.CategoryId = updatedMerch.CategoryId;

            await _dataContext.SaveChangesAsync();

            return Ok(merch);
        }


        [HttpDelete("merchId")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteMerch(int merchId)
        {
            var merch = await _dataContext.Merch.FindAsync(merchId);
            if (merch == null)
            {
                return NotFound();
            }

            _dataContext.Merch.Remove(merch);
            await _dataContext.SaveChangesAsync();

            return Ok("Deleted merch.");
        }
    }
}