using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity;
  
public class AppUser : IdentityUser<Guid>
{
    public required string KnownAs { get; set; }
    public required string Gender { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public required string City { get; set; }
    public required string Country { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

