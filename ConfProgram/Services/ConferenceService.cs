using ConfProgram.Models;
using System.Text.Json;

namespace ConfProgram.Services;

/// <summary>
/// Сервис загрузки программы конференции из JSON-файла.
/// </summary>
public class ConferenceService
{
    private readonly IWebHostEnvironment _env;
    private readonly string _jsonPath;

    /// <summary>Текущие данные конференции.</summary>
    public ConferenceData Data { get; private set; } = new();

    public ConferenceService(IWebHostEnvironment env)
    {
        _env = env ?? throw new ArgumentNullException(nameof(env));
        _jsonPath = Path.Combine(_env.WebRootPath, "data", "conference.json");
    }

    /// <summary>
    /// Загружает данные конференции из JSON-файла.
    /// </summary>
    public async Task<bool> LoadDataAsync()
    {
        if (!File.Exists(_jsonPath))
        {
            Console.WriteLine("❌ Файл conference.json не найден по пути: " + _jsonPath);
            return false;
        }

        try
        {
            var json = await File.ReadAllTextAsync(_jsonPath);
            var data = JsonSerializer.Deserialize<ConferenceData>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (data?.Rooms?.Any() == true)
            {
                Data = data;
                Console.WriteLine($"✅ Данные конференции успешно загружены. Комнат: {Data.Rooms.Count}");
                return true;
            }
            else
            {
                Console.WriteLine("⚠️ JSON загружен, но структура данных пуста или некорректна.");
            }
        }
        catch (JsonException ex)
        {
            Console.WriteLine("Ошибка разбора JSON: " + ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка загрузки JSON-файла: " + ex.Message);
        }

        return false;
    }

    /// <summary>
    /// Загружает тестовые данные конференции в память (без JSON-файла).
    /// </summary>
    public ConferenceData LoadMockData()
    {
        return new ConferenceData
        {
           
            Rooms = new List<ConferenceRoom>
            {
                new ConferenceRoom
                {
                    Id = "room1",
                    Title = "Тестовая Комната",
                    JoinUrl = "https://example.com/join",
                    Talks = new List<ConferenceTalk>
                    {
                        new ConferenceTalk
                        {
                            Time = "10:00 – 10:30",
                            Topic = "Тестовая тема",
                            Speaker = "Тестов Тест",
                            Organization = "ТестОрг",
                            PresentationUrl = "https://example.com/test.pdf",
                                   
                        }
                    }
                }
            }
                
            
        };
    }
}
