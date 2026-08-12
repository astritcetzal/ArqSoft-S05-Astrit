// CitasApp.Infrastructure/Repositories/SqliteMedicoRepository.cs
// Adapter de salida — implementa IMedicoRepository usando SQLite

using Citas_App.Domain.Interfaces;
using Citas_App.Domain.Models;
using Microsoft.Data.Sqlite;

namespace Citas_App.Infrastructure.Repositories
{
    public class SqliteMedicoRepository : IMedicoRepository
    {
        private readonly string _connectionString;

        public SqliteMedicoRepository(string dbPath)
        {
            _connectionString = $"Data Source={dbPath}";
            InicializarTabla();
        }
        private void InicializarTabla()
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS Medicos (
                    Id             INTEGER PRIMARY KEY AUTOINCREMENT,
                    Nombre         TEXT NOT NULL,
                    Apellido       TEXT NOT NULL,
                    Especialidad   TEXT,
                    NumeroLicencia TEXT,
                    Email          TEXT -- ¡Nueva columna agregada!
                );";
            cmd.ExecuteNonQuery();
        }

        private static Medico LeerFila(SqliteDataReader r) => new Medico
        {
            Id = r.GetInt32(0),
            Nombre = r.GetString(1),
            Apellido = r.GetString(2),
            Especialidad = r.IsDBNull(3) ? string.Empty : r.GetString(3),
            NumeroLicencia = r.IsDBNull(4) ? string.Empty : r.GetString(4),
            Email = r.IsDBNull(5) ? string.Empty : r.GetString(5)
        };

        public List<Medico> ObtenerTodos()
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Id, Nombre, Apellido, Especialidad, NumeroLicencia, Email FROM Medicos;";

            var lista = new List<Medico>();
            using var r = cmd.ExecuteReader();
            while (r.Read()) lista.Add(LeerFila(r));
            return lista;
        }

        public Medico? ObtenerPorId(int id)
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Id, Nombre, Apellido, Especialidad, NumeroLicencia, Email FROM Medicos WHERE Id = $id;";
            cmd.Parameters.AddWithValue("$id", id);

            using var r = cmd.ExecuteReader();
            return r.Read() ? LeerFila(r) : null;
        }

        public void Agregar(Medico medico)
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            var cmd = conn.CreateCommand();

            cmd.CommandText = @"
                INSERT INTO Medicos (Nombre, Apellido, Especialidad, NumeroLicencia, Email)
                VALUES (@nombre, @apellido, @especialidad, @numeroLicencia, @email);";

            cmd.Parameters.AddWithValue("@nombre", medico.Nombre ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@apellido", medico.Apellido ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@especialidad", medico.Especialidad ?? string.Empty);
            cmd.Parameters.AddWithValue("@numeroLicencia", medico.NumeroLicencia ?? string.Empty);
            cmd.Parameters.AddWithValue("@email", medico.Email ?? string.Empty);

            cmd.ExecuteNonQuery();
        }


        public void Eliminar(int id)
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM Medicos WHERE Id = $id;";
            cmd.Parameters.AddWithValue("$id", id);
            cmd.ExecuteNonQuery(); conn.Close();
        }

        public void EditaMedico(Medico medico)
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE Medicos SET Nombre = $nombre, Apellido = $apellido, Especialidad = $especialidad, NumeroLicencia = $numeroLicencia, Email = $email WHERE Id = $id;";
            cmd.Parameters.AddWithValue("$nombre", medico.Nombre ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("$apellido", medico.Apellido ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("$especialidad", medico.Especialidad ?? string.Empty);
            cmd.Parameters.AddWithValue("$numeroLicencia", medico.NumeroLicencia ?? string.Empty);
            cmd.Parameters.AddWithValue("$email", medico.Email ?? string.Empty);
            cmd.Parameters.AddWithValue("$id", medico.Id);
            cmd.ExecuteNonQuery();
        }
    }

}
