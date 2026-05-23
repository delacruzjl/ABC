using Microsoft.AspNetCore.Identity;

namespace ABC.PostGreSQL;

public class ApplicationUser : IdentityUser
{
    public bool IsActive { get; set; } = true;
    public Guid? DefaultChildId { get; set; }
}
