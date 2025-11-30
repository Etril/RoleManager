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

       
    }
}