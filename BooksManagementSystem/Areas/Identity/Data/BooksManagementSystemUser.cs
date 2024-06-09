using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using BooksManagementSystem.Models;
using Microsoft.AspNetCore.Identity;

namespace BooksManagementSystem.Areas.Identity.Data;

// Add profile data for application users by adding properties to the BooksManagementSystemUser class
public class BooksManagementSystemUser : IdentityUser
{
    [PersonalData]
    [Column(TypeName = "nvarchar(100)")]
    public string Firstname { get; set; }

    [PersonalData]
    [Column(TypeName = "nvarchar(100)")]
    public string LastName { get; set; }

    public virtual ICollection<BorrowingViewModel> Borrowings { get; set; }
}

