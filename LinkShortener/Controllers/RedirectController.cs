using LinkShortenerCore.Repository;
using Microsoft.AspNetCore.Mvc;

namespace LinkShortener.Controllers;

[ApiController, Route("/")]
public class RedirectController : ControllerBase
{
    private readonly IUrlRepository _urlRepository;

    public RedirectController(IUrlRepository urlRepository)
    {
        _urlRepository = urlRepository;
    }

    [HttpGet, Route("/{shortLink}")]
    public async Task<ActionResult> VisitByShortLink(string shortLink)
    {
        if (await _urlRepository.VisitUrl(shortLink) is not {  } fullUri)
            return new NotFoundResult();
        return new RedirectResult(fullUri);
    }
}