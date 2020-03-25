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

        public List<Owner> GetAllOwners()
        {
            using (SqlConnection conn = Connection)
            conn.Open();

            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT Id, Name, Address, NeighborhoodId, Phone FROM Owner";

                SqlDataReader reader = cmd.ExecuteReader();

                List<Owner> owners = new List<Owner>();

                while (reader.Read())
                {
                    int idColumnPosition = reader.GetOrdinal("Id");
                    int idValue = reader.GetInt32(idColumnPosition);

                    int ownerNameColumnPosition = reader.GetOrdinal("Name");
                    string ownerNameValue = reader.GetString(ownerNameColumnPosition);

                    int ownerAddressColumnPosition = reader.GetOrdinal("Address");
                    string ownerAddressValue = reader.GetInt32(ownerAddressColumnPosition);

                    int ownerNeighborhoodColumnPosition = reader.GetOrdinal("Neighborhood");
                    int ownerNeighborhoodIdValue = reader.GetInt32(ownerNeighborhoodColumnPosition);

                    int ownerPhoneColumnPosition = reader.GetOrdinal("Phone");
                    string ownerPhoneValue = reader.GetString(ownerPhoneColumnPosition);

                    Owner owner = new Owner
                    {
                        Id = idValue,
                        Name = ownerNameValue,
                        Address = ownerAddressValue,
                        NeighborhoodId = ownerNeighborhoodIdValue,
                        Phone = ownerPhoneValue
                    };

                    owners.Add(owner);
                }

                reader.Close();

                return owners;
            }
        }

        public Owner GetOwnerById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Name, Address, NeighborhoodId, Phone FROM Owner WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    Owner owner = null;

                    if (reader.Read())
                    {
                        owner = new Owner
                        {
                            Id = id,
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Address = reader.GetString(reader.GetOrdinal("Address")),
                            NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId")),
                            Phone = reader.GetString(reader.GetOrdinal("Phone"))
                        };
                    }

                    reader.Close();

                    return owner;
                }
            }
        }

        public void AddOwner(Owner owner)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {

                    cmd.CommandText = "INSERT INTO Owner (Name) OUTPUT INSERTED.Id Values (@Name)";
                    cmd.Parameters.Add(new SqlParameter("@Name", owner.Name));

                    cmd.CommandText = "INSERT INTO Owner (Address) OUTPUT INSERTED.Id Values (@Address)";
                    cmd.Parameters.Add(new SqlParameter("@Address", owner.Address));

                    cmd.CommandText = "INSERT INTO Dog (NeighborhoodId) OUTPUT INSERTED.Id Values (@NeighborhoodId)";
                    cmd.Parameters.Add(new SqlParameter("@NeighborhoodId", owner.NeighborhoodId));

                    cmd.CommandText = "INSERT INTO Dog (Notes) OUTPUT INSERTED.Id Values (@Phone)";
                    cmd.Parameters.Add(new SqlParameter("@Notes", owner.Phone));

                    int id = (int)cmd.ExecuteScalar();

                    owner.Id = id;
                }
            }

        }

        public void UpdateOwner(int id, Owner owner)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Dog
                                     SET Name = @Name
                                     SET Address = @Address
                                     SET NeighborhoodId = @NeighborhoodId
                                     SET Phone = @Phone
                                     WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@Name", owner.Name));
                    cmd.Parameters.Add(new SqlParameter("@OwnerId", owner.Address));
                    cmd.Parameters.Add(new SqlParameter("@Breed", owner.NeighborhoodId));
                    cmd.Parameters.Add(new SqlParameter("@Notes", owner.Phone));
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteOwner(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM Owner WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}