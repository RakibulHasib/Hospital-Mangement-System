using BlazorApp.Server.Models;
using BlazorMasterDetails.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MasterDetailController : ControllerBase
    {
        private readonly PatientDbContext _context;
        private readonly IWebHostEnvironment _env;

        public MasterDetailController(PatientDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            this._env = env;
        }

        [HttpGet]
        [Route("GetTests")]
        public async Task<ActionResult<IEnumerable<Test>>> GetTests()
        {
            return await _context.Tests.ToListAsync();
        }

        [HttpGet]
        [Route("GetPatient")]
        public async Task<ActionResult<IEnumerable<Patient>>> GetPatient()
        {
            return await _context.Patients.Include(c => c.TestEntries).ThenInclude(b => b.Test).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<Patient> GetPatientById(int id)
        {
            return await _context.Patients.Where(x => x.PatientId == id).Include(c => c.TestEntries).ThenInclude(b => b.Test).FirstOrDefaultAsync();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] PatientVM patientVM)
        {

            if (ModelState.IsValid)
            {
                Patient patient = new Patient()
                {
                    PatientName = patientVM.PatientName,
                    BirthDate = patientVM.BirthDate,
                    Phone = patientVM.Phone,
                    MaritialStatus = patientVM.MaritialStatus
                };

                //Image
                if (patientVM.PictureFile is not null)
                {
                    string webroot = _env.WebRootPath;
                    string folder = "Images";
                    string imgFileName = Guid.NewGuid().ToString() + Path.GetExtension(patientVM.PictureFile.FileName);
                    string fileToWrite = Path.Combine(webroot, folder, imgFileName);

                    using (var stream = new FileStream(fileToWrite, FileMode.Create))
                    {
                        await patientVM.PictureFile.CopyToAsync(stream);
                        patient.Picture = imgFileName;
                    }
                }
                _context.Patients.Add(patient);

                if (patientVM.TestList.Count() > 0)
                {
                    foreach (Test test in patientVM.TestList)
                    {
                        _context.TestEntries.Add(new TestEntry
                        {
                            Patient = patient,
                            PatientId = patient.PatientId,
                            TestId = test.TestId
                        });
                    }
                }


                await _context.SaveChangesAsync();
                return Ok(patient);
            }
            return BadRequest();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromForm] PatientVM patientVM)
        {

            if (ModelState.IsValid)
            {
                Patient patient = _context.Patients.Find(patientVM.PatientId);
                patient.PatientName = patientVM.PatientName;
                patient.BirthDate = patientVM.BirthDate;
                patient.Phone = patientVM.Phone;
                patient.MaritialStatus = patientVM.MaritialStatus;

                //Image
                if (patientVM.PictureFile is not null)
                {
                    string webroot = _env.WebRootPath;
                    string folder = "Images";
                    string imgFileName = Guid.NewGuid().ToString() + Path.GetExtension(patientVM.PictureFile.FileName);
                    string fileToWrite = Path.Combine(webroot, folder, imgFileName);

                    using (var stream = new FileStream(fileToWrite, FileMode.Create))
                    {
                        await patientVM.PictureFile.CopyToAsync(stream);
                        patient.Picture = imgFileName;
                    }
                }

                var existsTests = _context.TestEntries.Where(x => x.PatientId == patientVM.PatientId).ToList();
                if (existsTests is not null)
                {
                    foreach (var entry in existsTests)
                    {
                        _context.Remove(entry);
                    }
                }


                if (patientVM.TestList.Count() > 0)
                {
                    foreach (Test test in patientVM.TestList)
                    {
                        _context.TestEntries.Add(new TestEntry
                        {
                            PatientId = patient.PatientId,
                            TestId = test.TestId
                        });
                    }
                }

                _context.Entry(patient).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(patient);
            }
            return NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            Patient patient = _context.Patients.Find(id);

            var existsTests = _context.TestEntries.Where(x => x.PatientId == patient.PatientId).ToList();
            if (existsTests is not null)
            {
                foreach (var entry in existsTests)
                {
                    _context.Remove(entry);
                }
            }
            _context.Remove(patient);
            try
            {
                await _context.SaveChangesAsync();

                return new OkObjectResult(patient);
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }
    }
}
