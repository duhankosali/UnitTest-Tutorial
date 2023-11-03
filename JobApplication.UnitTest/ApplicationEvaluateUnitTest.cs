using JobApplicationLibrary;
using JobApplicationLibrary.Models;
using static JobApplicationLibrary.ApplicationEvaluator;

namespace JobApplication.UnitTest
{
    public class ApplicationEvaluateUnitTest
    {
        // Method Name Pattern: UnitOfWork_Condition_ExpectedResult

        [Test] // NUnit 'Test Method' attribute
        public void ApplicationEvaluator_WithUnderAge_TransferredToAutoRejected()   
        {
            // Arrange
            ApplicationEvaluator evaluator = new();

            var form = new JobsApplication()
            {
                Applicant = new Applicant()
                {
                    Age = 17
                }
            };

            // Action
            var result = evaluator.Evaluate(form);

            // Assert
            Assert.AreEqual(result, ApplicationResult.AutoRejected);
        }
    }
}