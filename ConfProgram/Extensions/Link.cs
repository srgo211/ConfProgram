namespace ConfProgram.Extensions;

public class Link
{ 
    /// <summary>строим ссылку для презентации </summary>
    public static string? HrefForPresentation(string? presentationUrl)
    {
        if (string.IsNullOrWhiteSpace(presentationUrl))
            return null;

        if (IsAbsoluteHttpUrl(presentationUrl))
            return presentationUrl;

        // Безопасно берём только имя файла (чтобы не было ../)
        var safeName = Path.GetFileName(presentationUrl);
        if (string.IsNullOrWhiteSpace(safeName))
            return null;

        // Файлы лежат в wwwroot/presentations/
        return $"/presentations/{Uri.EscapeDataString(safeName)}";
    }

    /// <summary>проверяем, является ли строка абсолютным HTTP(S) URL </summary>
    private static bool IsAbsoluteHttpUrl(string value) =>
        Uri.TryCreate(value, UriKind.Absolute, out var uri) &&
        (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);

}
