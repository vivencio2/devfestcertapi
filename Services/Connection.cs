using devfestcertapi.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace devfestcertapi.Services
{
    public class Connection
    {
        private readonly SqliteConnectionStringBuilder _connectionBuilder;
        public Connection()
        {
            _connectionBuilder = new SqliteConnectionStringBuilder();
            _connectionBuilder.DataSource = "gdgph.db";
        }

        public Response IsTicketNumberValid(string ticketNumber)
        {
            Response response = new Response() { IsValid = false, TicketNumber = ticketNumber };
            using (var connection = new SqliteConnection(_connectionBuilder.ConnectionString))
            {
                connection.Open();
                var query = connection.CreateCommand();
                query.CommandText = string.Format("SELECT FirstName, LastName FROM attendees WHERE OrderNumber = '{0}';", ticketNumber);
                using (var reader = query.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        response.FirstName = reader.GetString(0);
                        response.LastName = reader.GetString(1);
                        response.IsValid = true;
                        return response;
                    }
                }
            }
            return response;
        }
        public bool UpdateAttendeeNameByTicketNumber(string ticketNumber, string firstName, string lastName)
        {
            using (var connection = new SqliteConnection(_connectionBuilder.ConnectionString))
            {
                connection.Open();
                var query = connection.CreateCommand();
                query.CommandText = string.Format("UPDATE attendees SET FirstName = '{0}', LastName = '{1}' WHERE OrderNumber = '{2}';", firstName, lastName, ticketNumber);
                return (query.ExecuteNonQuery() > 0);
            }
        }
    }
}
