using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using SystemWakeUp.DBHandler.Entity;
using SystemWakeUp.Network.Misc;

namespace SystemWakeUp.DBHandler
{
    // boring DB stuff
    public class AppDbContext : DbContext
    {
        public DbSet<DBEntity> Entities { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DBEntity>()
             .Property(e => e.Id)
             .ValueGeneratedOnAdd(); //making id auto-incrementing value.

            base.OnModelCreating(modelBuilder);
            InitializeDatabase();
        }

        public void InitializeDatabase()
        {
            string[] dbdata = ReadWriteConfig.ReadDBCreds();
            string db = "mysystemwakeup";
            string connString = $"Host=localhost;Port=5432;Username={dbdata[0]};Password={dbdata[1]};Database=postgres";

            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                string checkDbExist = $"SELECT 1 FROM pg_database WHERE datname='{db}'";
                using (var cmd = new NpgsqlCommand(checkDbExist, conn))
                {
                    var exists = cmd.ExecuteScalar();
                    if (exists == null)
                    {
                        string createDb = $"CREATE DATABASE {db}";
                        using (var createCmd = new NpgsqlCommand(createDb, conn))
                        {
                            createCmd.ExecuteNonQuery();
                            Console.WriteLine("DB has been created");
                        }
                    }
                    else
                    {
                        Console.WriteLine("DB already exists!");
                    }
                }

                conn.ChangeDatabase(db);

                string checkTableExist = $"SELECT 1 FROM information_schema.tables WHERE table_schema = 'public' AND table_name = 'usagesw'";
                using (var cmd = new NpgsqlCommand(checkTableExist, conn))
                {
                    var tableExists = cmd.ExecuteScalar();
                    if (tableExists == null)
                    {
                        string createTable = "CREATE TABLE usagesw (id INT PRIMARY KEY, mac VARCHAR(50), lastlogin TIMESTAMP);";
                        using (var createTableCmd = new NpgsqlCommand(createTable, conn))
                        {
                            createTableCmd.ExecuteNonQuery();
                            Console.WriteLine("Table has been created");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Table already exists!");
                    }
                }
            }
        }

        public async Task<List<DBEntity>> GetEntitiesAsync()
        {
            return await Entities.ToListAsync();
        }

        public async Task<DBEntity> GetEntityByIdAsync(int id)
        {
            return await Entities.FindAsync(id);
        }

        public async Task AddEntityAsync(DBEntity entity)
        {
            Entities.Add(entity);
            await SaveChangesAsync();
        }

        public async Task UpdateEntityAsync(DBEntity entity)
        {
            Entities.Update(entity);
            await SaveChangesAsync();
        }

        public async Task DeleteEntityAsync(int id)
        {
            var entity = await GetEntityByIdAsync(id);
            if (entity != null)
            {
                Entities.Remove(entity);
                await SaveChangesAsync();
            }
        }
    }
}
