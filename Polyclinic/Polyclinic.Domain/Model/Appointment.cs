namespace Polyclinic.Domain.Model;

/// <summary>
/// Класс, представляющий запись на прием.
/// </summary>
public class Appointment
{
    /// <summary>
    /// Уникальный идентификатор записи на прием.
    /// </summary>
    public required int Id { get; set; }

    /// <summary>
    /// ID пациента, связанного с записью.
    /// </summary>
    public required int PatientId { get; set; }

    /// <summary>
    /// Пациент, связанный с записью.
    /// </summary>
    public virtual Patient? Patient { get; set; }

    /// <summary>
    /// ID врача, связанного с записью.
    /// </summary>
    public required int DoctorId { get; set; }

    /// <summary>
    /// Врач, связанный с записью.
    /// </summary>
    public virtual Doctor? Doctor { get; set; }

    /// <summary>
    /// Дата и время приема.
    /// </summary>
    public required DateTime AppointmentDateTime { get; set; }

    /// <summary>
    /// Заключение врача.
    /// </summary>
    public string? Conclusion { get; set; }

    /// <summary>
    /// Статус пациента после приема (на лечении / здоров).
    /// </summary>
    public required string Status { get; set; }
}
