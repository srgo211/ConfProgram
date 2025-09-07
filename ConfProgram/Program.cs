// Program.cs
using ConfProgram.Services;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

// 1) Сервисы
builder.Services.AddRazorPages();
builder.Services
    .AddServerSideBlazor()
    .AddHubOptions(o =>
    {
        // Если гоняете крупные диффы UI, можно поднять лимит
        o.MaximumReceiveMessageSize = 1024 * 1024; // 1 МБ (по необходимости)
        o.KeepAliveInterval = TimeSpan.FromSeconds(15);
        o.ClientTimeoutInterval = TimeSpan.FromSeconds(60);
    })
    .AddCircuitOptions(o =>
    {
        o.DetailedErrors = !builder.Environment.IsProduction();
    });

builder.Services.AddSingleton<ConferenceService>();

// === Telegram bot (регистрируем ДО Build) ===
builder.Services.Configure<TelegramOptions>(builder.Configuration.GetSection("Telegram"));
builder.Services.AddHttpClient<TelegramSender>();

// включаем раздачу статики
builder.Services.AddRazorComponents().AddInteractiveServerComponents();

var app = builder.Build();

// 2) ВАЖНО: принимаем форвардные заголовки ОТ ПРОКСИ
//    Ставим это РАНО, до редиректов/аутентификации/роутинга,
//    чтобы схема https правильно определялась за прокси.
var fwd = new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedFor,
    ForwardLimit = 2 // обычно достаточно; при цепочке прокси увеличьте
};

// Если ваш прокси НЕ локальный (не 127.0.0.1) —
// явно разрешите адреса прокси. Для быстрого теста можно очистить списки,
// но в продакшене лучше добавить конкретные IP прокси:
// fwd.KnownProxies.Add(IPAddress.Parse("X.X.X.X"));
fwd.KnownNetworks.Clear();
fwd.KnownProxies.Clear();

app.UseForwardedHeaders(fwd);

// 3) Остальной конвейер
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// 4) Маршрутизация Blazor Hub и fallback
app.MapBlazorHub();                 // даёт эндпоинт /_blazor (SignalR)
app.MapFallbackToPage("/_Host");    // страница-хост с <base href="~/">

// 5) Инициализация данных до Run()
var confService = app.Services.GetRequiredService<ConferenceService>();
await confService.LoadDataAsync();

app.Run();
