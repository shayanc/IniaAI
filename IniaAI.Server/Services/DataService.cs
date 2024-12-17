using ClosedXML.Excel;
using IniaAI.Server.Models;

namespace IniaAI.Server.Services
{
    public class DataService : IDataService
    {
        private readonly string _excelFilePath;

        public DataService(IWebHostEnvironment webHostEnvironment)
        {
            _excelFilePath = Path.Combine(webHostEnvironment.ContentRootPath, "Data", "WEOOct2023all.xlsx");
        }

        public async Task<IEnumerable<DataPoint>> GetDataAsync(string country, string subject)
        {
            var dataPoints = new List<DataPoint>();

            // Use constants to improve readability and maintainability
            const int startYear = 1980;
            const int endYear = 2023;
            const int dataStartColumnIndex = 10; // Column 'J' for year 1980

            await Task.Yield(); // Yield control to ensure async flow without Task.Run

            try
            {
                // Load workbook and worksheet
                using var workbook = new XLWorkbook(_excelFilePath);
                var worksheet = workbook.Worksheet(1);

                // Skip the header and filter rows
                var filteredRows = worksheet
                    .RowsUsed()
                    .Skip(1) // Skip header
                    .Where(row =>
                        row.Cell(4).GetString().Equals(country, StringComparison.OrdinalIgnoreCase) && // Column D
                        row.Cell(5).GetString().Equals(subject, StringComparison.OrdinalIgnoreCase));  // Column E

                // Parse year-based data for each row
                foreach (var row in filteredRows)
                {
                    dataPoints.AddRange(
                        Enumerable.Range(startYear, endYear - startYear + 1)
                            .Select(year => new
                            {
                                Year = year,
                                CellValue = row.Cell(dataStartColumnIndex + (year - startYear)).GetString()
                            })
                            .Where(cell =>
                                !string.IsNullOrEmpty(cell.CellValue) &&
                                !cell.CellValue.Equals("n/a", StringComparison.OrdinalIgnoreCase) &&
                                double.TryParse(cell.CellValue, out _))
                            .Select(cell => new DataPoint
                            {
                                Year = cell.Year,
                                Value = double.Parse(cell.CellValue)
                            }));
                }
            }
            catch (Exception ex)
            {
                // Log the exception (consider using a logging framework)
                Console.Error.WriteLine($"Error reading data: {ex.Message}");
            }

            return dataPoints;
        }

    }
}