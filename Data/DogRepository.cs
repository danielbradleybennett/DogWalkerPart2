using Microsoft.Data.SqlClient;
using LordsOfDogtown.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DogWalkerPart2.Data
{
    class OwnerRepository
    {
        public SqlConnection Connection
        {
            get
            {
                string _connectionString = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=DogWalkerPart2;Integrated Security=True";
                return new SqlConnection(_connectionString);
            }
        }

        public List<Dog> GetAllDogs()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Name, OwnerId, Breed, Notes FROM Dog";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Dog> dogs = new List<Dog>();

                    while (reader.Read())
                    {
                        int idColumnPosition = reader.GetOrdinal("Id");
                        int idValue = reader.GetInt32(idColumnPosition);

                        int dogNameColumnPosition = reader.GetOrdinal("Name");
                        string dogNameValue = reader.GetString(dogNameColumnPosition);

                        int dogOwnerIdColumnPosition = reader.GetOrdinal("OwnerId");
                        int dogOwnerIdValue = reader.GetInt32(dogOwnerIdColumnPosition);

                        int dogBreedColumnPosition = reader.GetOrdinal("Breed");
                        string dogBreedValue = reader.GetString(dogBreedColumnPosition);

                        int dogNotesColumnPosition = reader.GetOrdinal("Notes");
                        string dogNotesValue = reader.GetString(dogNotesColumnPosition);

                        Dog dog = new Dog
                        {
                            Id = idValue,
                            Name = dogNameValue,
                            OwnerId = dogOwnerIdValue,
                            Breed = dogBreedValue,
                            Notes = dogNotesValue
                        };

                        dogs.Add(dog);
                    }

                    reader.Close();

                    return dogs;
                }
            }
        }

        public Dog GetDogById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Name, OwnerId, Breed, Notes FROM Dog WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    Dog dog = null;

                    if (reader.Read())
                    {
                        dog = new Dog
                        {
                            Id = id,
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            OwnerId = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                            Breed = reader.GetString(reader.GetOrdinal("Breed")),
                            Notes = reader.GetString(reader.GetOrdinal("Notes"))
                        };
                    }

                    reader.Close();

                    return dog;
                }
            }
        }

        public void AddDog(Dog dog)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {

                    cmd.CommandText = "INSERT INTO Dog (Name) OUTPUT INSERTED.Id Values (@Name)";
                    cmd.Parameters.Add(new SqlParameter("@Name", dog.Name));

                    cmd.CommandText = "INSERT INTO Dog (OwnerId) OUTPUT INSERTED.Id Values (@OwnerId)";
                    cmd.Parameters.Add(new SqlParameter("@OwnerId", dog.OwnerId));

                    cmd.CommandText = "INSERT INTO Dog (Breed) OUTPUT INSERTED.Id Values (@Breed)";
                    cmd.Parameters.Add(new SqlParameter("@Breed", dog.Breed));

                    cmd.CommandText = "INSERT INTO Dog (Notes) OUTPUT INSERTED.Id Values (@Notes)";
                    cmd.Parameters.Add(new SqlParameter("@Notes", dog.Notes));

                    int id = (int)cmd.ExecuteScalar();

                    dog.Id = id;
                }
            }

        }

        public void UpdateDog(int id, Dog dog)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Dog
                                     SET Name = @Name
                                     SET OwnerId = @OwnerId
                                     SET Breed = @Breed
                                     SET Notes = @Notes
                                     WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@Name", dog.Name));
                    cmd.Parameters.Add(new SqlParameter("@OwnerId", dog.OwnerId));
                    cmd.Parameters.Add(new SqlParameter("@Breed", dog.Breed));
                    cmd.Parameters.Add(new SqlParameter("@Notes", dog.Notes));
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteDog(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM Dog WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}