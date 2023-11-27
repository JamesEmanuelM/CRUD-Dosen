using System;
using System.Data.SqlClient;
using System.Data;
using Npgsql;

class Program
{                                   //contoh aja
    static string connectionString = "Host=localhost;Port=5432;User ID=postgres;Password=' ';Database=mahasiswa"; 
    static void Main()
    {
        CreateDosenTable();

        // Menambahkan data dosen
        TambahDosen("Dr. John Doe", "Matematika", "john.doe@email.com");

        // Membaca semua data dosen
        Console.WriteLine("Semua Dosen:");
        BacaSemuaDosen();

        // Membaca data dosen berdasarkan ID
        int dosenId = 1;
        Console.WriteLine($"\nDosen dengan ID {dosenId}:");
        BacaDosenById(dosenId);

        // Memperbarui data dosen
        UpdateDosen(dosenId, "Dr. John Doe", "Statistika", "john.doe@email.com");

        // Membaca kembali data dosen setelah pembaruan
        Console.WriteLine("\nSemua Dosen Setelah Pembaruan:");
        BacaSemuaDosen();

        // Menghapus data dosen
        HapusDosen(2);

        // Membaca kembali data dosen setelah penghapusan
        Console.WriteLine("\nSemua Dosen Setelah Penghapusan:");
        BacaSemuaDosen();
    }

    static void CreateDosenTable()
    {
        using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();

            string createTableQuery = @"
                CREATE TABLE IF NOT EXISTS Dosen (
                    id NVARCHAR(10) PRIMARY KEY IDENTITY(1,1),
                    nama NVARCHAR(100) NOT NULL,
                    nidn NVARCHAR(50) NOT NULL,
                    nip NVARCHAR(100) NOT NULL
                 
                )";

            using (NpgsqlCommand command = new NpgsqlCommand(createTableQuery, connection))
            {
                command.ExecuteNonQuery();
            }
        }
    }

    static void TambahDosen(string nama, string nidn, string nip)
    {
        using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();

            string insertQuery = @"
                INSERT INTO Dosen (nama, nidn, nip)
                VALUES (@nama, @nidn, @nip)";

            using (NpgsqlCommand command = new NpgsqlCommand(insertQuery, connection))
            {
                command.Parameters.AddWithValue("@nama", nama);
                command.Parameters.AddWithValue("@nidn", nidn);
                command.Parameters.AddWithValue("@nip", nip);
                

                command.ExecuteNonQuery();
            }
        }
    }

    static void BacaSemuaDosen()
    {
        using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();

            string selectAllQuery = "SELECT * FROM Dosen";

            using (NpgsqlCommand command = new NpgsqlCommand(selectAllQuery, connection))
            {
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"ID: {reader["id"]}, Nama: {reader["nama"]}, NIDN: {reader["nidn"]}, NIP: {reader["nip"]}");
                    }
                }
            }
        }
    }

    static void BacaDosenById(int dosenId)
    {
        using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();

            string selectByIdQuery = "SELECT * FROM Dosen WHERE id = @id";

            using (NpgsqlCommand command = new NpgsqlCommand(selectByIdQuery, connection))
            {
                command.Parameters.AddWithValue("@id", dosenId);

                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"ID: {reader["id"]}, Nama: {reader["nama"]}, NIDN: {reader["nidn"]}, NIP: {reader["nip"]}");
                    }
                }
            }
        }
    }

    static void UpdateDosen(int dosenId, string nama, string nidn, string nip)
    {
        using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();

            string updateQuery = @"
                UPDATE Dosen
                SET nama = @nama, nidn = @nidn, nip = @nip
                WHERE id = @id";

            using (NpgsqlCommand command = new NpgsqlCommand(updateQuery, connection))
            {
                command.Parameters.AddWithValue("@nama", nama);
                command.Parameters.AddWithValue("@nidn", nidn);
                command.Parameters.AddWithValue("@nip", nip);
                command.Parameters.AddWithValue("@Id", dosenId);

                command.ExecuteNonQuery();
            }
        }
    }

    static void HapusDosen(int dosenId)
    {
        using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();

            string deleteQuery = "DELETE FROM Dosen WHERE id = @id";

            using (NpgsqlCommand command = new NpgsqlCommand(deleteQuery, connection))
            {
                command.Parameters.AddWithValue("@id", dosenId);

                command.ExecuteNonQuery();
            }
        }
    }
}
