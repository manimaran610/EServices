using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Domain.Common;
using Infrastructure.Identity.Contexts;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Infrastructure.Persistence.Extensions;

public static class EFCoreFilterByUserGroupsExtension
{

    public static System.Linq.IQueryable<T> FilterByUserGroups<T>(this IQueryable<T> collection, IdentityContext identityContext, string userId) where T : AuditableBaseEntity
    {

        //   "email": "group2024032310551@mail.com",
        //   "password": "r1yaPh8M9N8j5e@%!&@i"

        if (userId != null)
        {
            var userIds = identityContext.UserGroups
                 .Join(identityContext.UserGroups, e => e.GroupId, x => x.GroupId, (u, ug) => new { CurrentUserId = u.UserId, ug.UserId })
                 .Where(ug => ug.CurrentUserId == userId)
                 .Select(ug => ug.UserId)
                 .ToListAsync().Result;
  
            return collection.Where(e => userIds.Contains(e.CreatedBy));
        }
        else
        {
            return collection;
        }
    }

}
