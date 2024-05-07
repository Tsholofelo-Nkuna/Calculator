using Calculator.Model.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace Calculator.SQLServer.DAL.Base
{
    public interface IRepository<TEntity>
    {
        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> expression);

        IEnumerable<long> InsertBulk(List<TEntity> entities);

        IEnumerable<long> UpdateBulk(List<TEntity> entities);

        IEnumerable<long> DeleteBulk(List<long> ids);
    }
    public class GenericRepositoryPIT<TEntity> : IRepository<TEntity> where TEntity : BaseEntityPIT
    {
        private readonly DbSet<TEntity> _dbSet;
        private readonly WebDbContext _webDbContext;
        public GenericRepositoryPIT(WebDbContext dbContext) { 
          this._dbSet = dbContext.Set<TEntity>();
          this._webDbContext = dbContext;
        }
        public IEnumerable<long> DeleteBulk(List<long> ids)
        {
           var toBeDeleted = this._dbSet.Where(x => ids.Contains(x.Id) && x.ModifiedOn == null)
                            .ToList()
                            .Select(x =>
                            {
                                x.Inactive = true;
                                x.ModifiedOn = DateTime.Now;
                                return x;
                            });
         
           this._webDbContext.SaveChanges();
          return this.InsertBulkPIT(toBeDeleted.ToList());
        }

        public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> expression)
        {
            return this._dbSet.Where(expression).AsNoTracking();
        }

        public IEnumerable<long> InsertBulk(List<TEntity> entities)
        {
            foreach(var e in entities)
            {
                e.MasterId = 0;
               
                    e.Id = e.MasterId;
                    e.ModifiedOn = null;
                    e.Inactive = false;
                    this._dbSet.Add(e);
                    this._webDbContext.SaveChanges();
                    e.MasterId = e.Id;
                
            }
            this._webDbContext.SaveChanges();
            return entities.Select(x => x.MasterId);
        }

        public IEnumerable<long> InsertBulkPIT(List<TEntity> entities)
        {
            foreach (var e in entities)
            {
                if(e.ModifiedOn is not null)
                {
                    e.ModifiedOn = null;
                }
                e.Id = 0;
            }

            this._dbSet.AddRange(entities);
            this._webDbContext.SaveChanges();
            return entities.Select(x => x.MasterId);
        }
        public IEnumerable<long> UpdateBulk(List<TEntity> entities)
        {
          var existingRecords = this._dbSet
                .Where(x => entities.Select(y => y.MasterId).Contains(x.MasterId) && x.ModifiedOn == null && !x.Inactive)
                .AsNoTracking()
                .ToList();
        
          var toBeUpdated = from dbRec in existingRecords
                             join update in entities
                             on dbRec.MasterId equals update.MasterId
                             select new { UpdateId = dbRec.Id, Update = update, dbRec };
          var updatedRecords = new List<TEntity>(); 
          var dbRecs = new List<TEntity>();
           foreach(var e in toBeUpdated)
            {
                
               e.dbRec.ModifiedOn = DateTime.Now;
               e.Update.Id = e.UpdateId;
               e.Update.ModifiedOn = null;
               dbRecs.Add(e.dbRec);
               updatedRecords.Add(e.Update);
            }

           this._dbSet.UpdateRange(dbRecs);
           this._webDbContext.SaveChanges();
           return this.InsertBulkPIT(updatedRecords);
        }

        
    }
}
