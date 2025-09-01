using Microsoft.Extensions.Options;

namespace ConfProgram.Services;

public sealed class TelegramOptions
{
    public string BotToken { get; set; } = string.Empty;
    public string ChatId { get; set; } = string.Empty;
}

public sealed class TelegramSender
{
    private readonly HttpClient _http;
    private readonly TelegramOptions _opt;
    private readonly ILogger<TelegramSender> _log;

    public TelegramSender(HttpClient http, IOptions<TelegramOptions> opt, ILogger<TelegramSender> log)
    {
        _http = http;
        _opt = opt.Value;
        _log = log;
    }

    public async Task SendMessageAsync(string text, CancellationToken ct = default)
    {
        var url = $"https://api.telegram.org/bot{_opt.BotToken}/sendMessage";
        var payload = new
        {
            chat_id = _opt.ChatId,
            text,
            parse_mode = "HTML"
        };

        var resp = await _http.PostAsJsonAsync(url, payload, ct);
        if (!resp.IsSuccessStatusCode)
        {
            var body = await resp.Content.ReadAsStringAsync(ct);
            _log.LogError("Ошибка Telegram API: {Status} {Body}", resp.StatusCode, body);
            throw new InvalidOperationException($"Не удалось отправить сообщение в Telegram. {body}");
        }
    }
}