using JobApplicationLibrary;
using JobApplicationLibrary.Models;
using JobApplicationLibrary.Services;
using Moq;
using static JobApplicationLibrary.ApplicationEvaluator;
using FluentAssertions;

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
            //Assert.AreEqual(ApplicationResult.AutoRejected, result);
            result.Should().Be(ApplicationResult.AutoRejected);
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
            //Assert.AreEqual(ApplicationResult.AutoRejected, result);
            result.Should().Be(ApplicationResult.AutoRejected);
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
            //Assert.AreEqual(ApplicationResult.AutoAccepted, result);
            result.Should().Be(ApplicationResult.AutoAccepted);
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
            //Assert.AreEqual(ApplicationResult.TransferredToHR, result);
            result.Should().Be(ApplicationResult.TransferredToHR);
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
            //Assert.AreEqual(ApplicationResult.TransferredToCTO, result);
            result.Should().Be(ApplicationResult.TransferredToCTO);
        }

        [Test]
        public void ApplicationEvaluator_WithOverAge_ValidationModeToDetailed()
        {
            // Arrange
            var mockValidator = new Mock<IIdentityValidator>();

            //mockValidator.SetupAllProperties(); --> Eðer birden fazla setlenmesi gereken varsa bunu kullanabiliriz.
            mockValidator.SetupProperty(i => i.ValidationMode);
            mockValidator.Setup(i => i.CountryDataProvider.CountryData.Country).Returns("SPAIN"); // Moq Hierarchy !!

            var evaluator = new ApplicationEvaluator(mockValidator.Object);

            var form = new JobsApplication()
            {
                Applicant = new Applicant
                {
                    Age = 51
                }
            };

            // Action
            var result = evaluator.Evaluate(form);

            // Assert
            //Assert.AreEqual(ValidationMode.Detailed, mockValidator.Object.ValidationMode);
            mockValidator.Object.ValidationMode.Should().Be(ValidationMode.Detailed);
        }

        [Test]
        public void ApplicationEvaluator_WithNullApplicant_ThrowsArgumentNullException()
        {
            // Arrange
            var mockValidator = new Mock<IIdentityValidator>();

            var evaluator = new ApplicationEvaluator(mockValidator.Object);

            var form = new JobsApplication();

            // Action
            Action resultAction = () => evaluator.Evaluate(form);

            // Assert
            resultAction.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void ApplicationEvaluator_WithDefaultValue_IsValidCalled()
        {
            // Arrange
            var mockValidator = new Mock<IIdentityValidator>();
            mockValidator.DefaultValue = DefaultValue.Mock;
            mockValidator.Setup(i => i.CountryDataProvider.CountryData.Country).Returns("TURKEY");

            var evaluator = new ApplicationEvaluator(mockValidator.Object);

            var form = new JobsApplication()
            {
                Applicant = new Applicant
                {
                    Age = 20,
                    identityNumber = "1234"
                },
            };

            // Action
            var result = evaluator.Evaluate(form);

            // Assert
            mockValidator.Verify(i => i.IsValid(It.IsAny<string>()));
        }
    }
}