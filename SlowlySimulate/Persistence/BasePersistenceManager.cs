using Breeze.Persistence;
using Breeze.Persistence.EFCore;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SlowlySimulate.Infrastructure.AuthorizationDefinitions;
using SlowlySimulate.Persistence.Permissions;
using System.Net;
using System.Reflection;
using EntityState = Breeze.Persistence.EntityState;


namespace SlowlySimulate.Persistence
{
    public abstract class BasePersistenceManager<T> : EFPersistenceManager<T> where T : DbContext
    {
        protected readonly IHttpContextAccessor httpContextAccessor;
        protected readonly IServiceProvider serviceProvider;
        public BasePersistenceManager(T dbContext,
            IHttpContextAccessor accessor,
            IServiceProvider serviceProvider) : base(dbContext)
        {
            httpContextAccessor = accessor;
            this.serviceProvider = serviceProvider;
        }
        public DbSet<TEntity> GetEntities<TEntity>() where TEntity : class
        {
            var requiredPermissions = typeof(TEntity).GetCustomAttribute<PermissionsAttribute>(false);

            if (requiredPermissions != null && (requiredPermissions.Actions & Actions.Read) == Actions.Read)
            {
                var user = httpContextAccessor?.HttpContext?.User;

                if (user == null || user.Identity.IsAuthenticated == false)
                    throw new UnauthorizedAccessException("AuthenticationRequired" + ": " + "LoginRequired");
                else if (!user.Claims.Any(c => c.Type == ApplicationClaimTypes.Permission && c.Value == $"{typeof(TEntity).Name}.{Actions.Read}"))
                    throw new UnauthorizedAccessException("Operation not allowed" + ": " + "NotAuthorizedTo");
            }

            return Context.Set<TEntity>();
        }

        protected override Dictionary<Type, List<EntityInfo>> BeforeSaveEntities(Dictionary<Type, List<EntityInfo>> saveMap)
        {
            var errors = new List<EFEntityError>();
            var user = httpContextAccessor?.HttpContext?.User;

            foreach (var type in saveMap.Keys)
            {
                var requiredPermissions = type.GetCustomAttribute<PermissionsAttribute>(false);

                if (requiredPermissions != null)
                {
                    Actions? requiredAction = null;

                    foreach (var entityInfo in saveMap[type])
                    {
                        switch (entityInfo.EntityState)
                        {
                            case EntityState.Added:
                                requiredAction = Actions.Create;
                                break;
                            case EntityState.Modified:
                                requiredAction = Actions.Update;
                                break;
                            case EntityState.Deleted:
                                requiredAction = Actions.Delete;
                                break;
                        }

                        var entityType = entityInfo.Entity.GetType();

                        if ((requiredPermissions.Actions & requiredAction) == requiredAction)
                        {
                            if (user == null || user.Identity.IsAuthenticated == false)
                                errors.Add(new EFEntityError(entityInfo, "AuthenticationRequired", "LoginRequired", null));
                            else if (!user.Claims.Any(c => c.Type == ApplicationClaimTypes.Permission && c.Value == $"{entityType.Name}.{requiredAction}"))
                                errors.Add(new EFEntityError(entityInfo, "Operation not allowed", "NotAuthorizedTo", null));
                        }

                        if (entityInfo.EntityState == EntityState.Added || entityInfo.EntityState == EntityState.Modified)
                        {
                            IValidator validator = (IValidator)serviceProvider.GetService(typeof(IValidator<>).MakeGenericType(entityType));

                            if (validator == null)
                            {
                                //var iface = entityType.GetInterfaces().SingleOrDefault(i => i.Namespace == typeof(LocalizationRecord).Namespace);

                                //if (iface != null)
                                //{
                                //    validator = (IValidator)serviceProvider.GetService(typeof(IValidator<>).MakeGenericType(iface));

                                //    if (validator != null)
                                //    {
                                //        var results = validator.Validate(new ValidationContext<object>(entityInfo.Entity));

                                //        if (!results.IsValid)
                                //            errors.AddRange(results.Errors.Select(i => new EFEntityError(entityInfo, i.ErrorCode, i.ErrorMessage, i.PropertyName)));
                                //    }
                                //}
                            }
                        }
                    }
                }
            }

            if (errors.Count > 0)
            {
                if (user == null || user.Identity.IsAuthenticated == false)
                    throw new EntityErrorsException(errors) { StatusCode = HttpStatusCode.Unauthorized };
                else
                    throw new EntityErrorsException(errors) { StatusCode = HttpStatusCode.Forbidden };
            }

            return saveMap;
        }
    }
}
