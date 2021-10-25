using CharactersLib;
using GameLib;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace GameApi.Database
{
    public class GameData
    {
        static string connectionString = "Server=database-2.cnfp1r5tha7w.us-east-1.rds.amazonaws.com,1433;Database=test123;User Id=admin;password=x";
        SqlConnection connection;

        public string Connect()
        {
            connection = new SqlConnection(connectionString);
            connection.Open();

            return "Ok";
        }

        public List<Hero> GetAllHeroesFromDB()
        {
            List<Hero> heroes = new List<Hero>(); //temp array, sends to controller

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM HERO", connection);

                SqlDataReader reader = command.ExecuteReader();

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

            return heroes;
        }

        public List<Villain> GetAllVillainsFromDB()
        {
            List<Villain> villains = new List<Villain>(); //temp array, sends to controller

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM VILLAIN", connection);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int VillainID = reader.GetInt32(0);
                    string VillainName = reader.GetString(1);
                    int VillainHealth = reader.GetInt32(2);
                    string ImageFileName = reader.GetString(3);

                    villains.Add(new Villain(VillainID, VillainName, VillainHealth, ImageFileName));
                }
            }

            return villains;
        }

        public List<GameResults> GetAllGamesFromDB()
        {
            // TO BE COMPLETED
            return null;
        }

    }
}
