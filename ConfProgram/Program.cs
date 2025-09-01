using ConfProgram.Services;

var builder = WebApplication.CreateBuilder(args);

// Добавляем поддержку Razor Pages и Blazor Server
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();


builder.Services.AddSingleton<ConferenceService>();

// === Telegram bot (до Build!) ===
builder.Services.Configure<TelegramOptions>(
    builder.Configuration.GetSection("Telegram"));
builder.Services.AddHttpClient<TelegramSender>(); // IHttpClientFactory + наш sender

// 2) Собираем приложение
var app = builder.Build();

// 3) Middleware-конвейер
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

// 4) Инициализация данных до Run() — это ок
var confService = app.Services.GetRequiredService<ConferenceService>();
await confService.LoadDataAsync();

// 5) Запуск
app.Run();