using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Domain.Permissions
{
    public class EntityPermissions
    {
        private static readonly ReadOnlyCollection<EntityPermission> AllPermissions;

        /// <summary>
        /// Generates ApplicationPermissions based on Permissions Type by iterating over its nested classes and getting constant strings in each class as Value and Name, LocalizedDescriptionAttribute of the constant string as Description, the nested class name as GroupName.
        /// </summary>
        static EntityPermissions()
        {
            List<EntityPermission> allPermissions = new();
            IEnumerable<object> permissionClasses = typeof(Permissions).GetNestedTypes(BindingFlags.Static | BindingFlags.Public).Cast<TypeInfo>();
            foreach (TypeInfo permissionClass in permissionClasses)
            {
                IEnumerable<FieldInfo> permissions = permissionClass.DeclaredFields.Where(f => f.IsLiteral);
                foreach (FieldInfo permission in permissions)
                {
                    EntityPermission applicationPermission = new()
                    {
                        Value = permission.GetValue(null).ToString(),
                        Name = permission.GetValue(null).ToString().Replace('.', ' '),
                        GroupName = permissionClass.Name
                    };

                    DisplayAttribute[] attributes = (DisplayAttribute[])permission.GetCustomAttributes(typeof(DisplayAttribute), false);

                    applicationPermission.Description = attributes != null && attributes.Length > 0 ? attributes[0].Description : applicationPermission.Name;

                    allPermissions.Add(applicationPermission);
                }
            }

            var entitiesWithPermissionsAttribute = Assembly.GetExecutingAssembly().GetExportedTypes().Where(t => t.GetCustomAttributes<PermissionsAttribute>(inherit: false).Any());

            foreach (Type entity in entitiesWithPermissionsAttribute)
            {
                var requiredPermissions = entity.GetCustomAttribute<PermissionsAttribute>(false);

                foreach (Actions action in Enum.GetValues(typeof(Actions)))
                    if ((requiredPermissions.Actions & action) == action && action != Actions.CRUD && action != Actions.CUD)
                        allPermissions.Add(new EntityPermission
                        {
                            Value = $"{entity.Name}_{action}",
                            Name = $"{entity.Name}_{action}",
                            GroupName = entity.Name
                        });
            }

            AllPermissions = allPermissions.AsReadOnly();
        }


        private IEnumerable<EntityPermission> GetAllPermission()
        {
            return AllPermissions;
        }

        public EntityPermission GetPermissionByName(string permissionName)
        {
            return GetAllPermission().Where(p => p.Name == permissionName).FirstOrDefault();
        }

        public EntityPermission GetPermissionByValue(string permissionValue)
        {
            return GetAllPermission().Where(p => p.Value == permissionValue).FirstOrDefault();
        }

        public string[] GetAllPermissionValues()
        {
            return GetAllPermission().OrderBy(p => p.Value).Select(p => p.Value).ToArray();
        }

        public string[] GetAllPermissionNames()
        {
            return GetAllPermission().OrderBy(p => p.Name).Select(p => p.Name).ToArray();
        }

        public string[] GetAllPermissionValuesFor(UserFeatures userFeature)
        {
            switch (userFeature)
            {
                case UserFeatures.Operator:
                    return GetAllPermission()
                        .Where(i => i.GroupName == "User" ||
                                    i.GroupName == nameof(Permissions.User))
                        .Select(p => p.Value).ToArray();

                default:
                    return Array.Empty<string>();
            }

        }
    }
}
