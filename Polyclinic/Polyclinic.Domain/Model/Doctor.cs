namespace Polyclinic.Domain.Model;

/// <summary>
/// Класс, представляющий врача.
/// </summary>
public class Doctor
{
    /// <summary>
    /// Уникальный идентификатор врача.
    /// </summary>
    public required int Id { get; set; }

    /// <summary>
    /// Номер паспорта врача.
    /// </summary>
    public required string PassportNumber { get; set; }

    /// <summary>
    /// Полное имя врача.
    /// </summary>
    public required string FullName { get; set; }

    /// <summary>
    /// Год рождения врача.
    /// </summary>
    public required int BirthYear { get; set; }

    /// <summary>
    /// Специализация врача.
    /// </summary>
    public required string Specialization { get; set; }

    /// <summary>
    /// Стаж работы врача.
    /// </summary>
    public required int Experience { get; set; }

    /// <summary>
    /// Список записей на прием, связанных с врачом.
    /// </summary>
    public virtual List<Appointment>? Appointments { get; set; } = [];
}