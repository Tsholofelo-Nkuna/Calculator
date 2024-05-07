using Calculator.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.SQLServer.DAL
{
    public class WebDbContext: DbContext
    {
        private readonly ConnectionStringConfig _connectionStringConfig;
        public WebDbContext(IOptions<ConnectionStringConfig> connectionStringConfig)
        {
            this._connectionStringConfig = connectionStringConfig.Value;
            
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(this._connectionStringConfig.ConnectionString);
        }

        public DbSet<OperationEntity> Operations { get; set; }

        public DbSet<MREntity> MR { get; set; }

    }

    public class ConnectionStringConfig
    {
        public string ConnectionString { get; set; } = string.Empty;
    }
}
