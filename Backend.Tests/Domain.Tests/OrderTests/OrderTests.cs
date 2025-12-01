using Backend.Domain.Entities;
using Backend.Domain.ValueObjects;
using FluentAssertions;
using Xunit; 

namespace Domain.Tests
{
    public class OrderTests
    {
        [Fact]
        public void Order_Creation_WithValidParameters_ShouldSucceed()
        {
            // Arrange
            var name = new OrderName("Laptop");
            var date= new OrderDate(DateTime.Today);
            decimal price= 1200;
        
            // Act
            var order= new Order(name, price, date);
        
            // Assert
            order.Name.Value.Should().Be("Laptop");
            order.Date.Value.Should().Be(DateTime.Today);
            order.Price.Should().Be(1200);
            order.Id.Should().NotBeEmpty();
        }

        [Fact]

        public void Order_Creation_Fails_WithNegativePrice()
        {
            //Arrange
            decimal price= -1200; 
            var name= new OrderName("Laptop");
            var date= new OrderDate(DateTime.Today);

            // Act
            Action act = () => new Order(name, price, date);

            // Assert
            act.Should()
            .Throw<ArgumentException>()
            .WithMessage("Price can't be negative");
        }

        [Fact]

        public void Order_Creation_AssignsUniqueId()
        {
            //Arrange
            decimal price= 1200;
            var name= new OrderName("Laptop");
            var date= new OrderDate(DateTime.Today);

            //Act
            var order1= new Order (name, price, date);
            var order2= new Order (name, price, date);

            //Assert
            order1.Id.Should().NotBe(order2.Id);
            
        }
    }
}