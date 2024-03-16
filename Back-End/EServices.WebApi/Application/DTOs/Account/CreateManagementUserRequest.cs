using System;
using System.ComponentModel.DataAnnotations;
using Application.Enums;

namespace Application.DTOs.Account;

public class CreateManagementUserRequest
{
    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public Roles UserRole { get; set; }

    public Guid UserId { get; set; }
    public Guid GroupId { get; set; }

}
