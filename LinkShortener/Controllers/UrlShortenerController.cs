using LinkShortenerCore.Repository;
using LinkShortenerCore.Services;
using Microsoft.AspNetCore.Mvc;

namespace LinkShortener.Controllers;

[ApiController]
[Route("/links")]
public class UrlShortenerController : ControllerBase
{
    private readonly ILogger<UrlShortenerController> _logger;
    private readonly IUrlRepository _urlRepository;
    private readonly IUrlShortenerService _shortener;
    public UrlShortenerController(ILogger<UrlShortenerController> logger,
        IUrlRepository urlRepository,
        IUrlShortenerService shortener)
    {
        _logger = logger;
        _urlRepository = urlRepository;
        _shortener = shortener;
    }

    [HttpGet]
    public async Task<ActionResult> GetUrls() 
        => Ok(await _urlRepository.GetAllUrlInfo());

    [HttpGet, Route("{shortUrl}")]
    public async Task<ActionResult> GetUrl(string shortUrl) =>
            await _urlRepository.GetUrlInfo(shortUrl) is not { } urlDto
                ? NotFound()
                : Ok(urlDto);

    [HttpPost]
    public async Task<ActionResult> AddShortLink([FromBody] string fullUrl)
    {
        var urlDto = await _shortener.CreateShortLink(fullUrl);
        return urlDto == null
            ? BadRequest($"Short link for [{fullUrl}] is already created or collision occured")
            : Created(string.Empty, urlDto);
    }

    [HttpDelete, Route("{shortUrl}")]
    public async Task<ActionResult> DeleteShortLink(string shortUrl)
    {
        return await _urlRepository.RemoveUrl(shortUrl)
            ? NoContent()
            : NotFound($"Short url [{shortUrl}] not found");
    }

    [HttpPut, Route("{shortUrl}")]
    public async Task<ActionResult> UpdateShortLink(string shortUrl, [FromBody] string newFullUrl) =>
        await _urlRepository.UpdateUrl(shortUrl, newFullUrl) is not { } updatedUrl 
            ? NotFound($"Url with short url [{shortUrl}] not found")
            : Accepted(updatedUrl);
}