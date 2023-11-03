using JobApplicationLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobApplicationLibrary
{
    public class ApplicationEvaluator
    {
        private const int minAge = 18;

        public ApplicationResult Evaluate(JobsApplication form)
        {
            if(form.Applicant.Age < minAge)
                return ApplicationResult.AutoRejected;
            
            return ApplicationResult.AutoAccepted;
        }

        public enum ApplicationResult
        {
            AutoRejected,
            TransferredToHR,
            TransferredToLead,
            TransferredToCTO,
            AutoAccepted
        }
    }
}
