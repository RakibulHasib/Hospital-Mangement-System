using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorMasterDetails.Shared
{
    public class PatientVM
    {
        public int PatientId { get; set; }
        public string PatientName { get; set; } = default!;
        [Required, DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime BirthDate { get; set; } = DateTime.Now;
        public int Phone { get; set; }
        public string? Picture { get; set; }
        public IFormFile? PictureFile { get; set; }
        public bool MaritialStatus { get; set; }
        public List<Test> TestList { get; set; } = new List<Test>();

        public virtual ICollection<TestEntry>? TestEntries { get; set; } = new List<TestEntry>();

    }
}
