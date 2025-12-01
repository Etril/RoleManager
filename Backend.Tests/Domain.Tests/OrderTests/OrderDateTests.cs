using System.Runtime.CompilerServices;
using Backend.Domain.Entities;
using Backend.Domain.ValueObjects;
using FluentAssertions;
using Microsoft.VisualBasic;


namespace Domain.Tests {

    public class OrderDateTests{

        [Fact]
        
        public void Order_Creation_WithTodayDate_Succeeds()
        {
            //Arrange
            var now= DateTime.UtcNow;

            //Act
            var date= new OrderDate(now);

            //Assert 
            date.Value.Should().Be(now);
        }
        
        
        [Fact]
        public void Order_Creation_Fails_When_OrderDate_Future()
        {
            // Arrange
            var future= DateTime.UtcNow.AddYears(1);

            //Act
            Action act = () => new OrderDate(future);

    
            // Assert
            act.Should()
           .Throw<ArgumentException>()
           .WithMessage("Order date cannot be in the future");
        
        }

        [Fact]
        
        public void Order_Creation_Fails_When_DateIsMinValue()
        {
            //Arrange
            var min = DateTime.MinValue;

            // Act
            Action act= () => new OrderDate(min);

            //Assert
            act.Should()
            .Throw<ArgumentException>()
            .WithMessage("Date cannot be empty");
            


        }
    
    }

}