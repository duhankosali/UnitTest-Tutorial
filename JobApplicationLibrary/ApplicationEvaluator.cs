﻿using JobApplicationLibrary.Models;
using JobApplicationLibrary.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobApplicationLibrary
{
    public class ApplicationEvaluator
    {
        // Let's assume there is data from the user.
        private const int minAge = 18;
        private const int yearsOfExperience = 5;
        private List<string> techStackList = new() { "C#", "RabbitMQ", "Microservices", "REST API" };

        // constructor
        private IIdentityValidator identityValidator;
        public ApplicationEvaluator(IIdentityValidator identityValidator)
        {
            this.identityValidator = identityValidator;
        }
            
        public ApplicationResult Evaluate(JobsApplication form)
        {
            if(form.Applicant.Age < minAge)
                return ApplicationResult.AutoRejected;

            // 2. Ders bitti, Bu method test standartlarımıza uymuyor. 3. video da Mock veri kullanacağız.
            bool validIdentity = identityValidator.IsValid(form.Applicant.identityNumber);
            if (!validIdentity)
                return ApplicationResult.TransferredToHR;
            

            int similarityRate = GetTechStackSimilarityRate(techStackList);
            if(similarityRate < 25)
                return ApplicationResult.AutoRejected;
            

            if(similarityRate > 75 && form.YearsOfExperience > yearsOfExperience)
                return ApplicationResult.AutoAccepted;
            

            return ApplicationResult.AutoAccepted;
        }

        private int GetTechStackSimilarityRate(List<string> techStacks)
        {
            var matchedCount = techStacks
                                    .Where(i => techStackList.Contains(i, StringComparer.OrdinalIgnoreCase))
                                    .Count();
            return (int)((double)matchedCount / techStackList.Count() * 100); // percentage
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
