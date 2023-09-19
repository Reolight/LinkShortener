using System.Security.Cryptography;
using System.Text;
using LinkShortenerCore.Model;
using LinkShortenerCore.Repository;

namespace LinkShortenerCore.Services;

public class UrlShortenerMd5 : IUrlShortenerService
{
    private readonly IUrlRepository _repository;

    public UrlShortenerMd5(IUrlRepository repository)
    {
        _repository = repository;
    }

    public async Task<UrlDto?> CreateShortLink(string fullUrl)
    {      
        using MD5 md5Encoder = MD5.Create();
        byte[] md5Link = md5Encoder.ComputeHash(Encoding.UTF8.GetBytes(fullUrl));
        if (BitConverter.IsLittleEndian)
            Array.Reverse(md5Link);
        char[] rawUri = new char[8];
        
        // меняю проблемный символ (/) на - (и + заодно)
        var shortUrl = new StringBuilder(Convert.ToBase64String(md5Link)[..8])
            .Replace('/', '-')
            .Replace('+', '_')
            .ToString();

        var urlDto = await _repository.AddShortUrl(shortUrl, fullUrl);
        return urlDto;
    }
}