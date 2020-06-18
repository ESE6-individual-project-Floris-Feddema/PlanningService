using System;
using System.Collections.Generic;
using PlanningService.Exceptions;
using PlanningService.Helpers;
using PlanningService.Models;
using Xunit;

namespace PlanningServiceTests.Helpers
{
    public class ExtensionsTest
    {
        [Fact]
        public void ValidatePlanningSuccess()
        {
            //Arrange
            var planning = new Planning
            {
                id = Guid.NewGuid(),
                CompanyId = "id",
                Name = "test",
                StartDate = DateTime.Now.AddDays(5),
                EndDate = DateTime.Now.AddDays(25),
                WorkDays = new List<WorkDay>()
                {
                    new WorkDay
                    {
                        id = Guid.NewGuid(),
                        Date = DateTime.Now.AddDays(7),
                    },
                    new WorkDay
                    {
                        id = Guid.NewGuid(),
                        Date = DateTime.Now.AddDays(8),
                    }
                }
            };


            //Act
            planning.Validate();

            //Assert
            Assert.True(true);
        }

        [Fact]
        public void ValidatePlanningNoId()
        {
            //Arrange
            var planning = new Planning
            {
                id = Guid.Empty,
                CompanyId = "id",
                Name = "test",
                StartDate = DateTime.Now.AddDays(5),
                EndDate = DateTime.Now.AddDays(25),
                WorkDays = new List<WorkDay>()
                {
                    new WorkDay
                    {
                        id = Guid.NewGuid(),
                        Date = DateTime.Now.AddDays(7),
                    },
                    new WorkDay
                    {
                        id = Guid.NewGuid(),
                        Date = DateTime.Now.AddDays(8),
                    }
                }
            };

            //Act
            var result = Assert.Throws<ValidationException>(() => planning.Validate());

            //Assert
            Assert.IsType<ValidationException>(result);
            Assert.NotEqual(0, result.Message.Length);
        }

        [Fact]
        public void ValidatePlanningNoCompanyId()
        {
            //Arrange
            var planning = new Planning
            {
                id = Guid.NewGuid(),
                CompanyId = "",
                Name = "test",
                StartDate = DateTime.Now.AddDays(5),
                EndDate = DateTime.Now.AddDays(25),
                WorkDays = new List<WorkDay>()
                {
                    new WorkDay
                    {
                        id = Guid.NewGuid(),
                        Date = DateTime.Now.AddDays(7),
                    },
                    new WorkDay
                    {
                        id = Guid.NewGuid(),
                        Date = DateTime.Now.AddDays(8),
                    }
                }
            };


            //Act
            var result = Assert.Throws<ValidationException>(() => planning.Validate());

            //Assert
            Assert.IsType<ValidationException>(result);
            Assert.NotEqual(0, result.Message.Length);
        }


        [Fact]
        public void ValidatePlanningNoName()
        {
            //Arrange
            var planning = new Planning
            {
                id = Guid.NewGuid(),
                CompanyId = "id",
                Name = "",
                StartDate = DateTime.Now.AddDays(5),
                EndDate = DateTime.Now.AddDays(25),
                WorkDays = new List<WorkDay>()
                {
                    new WorkDay
                    {
                        id = Guid.NewGuid(),
                        Date = DateTime.Now.AddDays(7),
                    },
                    new WorkDay
                    {
                        id = Guid.NewGuid(),
                        Date = DateTime.Now.AddDays(8),
                    }
                }
            };


            //Act
            var result = Assert.Throws<ValidationException>(() => planning.Validate());

            //Assert
            Assert.IsType<ValidationException>(result);
            Assert.NotEqual(0, result.Message.Length);
        }

        [Fact]
        public void ValidatePlanningStartDateBeforeToday()
        {
            //Arrange
            var planning = new Planning
            {
                id = Guid.NewGuid(),
                CompanyId = "id",
                Name = "test",
                StartDate = DateTime.Now.AddDays(-1),
                EndDate = DateTime.Now.AddDays(25),
                WorkDays = new List<WorkDay>()
                {
                    new WorkDay
                    {
                        id = Guid.NewGuid(),
                        Date = DateTime.Now.AddDays(7),
                    },
                    new WorkDay
                    {
                        id = Guid.NewGuid(),
                        Date = DateTime.Now.AddDays(8),
                    }
                }
            };


            //Act
            var result = Assert.Throws<ValidationException>(() => planning.Validate());

            //Assert
            Assert.IsType<ValidationException>(result);
            Assert.NotEqual(0, result.Message.Length);
        }

        [Fact]
        public void ValidatePlanningEndDateBeforeToday()
        {
            //Arrange
            var planning = new Planning
            {
                id = Guid.NewGuid(),
                CompanyId = "id",
                Name = "test",
                StartDate = DateTime.Now.AddDays(5),
                EndDate = DateTime.Now.AddDays(-5),
                WorkDays = new List<WorkDay>()
                {
                    new WorkDay
                    {
                        id = Guid.NewGuid(),
                        Date = DateTime.Now.AddDays(7),
                    },
                    new WorkDay
                    {
                        id = Guid.NewGuid(),
                        Date = DateTime.Now.AddDays(8),
                    }
                }
            };


            //Act
            var result = Assert.Throws<ValidationException>(() => planning.Validate());

            //Assert
            Assert.IsType<ValidationException>(result);
            Assert.NotEqual(0, result.Message.Length);
        }

        [Fact]
        public void ValidatePlanningEndDateBeforeStartDate()
        {
            //Arrange
            var planning = new Planning
            {
                id = Guid.NewGuid(),
                CompanyId = "id",
                Name = "test",
                StartDate = DateTime.Now.AddDays(5),
                EndDate = DateTime.Now.AddDays(3),
                WorkDays = new List<WorkDay>()
                {
                    new WorkDay
                    {
                        id = Guid.NewGuid(),
                        Date = DateTime.Now.AddDays(7),
                    },
                    new WorkDay
                    {
                        id = Guid.NewGuid(),
                        Date = DateTime.Now.AddDays(8),
                    }
                }
            };


            //Act
            var result = Assert.Throws<ValidationException>(() => planning.Validate());

            //Assert
            Assert.IsType<ValidationException>(result);
            Assert.NotEqual(0, result.Message.Length);
        }

        [Fact]
        public void ValidatePlanningNoWorkDay()
        {
            //Arrange
            var planning = new Planning
            {
                id = Guid.NewGuid(),
                CompanyId = "id",
                Name = "test",
                StartDate = DateTime.Now.AddDays(5),
                EndDate = DateTime.Now.AddDays(25),
                WorkDays = new List<WorkDay>()
            };

            //Act
            var result = Assert.Throws<ValidationException>(() => planning.Validate());

            //Assert
            Assert.IsType<ValidationException>(result);
            Assert.NotEqual(0, result.Message.Length);
        }

        [Fact]
        public void ValidatePlanningWorkDayNoId()
        {
            //Arrange
            var planning = new Planning
            {
                id = Guid.NewGuid(),
                CompanyId = "id",
                Name = "test",
                StartDate = DateTime.Now.AddDays(5),
                EndDate = DateTime.Now.AddDays(25),
                WorkDays = new List<WorkDay>()
                {
                    new WorkDay
                    {
                        id = Guid.Empty,
                        Date = DateTime.Now.AddDays(7),
                    },
                    new WorkDay
                    {
                        id = Guid.NewGuid(),
                        Date = DateTime.Now.AddDays(8),
                    }
                }
            };


            //Act
            var result = Assert.Throws<ValidationException>(() => planning.Validate());

            //Assert
            Assert.IsType<ValidationException>(result);
            Assert.NotEqual(0, result.Message.Length);
        }

        [Fact]
        public void ValidatePlanningWorkDayDateNotInRange()
        {
            //Arrange
            var planning = new Planning
            {
                id = Guid.NewGuid(),
                CompanyId = "id",
                Name = "test",
                StartDate = DateTime.Now.AddDays(5),
                EndDate = DateTime.Now.AddDays(25),
                WorkDays = new List<WorkDay>()
                {
                    new WorkDay
                    {
                        id = Guid.Empty,
                        Date = DateTime.Now.AddDays(3),
                    },
                    new WorkDay
                    {
                        id = Guid.NewGuid(),
                        Date = DateTime.Now.AddDays(8),
                    }
                }
            };


            //Act
            var result = Assert.Throws<ValidationException>(() => planning.Validate());

            //Assert
            Assert.IsType<ValidationException>(result);
            Assert.NotEqual(0, result.Message.Length);
        }

        [Fact]
        public void ValidatePlanningDoubleWorkDayDateOnSameDay()
        {
            //Arrange
            var planning = new Planning
            {
                id = Guid.NewGuid(),
                CompanyId = "id",
                Name = "test",
                StartDate = DateTime.Now.AddDays(5),
                EndDate = DateTime.Now.AddDays(25),
                WorkDays = new List<WorkDay>()
                {
                    new WorkDay
                    {
                        id = Guid.Empty,
                        Date = DateTime.Now.AddDays(8),
                    },
                    new WorkDay
                    {
                        id = Guid.NewGuid(),
                        Date = DateTime.Now.AddDays(8),
                    }
                }
            };


            //Act
            var result = Assert.Throws<ValidationException>(() => planning.Validate());

            //Assert
            Assert.IsType<ValidationException>(result);
            Assert.NotEqual(0, result.Message.Length);
        }

    }
}
