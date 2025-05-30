using Polyclinic.Domain.Model;
using Polyclinic.Domain.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Polyclinic.Domain.Services.InMemory
{
    public class AppointmentInMemoryRepository : IRepository<Appointment, int>
    {
        private List<Appointment> _appointments;
        private List<Patient> _patients;
        private List<Doctor> _doctors;

        public AppointmentInMemoryRepository()
        {
            _appointments = new List<Appointment>(DataSeeder.Appointments);
            _patients = new List<Patient>(DataSeeder.Patients);
            _doctors = new List<Doctor>(DataSeeder.Doctors);
        }

        public Task<Appointment> Add(Appointment entity)
        {
            try
            {
                entity.Id = _appointments.Any() ? _appointments.Max(a => a.Id) + 1 : 1;
                _appointments.Add(entity);
                entity.Patient = _patients.FirstOrDefault(p => p.Id == entity.PatientId);
                entity.Doctor = _doctors.FirstOrDefault(d => d.Id == entity.DoctorId);
            }
            catch
            {
                return null!;
            }
            return Task.FromResult(entity);
        }

        public async Task<bool> Delete(int key)
        {
            try
            {
                var appointment = await Get(key);
                if (appointment != null)
                    _appointments.Remove(appointment);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public async Task<Appointment> Update(Appointment entity)
        {
            try
            {
                await Delete(entity.Id);
                await Add(entity);
            }
            catch
            {
                return null!;
            }
            return entity;
        }

        public Task<Appointment?> Get(int key) =>
            Task.FromResult(_appointments.FirstOrDefault(item => item.Id == key));

        public Task<IList<Appointment>> GetAll() =>
            Task.FromResult((IList<Appointment>)_appointments);

        // Специфичные методы для записей на прием
        public Task<IList<Appointment>> GetAppointmentsByDoctor(int doctorId) =>
            Task.FromResult((IList<Appointment>)_appointments
                .Where(a => a.DoctorId == doctorId)
                .OrderBy(a => a.AppointmentDateTime)
                .ToList());

        public Task<IList<Appointment>> GetAppointmentsByPatient(int patientId) =>
            Task.FromResult((IList<Appointment>)_appointments
                .Where(a => a.PatientId == patientId)
                .OrderBy(a => a.AppointmentDateTime)
                .ToList());

        public Task<IList<Appointment>> GetAppointmentsByDateRange(DateTime startDate, DateTime endDate) =>
            Task.FromResult((IList<Appointment>)_appointments
                .Where(a => a.AppointmentDateTime >= startDate && a.AppointmentDateTime <= endDate)
                .OrderBy(a => a.AppointmentDateTime)
                .ToList());

        public Task<IList<Appointment>> GetAppointmentsByStatus(string status) =>
            Task.FromResult((IList<Appointment>)_appointments
                .Where(a => a.Status.Equals(status, StringComparison.OrdinalIgnoreCase))
                .ToList());

        public Task<IList<Appointment>> GetUpcomingAppointments(int daysAhead) =>
            Task.FromResult((IList<Appointment>)_appointments
                .Where(a => a.AppointmentDateTime >= DateTime.Now &&
                          a.AppointmentDateTime <= DateTime.Now.AddDays(daysAhead))
                .OrderBy(a => a.AppointmentDateTime)
                .ToList());

        public Task<Dictionary<string, int>> GetAppointmentStatisticsByStatus() =>
            Task.FromResult(_appointments
                .GroupBy(a => a.Status)
                .ToDictionary(g => g.Key, g => g.Count()));

        IList<Appointment> IRepository<Appointment, int>.GetAll()
        {
            throw new NotImplementedException();
        }

        Appointment? IRepository<Appointment, int>.Get(int key)
        {
            throw new NotImplementedException();
        }

        bool IRepository<Appointment, int>.Add(Appointment entity)
        {
            throw new NotImplementedException();
        }

        bool IRepository<Appointment, int>.Update(Appointment entity)
        {
            throw new NotImplementedException();
        }

        bool IRepository<Appointment, int>.Delete(int key)
        {
            throw new NotImplementedException();
        }
    }
}