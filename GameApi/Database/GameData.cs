﻿using CharactersLib;
using GameLib;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;

namespace GameApi.Database
{
    public class GameData
    {
        // static string connectionString = "Server=database-2.cnfp1r5tha7w.us-east-1.rds.amazonaws.com,1433;Database=test123;User Id=admin;password=Summer2021!";
        SqlConnection connection;

        static string GetConnectionString() {
            try
            {
                 SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder() 
                 {
                     DataSource = "database-2.cnfp1r5tha7w.us-east-1.rds.amazonaws.com",
                     UserID = "admin",
                     Password = "Summer2021!",
                     InitialCatalog = "test123"
                 };
                 return builder.ConnectionString;
            }
            catch (System.Exception e)
            {
                throw new Exception("Error in GetConnectionString()" + e.Message);
            }
        }
        
        static string GetConnectionStringSecondary() {
            try
            {
                 SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder() 
                 {
                     DataSource = "MasterDB.mssql.somee.com",
                     UserID = "MPdb103340660",
                     Password = "Summer2020",
                     InitialCatalog = "MasterDB"
                 };
                 return builder.ConnectionString;
            }
            catch (System.Exception e)
            {
                throw new Exception("Error in GetConnectionStringSecondary()" + e.Message);
            }
        }
        
        public string Connect(string source)
        {
            try
            {
                // if the source = "primary", try connecting to the amazon database
                if (source == "primary") {
                    connection = new SqlConnection(GetConnectionString());
                    connection.Open();
                }
                else { // try with secondary database, at somee
                    connection = new SqlConnection(GetConnectionStringSecondary());
                    connection.Open();
                }
                 
            }
            catch (System.Exception e)
            {
                // we failed to connect to the primary database, try connecting to the secondary
                if (source == "primary") {
                    Connect("secondary"); // attempt sql connection with secondary database
                }
                else {
                    throw new Exception("Unable to open connection to the "+ source +" database: " + e.Message);
                }
            }

            return "Ok";
        }


        public List<Hero> GetAllHeroesFromDB()
        {
            List<Hero> heroes = new List<Hero>(); //temp array, sends to controller

            using (SqlConnection connection = new SqlConnection(GetConnectionString()))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM HERO", connection);

                SqlDataReader reader = command.ExecuteReader();

                try
                {
                    while (reader.Read())
                    {
                        //convert the data from DB into c# object
                        int HeroID = reader.GetInt32(0);
                        string HeroName = reader.GetString(1);
                        int MinDiceValue = reader.GetInt32(2);
                        int MaxDiceValue = reader.GetInt32(3);
                        int InitialUses = reader.GetInt32(4);
                        string ImageFileName = reader.GetString(5);

                        heroes.Add(new Hero(HeroID, HeroName, MinDiceValue, MaxDiceValue, InitialUses, ImageFileName));
                    }
                }
                catch (System.Exception e)
                {
                    throw new Exception("Unable to get all heroes from the database: " + e.Message);
                }
            }

            return heroes;
        }

        public List<Villain> GetAllVillainsFromDB()
        {
            List<Villain> villains = new List<Villain>(); //temp array, sends to controller

            using (SqlConnection connection = new SqlConnection(GetConnectionString()))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM VILLAIN", connection);

                SqlDataReader reader = command.ExecuteReader();

                try
                {
                    while (reader.Read())
                    {
                        int VillainID = reader.GetInt32(0);
                        string VillainName = reader.GetString(1);
                        int VillainHealth = reader.GetInt32(2);
                        string ImageFileName = reader.GetString(3);

                        villains.Add(new Villain(VillainID, VillainName, VillainHealth, ImageFileName));
                    }
                }
                catch (System.Exception e)
                {
                    throw new Exception("Unable to get all villains from the database: " + e.Message);
                }
            }

            return villains;
        }

        public List<GameResults> GetAllGamesFromDB()
        {
            List<GameResults> gameResults = new List<GameResults>();

            using (SqlConnection connection = new SqlConnection(GetConnectionString()))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM GAMERESULTS ORDER BY CREATED DESC", connection);

                SqlDataReader reader = command.ExecuteReader();

                try
                {
                    while (reader.Read())
                    {
                        int GameResultsID = reader.GetInt32(0);
                        DateTime Created = reader.GetDateTime(1);
                        string WinnerName = reader.GetString(2);

                        gameResults.Add(new GameResults(GameResultsID, Created, WinnerName));
                    }
                }
                catch (System.Exception e)
                {
                    throw new Exception("Unable to get all games from the database: " + e.Message);
                }
            }

            return gameResults;
        }

        public int SaveGameResults(GameResults gameResults) 
        {
            int rows = 0;

            using (SqlConnection connection = new SqlConnection(GetConnectionString()))
            {
                try
                {
                    connection.Open();

                    string query = "INSERT INTO GAMERESULTS (Created, WinnerName) " +
                                    "VALUES (@Created, @WinnerName)";
                    SqlCommand command = new SqlCommand(query, connection);

                    SqlParameter createdParameter = new SqlParameter() {
                        ParameterName = "@Created",
                        SqlDbType = SqlDbType.DateTime2,
                        Value = gameResults.Created
                    };

                    SqlParameter winnerNameParameter = new SqlParameter() {
                        ParameterName = "@WinnerName",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = gameResults.WinnerName
                    };

                    command.Parameters.Add(createdParameter);
                    command.Parameters.Add(winnerNameParameter);

                    rows = command.ExecuteNonQuery();
                }
                catch (System.Exception e)
                {
                    throw new Exception("Unable to add a new game result to the database: " + e.Message);
                }
            }

            return rows;
        }

    }
}

