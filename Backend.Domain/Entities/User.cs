using Backend.Domain.ValueObjects;
using Backend.Domain.Roles;
using Microsoft.VisualBasic;

namespace Backend.Domain.Entities;

public class User
{
    public Guid Id {get; private set;}
    public UserName Username {get; private set;}

    public PasswordHash Password {get; private set;}

    public Role Role { get; private set; }

    private readonly List<Order> _orders= new();
    public IReadOnlyCollection<Order> Orders => _orders.AsReadOnly();

    public User (UserName username, PasswordHash password, Role role)
    {
        Id= Guid.NewGuid();
        Username= username;
        Password= password;
        Role= role ?? throw new ArgumentNullException(nameof(role));
    }

    public void AddOrder(Order order, User addedBy)
    {
        if (order == null)
        throw new ArgumentNullException(nameof(order));

        if (!addedBy.Role.Permissions.Contains(Permission.AddOrder))
        throw new UnauthorizedAccessException();

        _orders.Add(order);
    }

    public void DeleteOrder(Guid orderId, User deletedBy)
    {
        var order = _orders.SingleOrDefault(o => o.Id == orderId);
        if (order == null) throw new ArgumentNullException();

        if (deletedBy.Id == Id)
        {
            if(!deletedBy.Role.Permissions.Contains(Permission.DeleteOwnOrder))
            throw new UnauthorizedAccessException();
        }
        
        else
        {
            if(!deletedBy.Role.Permissions.Contains(Permission.DeleteAnyOrder))
            throw new UnauthorizedAccessException();
        }

        _orders.Remove(order);
    }

    public void UpdateOrderPrice (Guid orderId, decimal newPrice, User updatedBy)
    {
        var order= _orders.SingleOrDefault(o => o.Id == orderId);
        if (order == null) throw new ArgumentNullException();

        if (updatedBy.Id == Id)
        {
            if (!updatedBy.Role.Permissions.Contains(Permission.EditOwnOrder))
            throw new UnauthorizedAccessException();
        }
        else
        {
            if (!updatedBy.Role.Permissions.Contains(Permission.EditAnyOrder))
            throw new UnauthorizedAccessException();
        }

        order.UpdatePrice(newPrice);

    }

    public void UpdateUserDetails (UserName newUserName, PasswordHash newPasswordHash, Role newRole)
    {
        Username= newUserName;
        Password= newPasswordHash;
        Role= newRole;
    }
}