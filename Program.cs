using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace ConsoleApp

{

    class Program
    {

        static void Main(string[] args)
        {

            Crimes crimes = new Crimes(args);

        }

    }

}

namespace ConsoleApp

{

    public class Crimes
    {

        private static List<CrimeData> CrimeDataList = new List<CrimeData>();

        public Crimes(string[] args)

        {

            string csvFilePath = string.Empty;

            string reportFilePath = string.Empty;

            string startupPath = Directory.GetCurrentDirectory();

// Debug Mode

        if (Debugger.IsAttached)
        {
            csvFilePath = Path.Combine(startupPath, "CrimeData.csv");

            reportFilePath = Path.Combine(startupPath, "CrimeReport.txt");
        }

        else

        {
            if (args.Length != 2)
            {

                Console.WriteLine("Invalid call.\n Valid call example : CrimeAnalyzer <crime_csv_file_path> <report_file_path>");

                Console.ReadLine();

            return;

            }

       else

        {

            csvFilePath = args[0];

            reportFilePath = args[1];

        if (!csvFilePath.Contains("\\"))

        {

            csvFilePath = Path.Combine(startupPath, csvFilePath);

        }

        if (!reportFilePath.Contains("\\"))

        {

        reportFilePath = Path.Combine(startupPath, reportFilePath);

        }

    }

}

        if (File.Exists(csvFilePath))

        {

        if (ReadData(csvFilePath))

        {

              try

        {

        var file = File.Create(reportFilePath);

        file.Close();

        }

        catch (Exception fe)

        {

            Console.WriteLine($"Unable to create report file at : {reportFilePath}");

        }

            WriteReport(reportFilePath);

        }

     }

        else

        {

            Console.Write($"Crime data file does not exist at path: {csvFilePath}");
    
        }

        Console.ReadLine();

        }

        private static bool ReadData(string filePath)

        {

        Console.WriteLine($"Reading data from file : {filePath}");

        try

        {

        int columns = 0;

        string[] crimeDataLines = File.ReadAllLines(filePath);

        for (int index = 0; index < crimeDataLines.Length; index++)

        {

        string crimeDataLine = crimeDataLines[index];

        string[] data = crimeDataLine.Split(',');

        if (index == 0) // Header

        {

            columns = data.Length;

        }

        else

        {

        if (columns != data.Length)

        {

            Console.WriteLine($"Row {index} contains {data.Length} values. It should contain {columns}.");

        return false;

        }

    else

    {

        try
             {

                CrimeData crimeData = new CrimeData();

                crimeData.Year = Convert.ToInt32(data[0]);

                crimeData.Population = Convert.ToInt32(data[1]);

                crimeData.Murders = Convert.ToInt32(data[2]);

                crimeData.Rapes = Convert.ToInt32(data[3]);

                crimeData.Robberies = Convert.ToInt32(data[4]);

                crimeData.ViolentCrimes = Convert.ToInt32(data[5]);

                crimeData.Thefts = Convert.ToInt32(data[6]);

                crimeData.MotorVehicleThefts = Convert.ToInt32(data[7]);

                CrimeDataList.Add(crimeData);

                }

        catch (InvalidCastException e)

        {

            Console.WriteLine($"Row {index} contains invalid value.");

            return false;

            }

        }

    }

}

            Console.WriteLine($"Data read completed successfully.");

            return true;

            }

            catch (Exception ex)

            {

                Console.WriteLine("Error in reading data from csv file.");

                throw ex;

            }

        }

           private static void WriteReport(string filePath)

            {

                try

                    {

                        if (CrimeDataList != null && CrimeDataList.Any())

                    {

                        Console.WriteLine($"Calculating the desired data and writing it to report file : {filePath}");

                        StringBuilder sb = new StringBuilder();

                        sb.Append("Crime Analyzer Report");

                        sb.Append(Environment.NewLine);

            // Question 1

int minYear = CrimeDataList.Min(x => x.Year);

int maxYear = CrimeDataList.Max(x => x.Year);

            // Years

int years = maxYear - minYear + 1;

sb.Append($"Period: {minYear}-{maxYear} ({years} years)");

sb.Append(Environment.NewLine);

            // Question 2

var mYears = from crimeData in CrimeDataList

where crimeData.Murders < 15000

select crimeData.Year;

string mYearsStr = string.Empty;

for (int i = 0; i < mYears.Count(); i++)

{

mYearsStr += mYears.ElementAt(i).ToString();

// No comma after last value

if (i < mYears.Count() - 1) mYearsStr += ", ";

}

sb.Append($"Years murders per year < 15000: {mYearsStr}");

sb.Append(Environment.NewLine);

        // Question 3

var rYears = from crimeData in CrimeDataList

where crimeData.Robberies > 500000

select crimeData;

string rYearsStr = string.Empty;

for (int i = 0; i < rYears.Count(); i++)

{

CrimeData crimeData = rYears.ElementAt(i);

rYearsStr += $"{crimeData.Year} = {crimeData.Robberies}";



if (i < rYears.Count() - 1) rYearsStr += ", ";

}

sb.Append($"Robberies per year > 500000: {rYearsStr}");

sb.Append(Environment.NewLine);

        // Question 4

var vCrime = from crimeData in CrimeDataList

where crimeData.Year == 2010

select crimeData;

CrimeData vCrimeData = vCrime.First();

double vCrimePerCapita = (double)vCrimeData.ViolentCrimes / (double)vCrimeData.Population;

sb.Append($"Violent crime per capita rate (2010): {vCrimePerCapita}");

sb.Append(Environment.NewLine);

        // Question 5

double avgMurders = CrimeDataList.Sum(x => x.Murders) / CrimeDataList.Count;

sb.Append($"Average murder per year (all years): {avgMurders}");

sb.Append(Environment.NewLine);

        // Question 6

int murders1 = CrimeDataList

.Where(x => x.Year >= 1994 && x.Year <= 1997)

.Sum(y => y.Murders);

double avgMurders1 = murders1 / CrimeDataList.Count;

sb.Append($"Average murder per year (1994-1997): {avgMurders1}");

sb.Append(Environment.NewLine);

        // Question 7

int murders2 = CrimeDataList

.Where(x => x.Year >= 2010 && x.Year <= 2014)

.Sum(y => y.Murders);

double avgMurders2 = murders2 / CrimeDataList.Count;

sb.Append($"Average murder per year (2010-2014): {avgMurders2}");

sb.Append(Environment.NewLine);

        // Question 8

int minTheft = CrimeDataList

.Where(x => x.Year >= 1999 && x.Year <= 2004)

.Min(x => x.Thefts);

sb.Append($"Minimum thefts per year (1999-2004): {minTheft}");

sb.Append(Environment.NewLine);

        // Question 9

int maxTheft = CrimeDataList

.Where(x => x.Year >= 1999 && x.Year <= 2004)

.Max(x => x.Thefts);

sb.Append($"Maximum thefts per year (1999-2004): {maxTheft}");

sb.Append(Environment.NewLine);

        // Question 10

int yMaxVehicleTheft = CrimeDataList.OrderByDescending(x => x.MotorVehicleThefts).First().Year;

sb.Append($"Year of highest number of motor vehicle thefts: {yMaxVehicleTheft}");

sb.Append(Environment.NewLine);

using (var stream = new StreamWriter(filePath))

{

stream.Write(sb.ToString());

}

Console.WriteLine();

Console.WriteLine(sb.ToString());

Console.WriteLine();

Console.WriteLine($"Written report file successfully at : {filePath}");

}

else

{

Console.WriteLine($"No data to write.");

}

}

catch (Exception ex)

{

Console.WriteLine("Error in writing report file.");

throw ex;

}

}

}

}

// CrimeData.cs

namespace ConsoleApp

{

public class CrimeData

{

public int Year { get; set; }

public int Population { get; set; }

public int Murders { get; set; }

public int Rapes { get; set; }

public int Robberies { get; set; }

public int ViolentCrimes { get; set; }

public int Thefts { get; set; }

public int MotorVehicleThefts { get; set; }

}

}
/*using System;

using System.Collections.Generic;

using System.Diagnostics;

using System.IO;

using System.Linq;

using System.Text;

namespace CrimeAnalyzer

{
    
    public class crimeStats
    {
      public int year;
      public int pop;
      public int vCrime;
      public int murder;
      public int rape;
      public int rob;
      public int agAssault;
      public int propCrime;
      public int burglary;
      public int theft;
      public int mvtheft;
   
      public class crimeStats(int year, int pop, int vCrime, int murder, int rape, int rob, int agAssault, int propCrime, int burglary, int theft, int mvtheft)
      {
        this.year = year;
        this.pop = pop;
        this.vCrime = vCrime;
        this.murder = murder;
        this.rape = rape; 
        this.rob = rob;
        this.agAssault = agAssault;
        this.propCrime = propCrime;
        this.burglary = burglary;
        this.theft = theft;
        this.mvtheft = mvtheft;
      }
        
     }
      
       class program
        {

        private static void Main(string[] args)
        {

            string cvsFile = args[0];
            string reportFile = args[1];
            List<crimeStats> list = new List<crimeStats>();
            int count = 0;

            if (args.Length != 2) 
            {
                Console.WriteLine("Format is incorrect. It should be \n dotnet CrimeAnalyzer.dll <csv_file_path> <report_file_path>  \n");
                Environment.Exit(-1);
            }
      
          cvsFile = args[0];

            if (File.Exists(csvFile) == false) // checks if CrimeData.csv file exists
            {
                Console.WriteLine("Nonexistent file. Goodbye. \n");
                Environment.Exit(-1);
            }

            using (var reader = new StreamReader(cvsFile))
            {

                string header = reader.ReadLine(); 
                var hValues = header.Split(','); //stores the header value where they can't hurt anyone

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                   
                    int year = Convert.ToInt32(values[0]);
                    int pop = Convert.ToInt32(values[1]);
                    int vCrime = Convert.ToInt32(values[2]);
                    int murder = Convert.ToInt32(values[3]);
                    int rape = Convert.ToInt32(values[4]);
                    int rob = Convert.ToInt32(values[5]);
                    int agAssault = Convert.ToInt32(values[6]);
                    int propCrime = Convert.ToInt32(values[7]);
                    int burglary = Convert.ToInt32(values[8]);
                    int theft = Convert.ToInt32(values[9]);
                    int mvtheft = Convert.ToInt32(values[10]);

                    list.Add(new crimeStats(year, pop, vCrime, murder, rape, rob, agAssault, propCrime, burglary, theft, mvtheft));            
                }

            }
//Analyzer Report          
          string report = ""; 
                         //Question 1 & 2
            var years = from crimeStats in list select crimeStats.year;
            foreach (var x in years)
            {
                count++;
            }
                         //Question 3 
            var q3Murders = from crimeStats in list where crimeStats.murder < 15000 select crimeStats.year;
                         //Question 4
            var q4Rob = from crimeStats in list where crimeStats.rob > 500000 select new { crimeStats.year, crimeStats.rob };
                         //Question 5
            var q5Violence = from crimeStats in list where crimeStats.year == 2010 select crimeStats.vc;
            var q5Capita = from crimeStats in list where crimeStats.year == 2010 select crimeStats.pop;
            double vcr = 0;
            double cpop = 0;
            foreach(var x in q5Violence)
            {
                vcr = (double)x;
            }

            foreach (var x in q5Capita)
            {
                cpop = (double)x;
            }


            double q5Answer = vcr/cpop;
                       //Question 6
            var q6 = from crimeStats in list select crimeStats.murder;
            double q6Murder = 0;
            foreach (var x in q6)
            {
                q6Murder += x;
            }

            double q6Answer = q6Murder / count;
                      //Question 7
            var q7 = from crimeStats in list where crimeStats.year >= 1994 && crimeStats.year <= 1997 select crimeStats.murder;
            double q7Murder = 0;
            int q7Count = 0;
            foreach (var x in q7)
            {
                q7Murder += x;
                q7Count++;
            }
                     //Question 8
            var q8 = from crimeStats in list where crimeStats.year >= 2010 && crimeStats.year <= 2013 select crimeStats.murder;
            double q8Murder = 0;
            int q8Count = 0;
            foreach (var x in q8)
            {
                q8Murder += x;
                q8Count++;
            }

            double q8Answer = q8Murder / q8Count;

                     //Question 9
            var q9 = from crimeStats in list where crimeStats.year >= 1999 && crimeStats.year <= 2004 select crimeStats.theft;

            int q9Answer = q9.Min();
                    //Question 10
            var q10 = from crimeStats in list where crimeStats.year >= 1999 && crimeStats.year <= 2004 select crimeStats.theft;

            int q10Answer = q10.Max();
                    //Question 11
            var q11 = from crimeStats in list select new { crimeStats.year, crimeStats.mvtheft };
            int q11Answer = 0;
            int temp = 0;

            foreach (var x in q11)
            {
                
                if(x.mvtheft > temp )
                {
                    q11Answer = x.year;
                    temp = x.mvtheft;
                }
            }
          
          report += "The range of years include " + years.Min() + " - " + years.Max() + " (" + count + " years) \n";
          report += "Years murders per year < 15000: ";
            foreach (var x in q3Murders)
            {
                report += x + " ";
            }
          report += "\n";

            report += "Robberies per year > 50000: ";
            foreach (var x in q4Rob)
            {
                report += string.Format("{0} = {1}, ", x.year, x.rob);
            }

            report += "\n";
          
          report += "Violent crime per capita rate (2010): " + q5Answer + "\n";

          report += "Average murder per year (all years): " + q6Answer + "\n";

          report += "Average murder per year (1994-1997): " + q7Answer + "\n";

          report += "Average murder per year (1994-1997): " + q8Answer + "\n";

          report += "Minimum thefts per year (1999-2004): " + q9Answer + "\n";

          report += "Maximumthefts per year (1999-2004): " + q10Answer + "\n";
          
          report += "Year of highest number of motor vehicle thefts: " + q11Answer + "\n";
          
          wFile = "Output.txt";

            StreamWriter sw = new StreamWriter(reportFile);

                try
                 {

                     sw.WriteLine(report);


                 }
                 catch (Exception e)
                 {
                     Console.WriteLine("Exception: " + e.Message);
                 }
                 finally
                 {
                     Console.WriteLine("Executing finally block.");

                     sw.Close();

                 }
            
            

}*/
