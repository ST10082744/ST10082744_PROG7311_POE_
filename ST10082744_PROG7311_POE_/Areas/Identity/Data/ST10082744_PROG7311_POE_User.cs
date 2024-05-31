using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ST10082744_PROG7311_POE_.Models;

namespace ST10082744_PROG7311_POE_.Areas.Identity.Data;

// Add profile data for application users by adding properties to the ST10082744_PROG7311_POE_User class
public class ST10082744_PROG7311_POE_User : IdentityUser
{
    /// <summary>
    /// stores farmer name
    /// </summary>
    public string? Name { get; set; }
    /// <summary>
    /// stores farmer surname
    /// </summary>
    public string? Surname { get; set; }
    /// <summary>
    /// stores farmeer phonenummber
    /// </summary>
    public string? PhoneNumber { get; set; }
}

