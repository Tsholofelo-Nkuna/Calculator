using Calculator.SQLServer.DAL.Base;
using Calculator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calculator.Model.Base;

namespace Calculator.SQLServer.DAL.Repository
{
    public interface IOperationRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {

    }
    public class OperationRepository: GenericRepositoryPIT<OperationEntity>, IOperationRepository<OperationEntity>
    {
        public OperationRepository(WebDbContext dbContext): base(dbContext){}
    }
}
