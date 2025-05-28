using System;
using System.Collections.Generic;
using System.Linq;
using Polyclinic.Domain.Model;

namespace Polyclinic.Domain.Data
{
    /// <summary>
    /// Класс для заполнения коллекций данными.
    /// </summary>
    public static class DataSeeder
    {
        /// <summary>
        /// Список пациентов.
        /// </summary>
        public static readonly List<Patient> Patients = new()
        {
            new Patient
            {
                Id = 1, // Уникальный идентификатор пациента
                PassportNumber = "9876543210",
                FullName = "Алексей Смирнов Иванович",
                BirthYear = 1992,
                Address = "пр. Победы, 15"
            },
            new Patient
            {
                Id = 2, // Уникальный идентификатор пациента
                PassportNumber = "1234567891",
                FullName = "Иванова Мария Петровна",
                BirthYear = 1985,
                Address = "ул. Мира, 25"
            },
            new Patient
            {
                Id = 3, // Уникальный идентификатор пациента
                PassportNumber = "1928374655",
                FullName = "Петров Игорь Сергеевич",
                BirthYear = 1980,
                Address = "ул. Ленина, 50"
            },
            new Patient
            {
                Id = 4, // Уникальный идентификатор пациента
                PassportNumber = "5647382910",
                FullName = "Кузнецова Ольга Александровна",
                BirthYear = 1982,
                Address = "ул. Чехова, 40"
            }
        };

        /// <summary>
        /// Список врачей.
        /// </summary>
        public static readonly List<Doctor> Doctors = new()
        {
            new Doctor
            {
                Id = 1, // Уникальный идентификатор врача
                PassportNumber = "3344556677",
                FullName = "Смирнова Ольга Владимировна",
                BirthYear = 1975,
                Specialization = "Терапевт",
                Experience = 20
            },
            new Doctor
            {
                Id = 2, // Уникальный идентификатор врача
                PassportNumber = "4455667788",
                FullName = "Васильев Алексей Иванович",
                BirthYear = 1990,
                Specialization = "Хирург",
                Experience = 8
            },
            new Doctor
            {
                Id = 3, // Уникальный идентификатор врача
                PassportNumber = "5566778899",
                FullName = "Козлов Михаил Сергеевич",
                BirthYear = 1985,
                Specialization = "Кардиолог",
                Experience = 15
            }
        };

        /// <summary>
        /// Список записей на прием.
        /// </summary>
        public static readonly List<Appointment> Appointments = new()
        {
            new Appointment
            {
                Id = 1, // Уникальный идентификатор записи на прием
                PatientId = 1,
                DoctorId = 1,
                AppointmentDateTime = new DateTime(2025, 11, 15, 10, 0, 0),
                Conclusion = "Здоров",
                Status = "Здоров"
            },
            new Appointment
            {
                Id = 2, // Уникальный идентификатор записи на прием
                PatientId = 2,
                DoctorId = 1,
                AppointmentDateTime = new DateTime(2025, 11, 16, 11, 0, 0),
                Conclusion = "На лечении",
                Status = "На лечении"
            },
            new Appointment
            {
                Id = 3, // Уникальный идентификатор записи на прием
                PatientId = 3,
                DoctorId = 2,
                AppointmentDateTime = new DateTime(2025, 11, 17, 12, 0, 0),
                Conclusion = "Здоров",
                Status = "Здоров"
            },
            new Appointment
            {
                Id = 4, // Уникальный идентификатор записи на прием
                PatientId = 4,
                DoctorId = 1,
                AppointmentDateTime = new DateTime(2025, 11, 18, 13, 0, 0),
                Conclusion = "На лечении",
                Status = "На лечении"
            }
        };

        /// <summary>
        /// Статический конструктор.
        /// </summary>
        static DataSeeder()
        {
            // Связываем пациентов с их записями на прием
            foreach (var appointment in Appointments)
            {
                appointment.Patient = Patients.FirstOrDefault(p => p.Id == appointment.PatientId);
                appointment.Doctor = Doctors.FirstOrDefault(d => d.Id == appointment.DoctorId);
            }

            // Связываем пациентов с их записями на прием
            foreach (var patient in Patients)
            {
                patient.Appointments = Appointments.Where(a => a.PatientId == patient.Id).ToList();
            }

            // Связываем врачей с их записями на прием
            foreach (var doctor in Doctors)
            {
                doctor.Appointments = Appointments.Where(a => a.DoctorId == doctor.Id).ToList();
            }
        }
    }
}