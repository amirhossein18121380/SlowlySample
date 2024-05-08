
using Breeze.Persistence;
using SlowlySimulate.Domain.Models;

namespace SlowlySimulate.Persistence
{
    public class ApplicationPersistenceManager : BasePersistenceManager<SlowlyDbContext>
    {
        public ApplicationPersistenceManager(SlowlyDbContext dbContext,
            IHttpContextAccessor accessor,
            IServiceProvider serviceProvider) : base(dbContext, accessor, serviceProvider)
        { }

        protected override bool BeforeSaveEntity(EntityInfo entityInfo)
        {
            if (entityInfo.Entity is SlowlyUser userProfile)
                userProfile.UpdatedDateTime = DateTime.Now;
            else if (entityInfo.Entity is User applicationUser && entityInfo.EntityState == Breeze.Persistence.EntityState.Modified)
            {
                var props = DbContext.Entry(applicationUser).GetDatabaseValues();
                applicationUser.PasswordHash = props.GetValue<string>("PasswordHash");
                applicationUser.SecurityStamp = props.GetValue<string>("SecurityStamp");
            }

            return true;
        }
    }
}
