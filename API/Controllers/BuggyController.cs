using API.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuggyController : ControllerBase
    {
        [HttpGet("unauthorized")]
        public IActionResult GetUnauthorized()
        {
            return Unauthorized();
        }
        [HttpGet("badrequest")]
        public IActionResult GetBadRequest()
        {
            return BadRequest("Not a good request");
        }
        [HttpGet("notfound")]
        public IActionResult GetNotFound()
        {
            return NotFound("We cannot find it");
        }
        [HttpGet("internalerror")]
        public IActionResult GetInternalError()
        {
            try
            {
                // Simulate some failure
                throw new Exception("Something went wrong!");
            }
            catch (Exception ex)
            {
                // Return RFC 7807 problem details with HTTP 500
                return Problem(
                    detail: ex.Message,
                    statusCode: 500,
                    title: "Internal Server Error"
                );
            }
        }
        [HttpPost("validationerror")]
        public IActionResult GetValidationError(CreateProductDto product)
        {

            // Process valid request
            return Ok();
        }
    }
}
