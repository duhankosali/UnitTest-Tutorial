using JobApplicationLibrary;
using JobApplicationLibrary.Models;
using JobApplicationLibrary.Services;
using Moq;
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
            ApplicationEvaluator evaluator = new(null);

            var form = new JobsApplication()
            {
                Applicant = new Applicant() { Age = 15 },
                TechStackList = new List<string>() { "C#", "RabbitMQ", "Microservices", "REST API" },
                YearsOfExperience = 15
            };

            // Action
            var result = evaluator.Evaluate(form);

            // Assert
            Assert.AreEqual(result, ApplicationResult.AutoRejected);
        }

        [Test] // NUnit 'Test Method' attribute
        public void ApplicationEvaluator_WithNoTechStack_TransferredToAutoRejected()   
        {
            // Arrange
            var mockValidator = new Mock<IIdentityValidator>(); // Moq Library, creates a fake class consisting of the relevant interface.
            mockValidator.DefaultValue = DefaultValue.Mock;

            mockValidator.Setup(i => i.CountryDataProvider.CountryData.Country).Returns("TURKEY");
            mockValidator.Setup(i=>i.IsValid(It.IsAny<string>())).Returns(true);

            ApplicationEvaluator evaluator = new(mockValidator.Object);

            var form = new JobsApplication()
            {
                Applicant = new Applicant() { Age = 20 },
                TechStackList = new List<string>() { "" }
            };

            // Action
            var result = evaluator.Evaluate(form);

            // Assert
            Assert.AreEqual(result, ApplicationResult.AutoRejected);
        }

        [Test] // NUnit 'Test Method' attribute
        public void ApplicationEvaluator_WithTechStackOver75_TransferredToAutoAccepted()
        {
            // Arrange
            var mockValidator = new Mock<IIdentityValidator>(); // Moq Library, creates a fake class consisting of the relevant interface.
            mockValidator.DefaultValue = DefaultValue.Mock;

            mockValidator.Setup(i => i.CountryDataProvider.CountryData.Country).Returns("TURKEY");
            mockValidator.Setup(i => i.IsValid(It.IsAny<string>())).Returns(true);

            ApplicationEvaluator evaluator = new(mockValidator.Object);

            var form = new JobsApplication()
            {
                Applicant = new Applicant() {  Age = 20 },
                TechStackList = new List<string>() { "C#", "RabbitMQ", "Microservices", "REST API" },
                YearsOfExperience = 15
            };

            // Action
            var result = evaluator.Evaluate(form);

            // Assert
            Assert.AreEqual(result, ApplicationResult.AutoAccepted);
        }

        [Test]
        public void ApplicationEvaluator_WithInValidIdentityNumber_TransferredToHR()
        {
            // Arrange
            var mockValidator = new Mock<IIdentityValidator>(); // Moq Library, creates a fake class consisting of the relevant interface.
            mockValidator.DefaultValue = DefaultValue.Mock;

            mockValidator.Setup(i => i.CountryDataProvider.CountryData.Country).Returns("TURKEY");
            mockValidator.Setup(i => i.IsValid(It.IsAny<string>())).Returns(false);

            ApplicationEvaluator evaluator = new(mockValidator.Object);

            var form = new JobsApplication()
            {
                Applicant = new Applicant() { Age = 20 }
            };

            // Action
            var result = evaluator.Evaluate(form);

            // Assert
            Assert.AreEqual(result, ApplicationResult.TransferredToHR);
        }

        [Test]
        public void ApplicationEvaluator_WithInValidIdentityNumber_TransferredToCTO()   
        {
            // Arrange
            var mockValidator = new Mock<IIdentityValidator>();
            mockValidator.Setup(i => i.CountryDataProvider.CountryData.Country).Returns("SPAIN"); // Moq Hierarchy !!

            ApplicationEvaluator evaluator = new(mockValidator.Object);

            var form = new JobsApplication()
            {
                Applicant = new Applicant { Age = 20 }
            };

            // Action
            var result = evaluator.Evaluate(form);

            // Assert
            Assert.AreEqual(result, ApplicationResult.TransferredToCTO);
        }
    }
}