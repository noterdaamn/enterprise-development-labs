using Polyclinic.Domain.Data;
using Polyclinic.Domain.Services.InMemory;


namespace Polyclinic.Domain.Tests
{
    public class PolyclinicTests
    {
        private readonly DoctorInMemoryRepository _doctorRepository;
        private readonly PatientInMemoryRepository _patientRepository;
        private readonly AppointmentInMemoryRepository _appointmentRepository;

        public PolyclinicTests()
        {
            _doctorRepository = new DoctorInMemoryRepository();
            _patientRepository = new PatientInMemoryRepository();
            _appointmentRepository = new AppointmentInMemoryRepository();
        }

        /// <summary>
        /// ���� ������, ������������� ���� ������, ���� ������� �� ����� 10 ���.
        /// </summary>
        [Fact]
        public async Task GetDoctorsWithExperienceMoreThan10Years_ReturnsCorrectDoctors()
        {
            // Act
            var result = await _doctorRepository.GetDoctorsWithExperience(10);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);

            var expectedDoctors = DataSeeder.Doctors
                .Where(d => d.Experience >= 10)
                .ToList();

            Assert.Equal(expectedDoctors.Count, result.Count);
            foreach (var doctor in expectedDoctors)
            {
                Assert.Contains(result, d => d.Id == doctor.Id);
            }
        }

        /// <summary>
        /// ���� ������, ������������� ��������� ���������� �����, ������������� �� ���.
        /// </summary>
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task GetPatientsByDoctor_ReturnsCorrectPatients(int doctorId)
        {
            // Act
            var result = await _patientRepository.GetPatientsByDoctor(doctorId);

            // Assert
            Assert.NotNull(result);

            var expectedPatients = DataSeeder.Appointments
                .Where(a => a.DoctorId == doctorId)
                .Join(DataSeeder.Patients,
                    a => a.PatientId,
                    p => p.Id,
                    (a, p) => p)
                .OrderBy(p => p.FullName)
                .ToList();

            Assert.Equal(expectedPatients.Count, result.Count);
            for (int i = 0; i < expectedPatients.Count; i++)
            {
                Assert.Equal(expectedPatients[i].Id, result[i].Id);
            }
        }

        /// <summary>
        /// ���� ������, ������������� �������� ���������.
        /// </summary>
        [Fact]
        public async Task GetHealthyPatients_ReturnsCorrectPatients()
        {
            // Act
            var result = await _patientRepository.GetHealthyPatients();

            // Assert
            Assert.NotNull(result);

            var expectedPatients = DataSeeder.Appointments
                .Where(a => a.Status == "������")
                .Join(DataSeeder.Patients,
                    a => a.PatientId,
                    p => p.Id,
                    (a, p) => p)
                .Distinct()
                .ToList();

            Assert.Equal(expectedPatients.Count, result.Count);
            foreach (var patient in expectedPatients)
            {
                Assert.Contains(result, p => p.Id == patient.Id);
            }
        }

        /// <summary>
        /// ���� ������, ������������� ���������� ������� �� ������.
        /// </summary>
        [Fact]
        public async Task GetAppointmentsCountByDoctors_ReturnsCorrectCounts()
        {
            // Act
            var result = await _appointmentRepository.GetAppointmentStatisticsByStatus();

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);

            var expectedStats = DataSeeder.Appointments
                .GroupBy(a => a.Status)
                .ToDictionary(g => g.Key, g => g.Count());

            Assert.Equal(expectedStats.Count, result.Count);
            foreach (var stat in expectedStats)
            {
                Assert.True(result.ContainsKey(stat.Key));
                Assert.Equal(stat.Value, result[stat.Key]);
            }
        }

        /// <summary>
        /// ���� ������, ������������� ���-5 �����������.
        /// </summary>
        [Fact]
        public async Task GetTop5CommonDiseases_ReturnsTop5Diseases()
        {
            // Act
            var result = await _appointmentRepository.GetAppointmentStatisticsByStatus();

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Count <= 5);

            var expectedTop5 = DataSeeder.Appointments
                .GroupBy(a => a.Conclusion)
                .OrderByDescending(g => g.Count())
                .Take(5)
                .ToDictionary(g => g.Key, g => g.Count());

            foreach (var disease in expectedTop5)
            {
                Assert.True(result.ContainsKey(disease.Key));
                Assert.Equal(disease.Value, result[disease.Key]);
            }
        }

        /// <summary>
        /// ���� ������, ������������� ��������� ������ 30 ��� � ����������� �������.
        /// </summary>
        [Fact]
        public async Task GetPatientsOver30WithMultipleDoctors_ReturnsCorrectPatients()
        {
            // Act
            var result = await _patientRepository.GetPatientsWithMultipleDoctors();

            // Assert
            Assert.NotNull(result);

            var currentYear = DateTime.Now.Year;
            var expectedPatients = DataSeeder.Appointments
                .GroupBy(a => a.PatientId)
                .Where(g => g.Select(a => a.DoctorId).Distinct().Count() > 1)
                .Join(DataSeeder.Patients,
                    g => g.Key,
                    p => p.Id,
                    (g, p) => p)
                .Where(p => (currentYear - p.BirthYear) > 30)
                .OrderBy(p => p.BirthYear)
                .ToList();

            Assert.Equal(expectedPatients.Count, result.Count);
            for (int i = 0; i < expectedPatients.Count; i++)
            {
                Assert.Equal(expectedPatients[i].Id, result[i].Id);
            }
        }

       
    }
}