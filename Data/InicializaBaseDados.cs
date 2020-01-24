using HealthUp.Helpers;
using HealthUp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthUp.Data
{
    public class InicializaBasedeDados
    {
        public static void Iniciar(HealthUpContext context)
        {
            //verifica e garante que a BD existe
            context.Database.EnsureCreated();

            // analiza a(s) tabela(s) onde pretendemos garantir os dados
            if (context.Pessoas.Any() == false)
            {
                // prepara os dados para a tabela...

                    Pessoa p1= new Pessoa()
                    {
                        NumCC="48715473",
                        Username="spamz",
                        Sexo="M",
                        DataNascimento=new DateTime(1999,6,23).Date,
                        Fotografia="admin.jpg",
                        Email="admin@healthup.pt",
                        Nacionalidade="PT",
                        Nome="Diogo Silva",
                        Telemovel="+351937372277",
                        Password=SecurePasswordHasher.Hash("admin"),
                        Admin=new Admin()
                    };
                context.Pessoas.Add(p1);
                    Pessoa p2 = new Pessoa()
                    {
                        NumCC = "87654321",
                        Username = "OralBento",
                        Sexo = "M",
                        DataNascimento = new DateTime(1999, 6, 23).Date,
                        Fotografia = "admin.jpg",
                        Email = "admin@healthup.pt",
                        Nacionalidade = "PT",
                        Nome = "João Soares",
                        Telemovel = "+351696969696",
                        Password = SecurePasswordHasher.Hash("admin"),
                        Admin = new Admin()
                    };
                context.Pessoas.Add(p2);
                Pessoa p3 = new Pessoa
                {
                    NumCC = "48715479",
                    Username = "spamzprof",
                    Sexo = "M",
                    DataNascimento = new DateTime(1999, 6, 23).Date,
                    Fotografia = "admin.jpg",
                    Email = "admin@healthup.pt",
                    Nacionalidade = "PT",
                    Nome = "Diogo Silva",
                    Telemovel = "+351937372277",
                    Password = SecurePasswordHasher.Hash("prof"),
                    Professor = new Professor()
                    };
                    p3.Professor.Especialidade = "KUNGFU";

                    context.Pessoas.Add(p3);

                    Pessoa p4 = new Pessoa
                    {
                        NumCC = "48725479",
                        Username = "spamzsocio",
                        Sexo = "M",
                        DataNascimento = new DateTime(1999, 6, 23).Date,
                        Fotografia = "admin.jpg",
                        Email = "admin@healthup.pt",
                        Nacionalidade = "PT",
                        Nome = "Diogo Silva",
                        Telemovel = "+351937372277",
                        Password = SecurePasswordHasher.Hash("socio"),
                        Socio = new Socio(),
                        
                    };
                    p4.Socio.Peso = 50;
                    p4.Socio.Altura = "150";
                    p4.Socio.DataRegisto = DateTime.Now;
                    context.Pessoas.Add(p4);
    
            }
            if (context.Ginasios.Any() == false)
            {
                Ginasio gym = new Ginasio()
                {
                    Email = "geral@healthup.pt",
                    Endereco = "HealthUp Street",
                    Telemovel = "+351938778987",
                    LocalizacaoGps = "Some Coordinates",
                    Nome = "HealthUp",
                };

                context.Ginasios.Add(gym);
            }
            

            context.SaveChanges();


        }
    }
}

