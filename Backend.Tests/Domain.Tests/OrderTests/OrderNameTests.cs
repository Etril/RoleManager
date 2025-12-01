using System.Runtime.CompilerServices;
using Backend.Domain.Entities;
using Backend.Domain.ValueObjects;
using FluentAssertions;
using Microsoft.VisualBasic;

namespace Domain.Tests
{
    public class OrderNameTests
    {
        [Fact]

        public void OrderName_Creation_WithValidValue_Succeeds()
        {
            //Arrange
            string name= "Laptop";

            //Act
            var orderName= new OrderName(name);

            //Assert
            orderName.Value.Should().Be(name);
        }

        [Fact]
        public void OrderName_Creation_Fails_WhenVallueisNullOrEmpty()
    
        {
            // Arrange
            string name= "";
        
            // Act
            Action act= () => new OrderName(name);
        
            // Assert
            act.Should()
            .Throw<ArgumentException>()
            .WithMessage("Order name cannot be empty");
            
        }

        [Fact]

        public void OrderName_Equals_ReturnsTrue_WhenSameName()
        {
            //Arrange
            string name= "Laptop";

            //Act
            var name1= new OrderName(name);
            var name2= new OrderName(name);

            //Assert
            name1.Should().Be(name2);
        }

}
}