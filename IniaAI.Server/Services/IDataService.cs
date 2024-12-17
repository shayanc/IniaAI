using IniaAI.Server.Models;

namespace IniaAI.Server.Services
{
    public interface IDataService
    {
        Task<IEnumerable<DataPoint>> GetDataAsync(string country, string subject);
    }
}