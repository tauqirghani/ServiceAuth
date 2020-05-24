using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DataAPI.Models
{
    public class FileContext : DbContext
    {
        public FileContext(DbContextOptions<FileContext> options)
            : base(options)
        {
        }

        public DbSet<File> Files { get; set; }

    }
}

