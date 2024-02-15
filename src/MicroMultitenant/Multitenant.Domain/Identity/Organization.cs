using Microsoft.AspNetCore.Identity;
using Multitenant.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Multitenant.Domain.Identity
{
    public class Organization : BaseEntities
    {
        public string OrganizationName { get; set; } = null!;

        public string IdentityUserId { get; set; } = null!;

        [ForeignKey(nameof(IdentityUserId))]
        public IdentityUser? User { get; set; }
    }
}
