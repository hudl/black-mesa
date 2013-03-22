using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeployExporter
{
    class Program
    {
        static void Main(string[] args)
        {
            var repo = new DeployRepository();
            var deploys = repo.Collection.FindAll().ToList();
            using (var csv = new CsvWriter(new StreamWriter("black-mesa.tsv")))
            {
                csv.Configuration.Delimiter = "\t";
                csv.WriteRecords<Deploy>(deploys);
            }
            Console.Read();
        }
    }
}
