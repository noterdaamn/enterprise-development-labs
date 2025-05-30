using Polyclinic.Domain.Model;

namespace Polyclinic.Domain.Services;

/// <summary>
/// Интерфейс репозитория для работы с врачами.
/// </summary>
public interface IDoctorRepository : IRepository<Doctor, int>
{
    /// <summary>   
    /// Возвращает информацию о всех врачах, стаж работы которых не меньше 10 лет.
    /// </summary>
    /// <returns>Список строк с информацией о врачах.</returns>
    IList<string> GetDoctorsWithExperienceMoreThan10Years();

    /// <summary>
    /// Возвращает информацию о всех пациентах, записанных на прием к указанному врачу, упорядоченных по ФИО.
    /// </summary>
    /// <param name="doctorId">ID врача.</param>
    /// <returns>Список строк с информацией о пациентах.</returns>
    IList<string> GetPatientsByDoctor(int doctorId);

    /// <summary>
    /// Возвращает информацию о здоровых на настоящий момент пациентах.
    /// </summary>
    /// <returns>Список строк с информацией о пациентах.</returns>
    IList<string> GetHealthyPatients();

    /// <summary>
    /// Возвращает информацию о количестве приемов пациентов по врачам за последний месяц.
    /// </summary>
    /// <returns>Список кортежей, содержащих имя врача и количество приемов.</returns>
    IList<Tuple<string, int>> GetAppointmentsCountByDoctorLastMonth();

    /// <summary>
    /// Возвращает информацию о топ 5 наиболее распространенных заболеваниях среди пациентов.
    /// </summary>
    /// <returns>Список строк с информацией о заболеваниях.</returns>
    IList<string> GetTop5Diseases();

    /// <summary>
    /// Возвращает информацию о пациентах старше 30 лет, которые записаны на прием к нескольким врачам, упорядоченных по дате рождения.
    /// </summary>
    /// <returns>Список строк с информацией о пациентах.</returns>
    IList<string> GetPatientsOver30WithMultipleDoctors();
}
