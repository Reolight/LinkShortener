using LinkShortenerCore.Repository;
using LinkShortenerCore.Services;
using Microsoft.AspNetCore.Mvc;

namespace LinkShortener.Controllers;

[ApiController]
[Route("/")]
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

    [HttpGet, Route("{shortLink}")]
    public async Task<ActionResult> GetShortUrl(string shortLink)
    {
        if (await _urlRepository.VisitUrl(shortLink) is not {  } fullUri)
            return new NotFoundResult();
        return new RedirectResult(fullUri);
    }

    [HttpPost, Route("shorten/{fullUrl}")]
    public async Task<ActionResult> AddShortLink(string fullUrl)
    {
        var urlDto = await _shortener.CreateShortLink(fullUrl);
        return urlDto == null
            ? BadRequest($"Short link for [{fullUrl}] is already created or collision occured")
            : Created("/", urlDto);
    }
}