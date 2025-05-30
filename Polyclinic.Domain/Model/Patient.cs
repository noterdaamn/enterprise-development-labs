namespace Polyclinic.Domain.Model;

/// <summary>
/// Класс, представляющий пациента.
/// </summary>
public class Patient
{
    /// <summary>
    /// Уникальный идентификатор пациента.
    /// </summary>
    public required int Id { get; set; }

    /// <summary>
    /// Номер паспорта пациента.
    /// </summary>
    public required string PassportNumber { get; set; }

    /// <summary>
    /// Полное имя пациента.
    /// </summary>
    public required string FullName { get; set; }

    /// <summary>
    /// Год рождения пациента.
    /// </summary>
    public required int BirthYear { get; set; }

    /// <summary>
    /// Адрес пациента.
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// Список записей на прием, связанных с пациентом.
    /// </summary>
    public virtual List<Appointment>? Appointments { get; set; } = [];
}