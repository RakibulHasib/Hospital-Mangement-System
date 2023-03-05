using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorMasterDetails.Shared
{
    public class TestEntry
    {
        public int TestEntryId { get; set; }
        [ForeignKey("Patient")]
        public int PatientId { get; set; }
        [ForeignKey("Test")]
        public int TestId { get; set; }

        //Navigation
        public virtual Patient? Patient { get; set; }
        public virtual Test? Test { get; set; }
    }
}
