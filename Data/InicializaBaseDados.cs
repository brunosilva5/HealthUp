﻿using HealthUp.Helpers;
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

                var Pessoas = new Pessoa[]
                {
                    new Pessoa()
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
                        IsNotified=false,
                        Admin=new Admin()
                    },
                    new Pessoa()
                    {
                        NumCC="87654321",
                        Username="OralBento",
                        Sexo="M",
                        DataNascimento=new DateTime(1999,6,23).Date,
                        Fotografia="admin.jpg",
                        Email="admin@healthup.pt",
                        Nacionalidade="PT",
                        Nome="João Soares",
                        Telemovel="+351696969696",
                        Password=SecurePasswordHasher.Hash("admin"),
                        IsNotified=false,
                        Admin=new Admin()
                    }
                };




                //... insere-os no model...
                foreach (var p in Pessoas)
                {
                    context.Pessoas.Add(p);
                }
                //...e atualiza a base de dados
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
            if (context.Professores.Any()==false)
            {
                var Pessoa = new Pessoa()
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
                    IsNotified = false,
                    Professor = new Professor()
                };
                Pessoa.Professor.Especialidade = "KUNGU";
                context.Pessoas.Add(Pessoa);

            }

            if (context.Socios.Any() == false)
            {
                var pessoa = new Pessoa()
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
                    IsNotified = false,
                    Socio = new Socio()
                };
                pessoa.Socio.Peso = 50;
                pessoa.Socio.Altura = "150";

                context.Pessoas.Add(pessoa);

            }

            context.SaveChanges();


        }
    }
}

