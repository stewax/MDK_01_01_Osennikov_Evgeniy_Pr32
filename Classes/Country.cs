using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinylRecordsApplication.Classes
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public static IEnumerable<Country> AllCountries()
        {
            List<Country> countries = new List<Country>();
            DataTable requestCountrys = DBConnection.Connection("SELECT * FROM [dbo].[Country]");
            foreach (DataRow row in requestCountrys.Rows)
            {
                countries.Add(new Country()
                {
                    Id = Convert.ToInt32(row[0]),
                    Name = row[1].ToString()
                });
            }
            return countries;
        }
    }
}
