using System;
using System.Collections.Generic;

namespace PlanlogicAI.Data
{
    public partial class BasicDetails
    {
        public int ClientId { get; set; }
        public string FamilyName { get; set; }
        public string ClientName { get; set; }
        public DateTime ClientDob { get; set; }
        public string ClientEmpStatus { get; set; }
        public int? ClientRetirementYear { get; set; }
        public string ClientRiskProfile { get; set; }
        public string ClientPrivateHealthInsurance { get; set; }
        public string MaritalStatus { get; set; }
        public string PartnerName { get; set; }
        public DateTime? PartnerDob { get; set; }
        public string PartnerEmpStatus { get; set; }
        public int? PartnerRetirementYear { get; set; }
        public string PartnerRiskProfile { get; set; }
        public string JointRiskProfile { get; set; }
        public string PartnerPrivateHealthInsurance { get; set; }
        public int StartDate { get; set; }
        public int Period { get; set; }
        public int? NoOfDependents { get; set; }
    }
}
