using Polyclinic.Domain.Model;
using Polyclinic.Domain.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Polyclinic.Domain.Services.InMemory
{
    public class DoctorInMemoryRepository : IRepository<Doctor, int>
    {
        private List<Doctor> _doctors;
        private List<Patient> _patients;
        private List<Appointment> _appointments;

        public DoctorInMemoryRepository()
        {
            _doctors = new List<Doctor>(DataSeeder.Doctors);
            _patients = new List<Patient>(DataSeeder.Patients);
            _appointments = new List<Appointment>(DataSeeder.Appointments);
        }

        public Task<Doctor> Add(Doctor entity)
        {
            try
            {
                entity.Id = _doctors.Any() ? _doctors.Max(d => d.Id) + 1 : 1;
                _doctors.Add(entity);
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
                var doctor = await Get(key);
                if (doctor != null)
                    _doctors.Remove(doctor);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public async Task<Doctor> Update(Doctor entity)
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

        public Task<Doctor?> Get(int key) =>
            Task.FromResult(_doctors.FirstOrDefault(item => item.Id == key));

        public Task<IList<Doctor>> GetAll() =>
            Task.FromResult((IList<Doctor>)_doctors);

        // Специфичные методы для врачей
        public Task<IList<Doctor>> GetDoctorsBySpecialization(string specialization) =>
            Task.FromResult((IList<Doctor>)_doctors
                .Where(d => d.Specialization.Equals(specialization, StringComparison.OrdinalIgnoreCase))
                .ToList());

        public Task<IList<Doctor>> GetDoctorsWithExperience(int minYears) =>
            Task.FromResult((IList<Doctor>)_doctors
                .Where(d => d.Experience >= minYears)
                .OrderByDescending(d => d.Experience)
                .ToList());

        public Task<IList<Appointment>> GetDoctorAppointments(int doctorId) =>
            Task.FromResult((IList<Appointment>)_appointments
                .Where(a => a.DoctorId == doctorId)
                .OrderBy(a => a.AppointmentDateTime)
                .ToList());

        public Task<int> GetDoctorPatientCount(int doctorId) =>
            Task.FromResult(_appointments
                .Where(a => a.DoctorId == doctorId)
                .Select(a => a.PatientId)
                .Distinct()
                .Count());

        IList<Doctor> IRepository<Doctor, int>.GetAll()
        {
            throw new NotImplementedException();
        }

        Doctor? IRepository<Doctor, int>.Get(int key)
        {
            throw new NotImplementedException();
        }

        bool IRepository<Doctor, int>.Add(Doctor entity)
        {
            throw new NotImplementedException();
        }

        bool IRepository<Doctor, int>.Update(Doctor entity)
        {
            throw new NotImplementedException();
        }

        bool IRepository<Doctor, int>.Delete(int key)
        {
            throw new NotImplementedException();
        }
    }
}