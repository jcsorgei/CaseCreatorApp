using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

public class AppUser : IdentityUser
{
    public List<SvgData> SvgData { get; set; } = new();
}