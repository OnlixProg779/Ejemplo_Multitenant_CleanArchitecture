using Base.Domain.Common;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoAuth.Domain.Identity
{
    public class Organization : BaseEntities
    {
        public string OrganizationName { get; set; } = null!;

        public string IdentityUserId { get; set; } = null!;

        [ForeignKey(nameof(IdentityUserId))]
        public IdentityUser? User { get; set; }
    }
}
