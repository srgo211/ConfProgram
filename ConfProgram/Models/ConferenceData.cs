namespace ConfProgram.Models;

/// <summary>Полные данные всей конференции.</summary>
public class ConferenceData
{
    /// <summary>Список всех комнат</summary>
    public List<ConferenceRoom> Rooms { get; set; } = [];
}

/// <summary>Комната или поток в рамках дня.</summary>
public class ConferenceRoom
{
    /// <summary>Идентификатор комнаты (например, "room1")</summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>Ссылка на подключение к конференции</summary>
    public string JoinUrl { get; set; } = string.Empty;

    /// <summary>Название комнаты (например, "СМЕТНЫЕ НОРМАТИВЫ...")</summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>Список докладов в комнате.</summary>
    public List<ConferenceTalk> Talks { get; set; } = [];
}

/// <summary>Отдельный доклад/мероприятие.</summary>
public class ConferenceTalk
{
    /// <summary>Время проведения доклада.</summary>
    public string Time { get; set; } = string.Empty;

    /// <summary>Название темы доклада.</summary>
    public string Topic { get; set; } = string.Empty;

    /// <summary>ФИО докладчика.</summary>
    public string Speaker { get; set; } = string.Empty;

    /// <summary>Организация, которую представляет докладчик.</summary>
    public string Organization { get; set; } = string.Empty;

    /// <summary>Ссылка на презентацию (PDF, PPT и т.п.).</summary>
    public string PresentationUrl { get; set; } = string.Empty;

  
}
