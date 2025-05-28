using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Polyclinic.Domain.Model; // Добавлено
using Polyclinic.Domain.Services; // Добавлено
using Polyclinic.Domain.Data; // Добавлено

namespace Polyclinic.Domain.Tests;

/// <summary>
/// Класс с тестами для поликлиники.
/// </summary>
public class PolyclinicTests
{
    private readonly DoctorInMemoryRepository _repository;

    public PolyclinicTests()
    {
        _repository = new DoctorInMemoryRepository();
    }

    /// <summary>
    /// Тест метода, возвращающего всех врачей, стаж которых не менее 10 лет.
    /// </summary>
    [Fact]
    public void GetDoctorsWithExperienceMoreThan10Years_ReturnsCorrectDoctors()
    {
        var result = _repository.GetDoctorsWithExperienceMoreThan10Years();

        Assert.NotNull(result);
        Assert.NotEmpty(result);

        var expectedDoctors = DataSeeder.Doctors
            .Where(d => d.Experience >= 10)
            .ToList();

        foreach (var doctor in expectedDoctors)
        {
            var expectedInfo = $"Фамилия: {doctor.FullName}, Специализация: {doctor.Specialization}, Стаж: {doctor.Experience} лет";
            Assert.Contains(expectedInfo, result);
        }
    }

    /// <summary>
    /// Вывод информации о всех пациентах, записанных на прием к указанному врачу, упорядочить по ФИО.
    /// </summary>
    /// <param name="doctorId">ID врача</param>
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void GetPatientsByDoctor_ReturnsCorrectPatients(int doctorId)
    {
        var result = _repository.GetPatientsByDoctor(doctorId);

        Assert.NotNull(result);
        Assert.NotEmpty(result);

        var patients = DataSeeder.Appointments
            .Where(a => a.DoctorId == doctorId)
            .Select(a => a.Patient)
            .OrderBy(p => p.FullName)
            .ToList();

        foreach (var patient in patients)
        {
            var expectedInfo = $"Пациент: {patient.FullName}, Паспорт: {patient.PassportNumber}, Год рождения: {patient.BirthYear}";
            Assert.Contains(expectedInfo, result);
        }
    }

    /// <summary>
    /// Вывод информации о здоровых на настоящий момент пациентах.
    /// </summary>
    [Fact]
    public void GetHealthyPatients_ReturnsCorrectPatients()
    {
        var result = _repository.GetHealthyPatients();

        Assert.NotNull(result);
        Assert.NotEmpty(result);

        var healthyPatients = DataSeeder.Appointments
            .Where(a => a.Status == "Здоров")
            .Select(a => a.Patient)
            .Distinct()
            .ToList();

        foreach (var patient in healthyPatients)
        {
            var expectedInfo = $"Пациент: {patient.FullName}, Паспорт: {patient.PassportNumber}, Год рождения: {patient.BirthYear}";
            Assert.Contains(expectedInfo, result);
        }
    }

    /// <summary>
    /// Вывод информации о количестве приемов пациентов по врачам за последний месяц.
    /// </summary>
    [Fact]
    public void GetAppointmentsCountByDoctorLastMonth_ReturnsCorrectCounts()
    {
        var result = _repository.GetAppointmentsCountByDoctorLastMonth();

        Assert.NotNull(result);
        Assert.NotEmpty(result);

        var lastMonth = DateTime.Now.AddMonths(-1);
        var appointmentsByDoctor = DataSeeder.Appointments
            .Where(a => a.AppointmentDateTime >= lastMonth)
            .GroupBy(a => a.DoctorId)
            .Select(g => new { DoctorId = g.Key, Count = g.Count() })
            .ToList();

        foreach (var appointment in appointmentsByDoctor)
        {
            var doctor = DataSeeder.Doctors.FirstOrDefault(d => d.Id == appointment.DoctorId);
            var expectedInfo = $"Врач: {doctor.FullName}, Количество приёмов: {appointment.Count}";
            Assert.Contains(expectedInfo, result);
        }
    }

    /// <summary>
    /// Вывести информацию о топ 5 наиболее распространенных заболеваниях среди пациентов.
    /// </summary>
    [Fact]
    public void GetTop5Diseases_ReturnsTop5Diseases()
    {
        var result = _repository.GetTop5Diseases();

        Assert.NotNull(result);
        Assert.True(result.Count <= 5);

        var topDiseases = DataSeeder.Appointments
            .GroupBy(a => a.Conclusion)
            .OrderByDescending(g => g.Count())
            .Take(5)
            .Select(g => g.Key)
            .ToList();

        foreach (var disease in topDiseases)
        {
            Assert.Contains(disease, result);
        }
    }

    /// <summary>
    /// Вывод информации о пациентах старше 30 лет, которые записаны на прием к нескольким врачам, упорядочить по дате рождения.
    /// </summary>
    [Fact]
    public void GetPatientsOver30WithMultipleDoctors_ReturnsCorrectPatients()
    {
        var result = _repository.GetPatientsOver30WithMultipleDoctors();

        Assert.NotNull(result);
        Assert.NotEmpty(result);

        var currentYear = DateTime.Now.Year;
        var patientsOver30 = DataSeeder.Patients
            .Where(p => currentYear - p.BirthYear > 30)
            .Where(p => p.Appointments.Select(a => a.DoctorId).Distinct().Count() > 1)
            .OrderBy(p => p.BirthYear)
            .ToList();

        foreach (var patient in patientsOver30)
        {
            var expectedInfo = $"Пациент: {patient.FullName}, Год рождения: {patient.BirthYear}, Адрес: {patient.Address}";
            Assert.Contains(expectedInfo, result);
        }
    }
}