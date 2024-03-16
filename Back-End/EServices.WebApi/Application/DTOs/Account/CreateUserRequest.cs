using System;
using System.ComponentModel.DataAnnotations;
using Application.Enums;

namespace Application.DTOs.Account;

public class CreateUserRequest
{
    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public UserRoles UserRole {get;set;}


}
