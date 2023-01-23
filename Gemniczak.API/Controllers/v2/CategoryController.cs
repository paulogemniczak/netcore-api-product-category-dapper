using Gemniczak.API.Validators;
using Microsoft.AspNetCore.Mvc;
using Gemniczak.AppService.Interfaces;
using System.Net.Mime;
using Gemniczak.API.Results;
using Gemniczak.AppService.Dtos;
using Gemniczak.AppService.Filters;

namespace Gemniczak.API.Controllers.v2
{
	[ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
	[ApiVersion("2.0")]
    [Produces("application/json")]
    public class CategoryController : Controller
    {
        private readonly ICategoryAppService _appService;
        private readonly CategoryValidator _validator;

        public CategoryController(
            ICategoryAppService appService,
            CategoryValidator validator)
        {
            _appService = appService;
            _validator = validator;
        }

        /// <summary>
        /// Get a list of category.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        /// <remarks>
        /// Request example:
        /// 
        ///     GET /api/v{version}/Category?InputText=cat&amp;PageNumber=1&amp;PageSize=10&amp;OrderBy=categoryName&amp;IsAsc=true
        ///     {
        ///     }
        /// 
        /// </remarks>
        /// <response code="200">Get categories successfully.</response>
        /// <response code="500">Internal server error.</response>
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[MapToApiVersion("2.0")]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] CategoryFilterDto filter)
        {
            var result = new GenericResult<IEnumerable<CategoryDto>>();

            try
            {
                result.Result = await _appService.List(filter);
				result.Result.Append(new CategoryDto() { CategoryId = 24, CategoryName = "bananinha"});
                result.Success = true;
                return StatusCode(StatusCodes.Status200OK, result);
            }
            catch (Exception ex)
            {
                result.Errors = new string[] { ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }
        }
    }
}
