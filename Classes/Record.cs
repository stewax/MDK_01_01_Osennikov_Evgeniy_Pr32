using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinylRecordsApplication.Classes
{
    public class Record
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
        public int Format { get; set; }
        public int Size { get; set; }
        public int IdManufacturer { get; set; }
        public float Price { get; set; }
        public int IdState { get; set; }
        public string Description { get; set; }

        public static IEnumerable<Record> AllRecords()
        {
            List<Record> records = new List<Record>();
            DataTable recordQuery = Classes.DBConnection.Connection("SELECT * FROM [dbo].[Record]");
            foreach (DataRow row in recordQuery.Rows)
            {
                records.Add(new Record()
                {
                    Id = Convert.ToInt32(row[0]),
                    Name = row[1].ToString(),
                    Year = Convert.ToInt32(row[2]),
                    Format = Convert.ToInt32(row[3]),
                    Size = Convert.ToInt32(row[4]),
                    IdManufacturer = Convert.ToInt32(row[5]),
                    Price = float.Parse(row[6].ToString()),
                    IdState = Convert.ToInt32(row[7]),
                    Description = row[8].ToString()
                });
            }
            return records;
        }

        public void Save(bool Update = false)
        {
            string CorrectPrice = this.Price.ToString().Replace(",", ".");

            if (!Update)
            {
                Classes.DBConnection.Connection(
                    "INSERT INTO " +
                    "[dbo].[Record](" +
                    "[Name], " +
                    "[Year], " +
                    "[Format], " +
                    "[Size], " +
                    "[IdManufacturer], " +
                    "[Price], " +
                    "[IdState], " +
                    "[Description]) " +
                    "VALUES(" +
                    $"N'{this.Name}', " +
                    $"{this.Year}, " +
                    $"{this.Format}, " +
                    $"{this.Size}, " +
                    $"{this.IdManufacturer}, " +
                    $"{CorrectPrice}, " +
                    $"{this.IdState}, " +
                    $"N'{this.Description}');"
                );
                this.Id = Record.AllRecords().Where(
                    x => x.Name == this.Name &&
                         x.Year == this.Year &&
                         x.Format == this.Format &&
                         x.Size == this.Size &&
                         x.IdManufacturer == this.IdManufacturer &&
                         x.IdState == this.IdState &&
                         x.Description == this.Description
                ).First().Id;
            }
            else
            {
                Classes.DBConnection.Connection(
                    "UPDATE [dbo].[Record] " +
                    $"SET [Name] = N'{this.Name}', " +
                    $"[Year] = {this.Year}, " +
                    $"[Format] = {this.Format}, " +
                    $"[Size] = {this.Size}, " +
                    $"[IdManufacturer] = {this.IdManufacturer}, " +
                    $"[Price] = {CorrectPrice}, " +
                    $"[IdState] = {this.IdState}, " +
                    $"[Description] = N'{this.Description}' " +
                    $"WHERE [Id] = {this.Id};"
                );
            }
        }

        public void Delete()
        {
            Classes.DBConnection.Connection($"DELETE FROM [dbo].[Record] WHERE [Id] = {this.Id};");
        }
    }
}
