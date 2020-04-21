using System;

namespace LightTown.Core.Domain.Roles
{
    [Flags]
    public enum Permissions
    {
        NONE                = 0,
        //CREATE_GROUPS       = 1 << 1,
        //MANAGE_GROUPS       = 1 << 2,
        //VIEW_ALL_GROUPS     = 1 << 3,
        CREATE_PROJECTS     = 1 << 4,
        MANAGE_PROJECTS     = 1 << 5,
        DELETE_PROJECTS     = 1 << 6,
        VIEW_ALL_PROJECTS   = 1 << 7,
        MANAGE_ROLES        = 1 << 8,
        MANAGE_USERS        = 1 << 9,
        ALL = ~NONE
    }
}