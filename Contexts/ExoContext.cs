using Exo.WebApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;

namespace Exo.WebApi.Contexts
{
    public class ExoContext : DbContext
    {
        //metodo construtor vazio
        public ExoContext()
        {            
        }

        //
        public ExoContext(DbContextOptions<ExoContext> options) : base(options)
        {            
        }

        //metodo onde configuramos a conexão
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(!optionsBuilder.IsConfigured)
            {
                //abaixo string de conexão com o BD - é o item Principal para comunicar com o BD
                optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;" + "Database=ExoApi;Trusted_Connection=True;");
            }
        }

        //abaixo vai referenciando a classe Projeto.cs
        public DbSet<Projeto> Projetos {get; set;}
    }
}

//////// Exemplo 1 de string de conexão:
// User ID= sa;Password admin;Server localhost;Database ExoApi;-
// +Trusted_Connection = false; 
//////// Exemplo 2 de string de conexão:
// Server= localhost SQLEXPRESS;Database ExoApi;Trusted_Connection True