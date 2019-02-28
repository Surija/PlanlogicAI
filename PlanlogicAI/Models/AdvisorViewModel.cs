using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlanlogicAI.Models
{
    public class AdvisorViewModel
    {
        public int AdvisorId { get; set; }
        public string AdvisorName { get; set; }

    }

    public class ClientBasicDetails
    {
        public BasicDetailsViewModel basicDetails { get; set; }
        public ClientViewModel client { get; set; }
    }

  
}
