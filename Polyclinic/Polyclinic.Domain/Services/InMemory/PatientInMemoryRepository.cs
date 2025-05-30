using Polyclinic.Domain.Model;
using Polyclinic.Domain.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Polyclinic.Domain.Services.InMemory
{
    public class PatientInMemoryRepository : IRepository<Patient, int>
    {
        private List<Patient> _patients;
        private List<Doctor> _doctors;
        private List<Appointment> _appointments;

        public PatientInMemoryRepository()
        {
            _patients = new List<Patient>(DataSeeder.Patients);
            _doctors = new List<Doctor>(DataSeeder.Doctors);
            _appointments = new List<Appointment>(DataSeeder.Appointments);
        }

        public Task<Patient> Add(Patient entity)
        {
            try
            {
                entity.Id = _patients.Any() ? _patients.Max(p => p.Id) + 1 : 1;
                _patients.Add(entity);
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
                var patient = await Get(key);
                if (patient != null)
                    _patients.Remove(patient);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public async Task<Patient> Update(Patient entity)
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

        public Task<Patient?> Get(int key) =>
            Task.FromResult(_patients.FirstOrDefault(item => item.Id == key));

        public Task<IList<Patient>> GetAll() =>
            Task.FromResult((IList<Patient>)_patients);

        // Специфичные методы для пациентов
        public Task<IList<Patient>> GetPatientsByDoctor(int doctorId) =>
            Task.FromResult((IList<Patient>)_appointments
                .Where(a => a.DoctorId == doctorId)
                .Join(_patients,
                    a => a.PatientId,
                    p => p.Id,
                    (a, p) => p)
                .Distinct()
                .ToList());

        public Task<IList<Patient>> GetHealthyPatients() =>
            Task.FromResult((IList<Patient>)_appointments
                .Where(a => a.Status == "Здоров")
                .Join(_patients,
                    a => a.PatientId,
                    p => p.Id,
                    (a, p) => p)
                .Distinct()
                .ToList());

        public Task<IList<Patient>> GetPatientsOverAge(int age)
        {
            var currentYear = DateTime.Now.Year;
            return Task.FromResult((IList<Patient>)_patients
                .Where(p => (currentYear - p.BirthYear) > age)
                .ToList());
        }

        public Task<IList<Patient>> GetPatientsWithMultipleDoctors() =>
            Task.FromResult((IList<Patient>)_appointments
                .GroupBy(a => a.PatientId)
                .Where(g => g.Select(a => a.DoctorId).Distinct().Count() > 1)
                .Join(_patients,
                    g => g.Key,
                    p => p.Id,
                    (g, p) => p)
                .ToList());

        public Task<IList<Appointment>> GetPatientAppointments(int patientId) =>
            Task.FromResult((IList<Appointment>)_appointments
                .Where(a => a.PatientId == patientId)
                .ToList());

        IList<Patient> IRepository<Patient, int>.GetAll()
        {
            throw new NotImplementedException();
        }

        Patient? IRepository<Patient, int>.Get(int key)
        {
            throw new NotImplementedException();
        }

        bool IRepository<Patient, int>.Add(Patient entity)
        {
            throw new NotImplementedException();
        }

        bool IRepository<Patient, int>.Update(Patient entity)
        {
            throw new NotImplementedException();
        }

        bool IRepository<Patient, int>.Delete(int key)
        {
            throw new NotImplementedException();
        }
    }
}