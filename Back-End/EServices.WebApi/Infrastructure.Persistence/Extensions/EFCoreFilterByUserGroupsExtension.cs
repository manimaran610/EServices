using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Common;
using Infrastructure.Identity.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Extensions;

public static class EFCoreFilterByUserGroupsExtension
{

    public static System.Linq.IQueryable<T> FilterByUserGroups<T>(this IQueryable<T> collection, IdentityContext identityContext, string userId) where T : AuditableBaseEntity
    {
        if (userId != null)
        {
            var userGroupIds = identityContext.UserGroups
                 .Where(ug => ug.UserId == userId)
                 .Select(ug => ug.GroupId)
                 .ToList();

                var groupUsers = identityContext.UserGroups.Where(ug => userGroupIds.Contains(ug.GroupId))
                 .Select(ug => ug.UserId)
                 .ToList();

            // return collection.Join(identityContext.UserGroups,
            //                             e => e.CreatedBy,
            //                             ug => ug.UserId,
            //                             (entity, userGroup) => new { Entity = entity, UserGroup = userGroup })
            // .Where(joined => userGroupIds.Contains(joined.UserGroup.GroupId))
            // .Select(e => e.Entity);

            return collection.Where(e=>groupUsers.Contains(e.CreatedBy));
        }
        else
        {
            return collection;
        }
    }

}
