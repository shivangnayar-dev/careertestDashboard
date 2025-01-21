using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Auxx.Models;

public partial class User
{
    [Key]
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Mobile { get; set; } = null!;

    public string Adhar { get; set; } = null!;

    [NotMapped]
    public Roles Roles { get; set; }
    public int RoleId { get; set; }

    [NotMapped]
    public string RoleName { get; set; }

    [NotMapped]
    public Organizations Organizations { get; set; }

    public int OrganizationId { get; set; }
    [NotMapped]
    public int OrganizationName { get;set; }

}