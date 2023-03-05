using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorMasterDetails.Shared
{
    public class Patient
    {
        public int PatientId { get; set; }
        public string PatientName { get; set; } = default!;
        public DateTime BirthDate { get; set; }
        public int Phone { get; set; }
        public string? Picture { get; set; } = null!;
        public bool MaritialStatus { get; set; }
        //Nev
        public virtual ICollection<TestEntry>? TestEntries { get; set; } = new List<TestEntry>();
    }
}
