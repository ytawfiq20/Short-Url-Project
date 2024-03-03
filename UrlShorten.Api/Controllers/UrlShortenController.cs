using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UrlShorten.Api.Models;
using UrlShorten.Data;
using UrlShorten.Data.DTO;
using UrlShorten.Data.Entities;
using UrlShorten.Repository.Repositories.UrlRepository;

namespace UrlShorten.Api.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class UrlShortenController : ControllerBase
    {
        private readonly IUrl _urlRepository;
        public UrlShortenController(IUrl _urlRepository)
        {
            this._urlRepository = _urlRepository;
        }

        [HttpGet("[action]")]
        public IActionResult GetAllMappingUrls()
        {
            try
            {
                var allMappingUrls = _urlRepository.GetAllMappingUrls();
                if (allMappingUrls == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest,
                        new ApiResponseModel<IEnumerable<UrlMapper>>()
                        {
                            IsSuccess = true,
                            CreatedAt = DateTime.Now,
                            StatusCode = 404,
                            Value = allMappingUrls,
                            Message = "No content found."
                        }
                    );
                }
                return StatusCode(StatusCodes.Status200OK,
                    new ApiResponseModel<IEnumerable<UrlMapper>>()
                    {
                        IsSuccess = true,
                        CreatedAt = DateTime.Now,
                        StatusCode = 200,
                        Value = allMappingUrls,
                        Message = "Content found successfully."
                    }
                );
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponseModel<UserUrlInputDto>()
                    {
                        IsSuccess = false,
                        CreatedAt = DateTime.Now,
                        StatusCode = 500,
                        Message = "Error while trying to get mapping urls."
                    }
                );
            }
        }


        [HttpPost("[action]")]
        public ActionResult<ShortenUrlDto> CreateShortenUrl([FromBody]UserUrlInputDto userUrlInputDto)
        {
            try
            {
                if(!Uri.TryCreate(userUrlInputDto.InputUrl, UriKind.Absolute, out var result))
                {
                    return StatusCode(StatusCodes.Status400BadRequest,
                            new ApiResponseModel<UserUrlInputDto>()
                            {
                                IsSuccess = false,
                                CreatedAt = DateTime.Now,
                                LongUrl = userUrlInputDto.InputUrl,
                                StatusCode = 400,
                                Value = userUrlInputDto,
                                Message = "Invalid Url."
                            }
                        );
                }
                UrlMapper urlMapper = _urlRepository.CreateShortenUrlForUserInputUrl(userUrlInputDto);
                string shortUrl = CreateUrl(urlMapper.ShortenUrl);
                urlMapper.ShortenUrl = shortUrl;
                return StatusCode(StatusCodes.Status200OK,
                        new ApiResponseModel<UrlMapper>()
                        {
                            IsSuccess = true,
                            CreatedAt = DateTime.Now,
                            LongUrl = userUrlInputDto.InputUrl,
                            StatusCode = 200,
                            Value = urlMapper,
                            Message = $"Shorten url created successfully: ({shortUrl})."
                        }
                    );
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                        new ApiResponseModel<ShortenUrlDto>()
                        {
                            IsSuccess = false,
                            CreatedAt = DateTime.Now,
                            LongUrl = userUrlInputDto.InputUrl,
                            StatusCode = 500,
                            Message = $"Error while trying to create shorten url for url:({userUrlInputDto.InputUrl})."
                        }
                    );
            }
        }

        [HttpGet("{shortUrl}")]
        public ActionResult<OriginalUrlDto> RedirectToOriginalUrl([FromRoute]string shortUrl)
        {
            try
            {
                OriginalUrlDto originalUrlDto = _urlRepository.GetOriginalUrlByKey(shortUrl);
                if (originalUrlDto != null)
                {
                    return Redirect($"{originalUrlDto.OriginalUrl}");
                }
                return StatusCode(StatusCodes.Status404NotFound,
                    new ApiResponseModel<ShortenUrlDto>()
                    {
                        IsSuccess = false,
                        CreatedAt = DateTime.Now,
                        LongUrl = originalUrlDto.OriginalUrl,
                        StatusCode = 404,
                        Message = $"Invalid shorten url."
                    }
                );
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                        new ApiResponseModel<ShortenUrlDto>()
                        {
                            IsSuccess = false,
                            CreatedAt = DateTime.Now,
                            StatusCode = 500,
                            Message = $"Error while trying to go to this url."
                        }
                    );
            }
        }

        [HttpPost("[action]")]
        public IActionResult GetNewUrl([FromBody] UserUrlInputDto userUrlInputDto)
        {
            try { 
                if (!Uri.TryCreate(userUrlInputDto.InputUrl, UriKind.Absolute, out var result))
                {
                    return StatusCode(StatusCodes.Status400BadRequest,
                        new ApiResponseModel<UrlMapper>()
                        {
                            IsSuccess = false,
                            CreatedAt = DateTime.Now,
                            LongUrl = userUrlInputDto.InputUrl,
                            StatusCode = 400,
                            Message = "Invalid Url."
                        }
                    );
                }
                UrlMapper urlMapper = _urlRepository.UpdateShortenUrl(userUrlInputDto);
                string shortUrl = CreateUrl(urlMapper.ShortenUrl);
                urlMapper.ShortenUrl = shortUrl;
                return StatusCode(StatusCodes.Status200OK,
                        new ApiResponseModel<UrlMapper>()
                        {
                            IsSuccess = true,
                            CreatedAt = DateTime.Now,
                            LongUrl = userUrlInputDto.InputUrl,
                            StatusCode = 200,
                            Value = urlMapper,
                            Message = $"Shorten url created successfully: ({shortUrl})."
                        }
                    );
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                        new ApiResponseModel<ShortenUrlDto>()
                        {
                            IsSuccess = false,
                            CreatedAt = DateTime.Now,
                            LongUrl = userUrlInputDto.InputUrl,
                            StatusCode = 500,
                            Message = $"Error while trying to create shorten url for url:({userUrlInputDto.InputUrl})."
                        }
                    );
            }
        }

        [HttpDelete("[action]")]
        public IActionResult DeleteShortenUrlByOriginalUrl(UserUrlInputDto userUrlInputDto)
        {
            try
            {
                var deletedRow = _urlRepository.DeleteShortenUrl(userUrlInputDto);
                if (userUrlInputDto == null || deletedRow==null)
                {
                    return StatusCode(StatusCodes.Status404NotFound,
                        new ApiResponseModel<UrlMapper>()
                        {
                            IsSuccess = true,
                            CreatedAt = DateTime.Now,
                            StatusCode = 404,
                            Message = $"No url found.",
                            Value = deletedRow
                        }
                    );
                }
                return StatusCode(StatusCodes.Status200OK,
                        new ApiResponseModel<UrlMapper>()
                        {
                            IsSuccess = true,
                            CreatedAt = DateTime.Now,
                            LongUrl = userUrlInputDto.InputUrl,
                            StatusCode = 200,
                            Message = $"Url:({userUrlInputDto.InputUrl}) deleted successfully.",
                            Value = deletedRow
                        }
                    );
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                        new ApiResponseModel<ShortenUrlDto>()
                        {
                            IsSuccess = false,
                            CreatedAt = DateTime.Now,
                            LongUrl = userUrlInputDto.InputUrl,
                            StatusCode = 500,
                            Message = $"Error while trying to delete shorten url for url:({userUrlInputDto.InputUrl})."
                        }
                    );
            }
        }

        [HttpDelete("[action]/{shortenUrl}")]
        public IActionResult DeleteShortenUrlByShortUrl([FromRoute]string shortenUrl)
        {
            try
            {
                ShortenUrlDto shortenUrlDto = new ShortenUrlDto { ShortenUrl = shortenUrl };
                var deletedRow = _urlRepository.DeleteShortenUrl(shortenUrlDto);
                if (shortenUrlDto == null || deletedRow==null)
                {
                    return StatusCode(StatusCodes.Status404NotFound,
                        new ApiResponseModel<UrlMapper>()
                        {
                            IsSuccess = true,
                            CreatedAt = DateTime.Now,
                            StatusCode = 404,
                            Message = $"No url found.",
                            Value = deletedRow
                        }
                    );
                }
                return StatusCode(StatusCodes.Status200OK,
                        new ApiResponseModel<UrlMapper>()
                        {
                            IsSuccess = true,
                            CreatedAt = DateTime.Now,
                            LongUrl = deletedRow.InputUrl,
                            StatusCode = 200,
                            Message = $"Url:({shortenUrlDto.ShortenUrl}) deleted successfully.",
                            Value = deletedRow
                        }
                    );
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                        new ApiResponseModel<ShortenUrlDto>()
                        {
                            IsSuccess = false,
                            CreatedAt = DateTime.Now,
                            StatusCode = 500,
                            Message = $"Error while trying to delete shorten url."
                        }
                    );
            }
        }
        private string CreateUrl(string key)
        {
            string url = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/{key}";
            return url;
        }

    }
}
