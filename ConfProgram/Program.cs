using ConfProgram.Services;

var builder = WebApplication.CreateBuilder(args);

// Добавляем поддержку Razor Pages и Blazor Server
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();


builder.Services.AddSingleton<ConferenceService>();

var app = builder.Build();

// Настройки middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Подключение Razor Pages и Blazor Hub
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

// 👇 ДО app.Run():
var confService = app.Services.GetRequiredService<ConferenceService>();
await confService.LoadDataAsync();

app.Run();