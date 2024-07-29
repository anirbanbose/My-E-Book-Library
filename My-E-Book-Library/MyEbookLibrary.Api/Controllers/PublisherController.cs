using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyEbookLibrary.Api.ActionFilters;
using MyEbookLibrary.Common;
using MyEbookLibrary.Common.DTO.Account;
using MyEbookLibrary.Common.DTO.Publisher;
using MyEbookLibrary.Common.DTO.Requests;
using MyEbookLibrary.Common.DTO.Responses;
using MyEbookLibrary.Common.Enums;
using MyEbookLibrary.Service.Contracts;
using MyEbookLibrary.Service.Services;

namespace MyEbookLibrary.Api.Controllers
{
    [Route("api/publisher")]
    [ApiController]
    public class PublisherController : ControllerBase
    {
        private readonly ILogger<PublisherController> _logger;
        private readonly IAuthService _authService;
        private readonly IPublisherService _publisherService;

        public PublisherController(ILogger<PublisherController> logger, IAuthService authService, IPublisherService publisherService)
        {
            _logger = logger;
            _authService = authService;
            _publisherService = publisherService;
        }

        [HttpGet("publisherlist")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ListResult<PublisherListDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ServiceFilter(typeof(AccessTokenProcessorAttribute))]
        [Authorize()]
        public async Task<IResult> GetPublisherList(int pageNumber, int pageSize, OrderByEnum sortOrder, string searchText = "", string sortColumn = "")
        {
            if (pageSize <= 0 || pageNumber < 0)
            {
                return Results.BadRequest();
            }

            ListRequestDTO listRequest = new ListRequestDTO(pageNumber, pageSize, searchText, sortColumn, sortOrder);
            var user = HttpContext.Items[Constants.USER_DTO_NAME] as UserDTO;
            var publisherList = await _publisherService.GetPublisherList(listRequest, user!);

            return Results.Ok(publisherList);
        }

        [HttpPost("savepublisher")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SaveResult<PublisherDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ServiceFilter(typeof(AccessTokenProcessorAttribute))]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [Authorize()]
        public async Task<IResult> SavePublisher([FromBody] PublisherDTO publisher)
        {
            var user = HttpContext.Items[Constants.USER_DTO_NAME] as UserDTO;
            var result = await _publisherService.SavePublisher(publisher, user!);
            if (!result.IsSuccess)
            {
                return Results.BadRequest();
            }
            return Results.Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DetailResult<PublisherDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ServiceFilter(typeof(AccessTokenProcessorAttribute))]
        [Authorize()]
        public async Task<IResult> GetPublisher(int publisherId)
        {
            var user = HttpContext.Items[Constants.USER_DTO_NAME] as UserDTO;
            var result = await _publisherService.GetPublisher(publisherId, user!);
            if (!result.IsSuccess)
            {
                return Results.BadRequest();
            }
            return Results.Ok(result);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ServiceFilter(typeof(AccessTokenProcessorAttribute))]
        [Authorize()]
        public async Task<IResult> DeletePublisher(int publisherId)
        {
            var user = HttpContext.Items[Constants.USER_DTO_NAME] as UserDTO;
            var publisher = await _publisherService.DeletePublisher(publisherId, user!);

            return Results.Ok(publisher);
        }

        [HttpGet("publisherdropdownlist")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DropdownResult<PublisherDropdownListDTO>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ServiceFilter(typeof(AccessTokenProcessorAttribute))]
        [Authorize()]
        public async Task<IResult> GetPublisherDropdownList(string? q)
        {
            var user = HttpContext.Items[Constants.USER_DTO_NAME] as UserDTO;
            var publisherList = await _publisherService.GetPublisherDropdownList(q, user!);

            return Results.Ok(publisherList);
        }

    }
}
