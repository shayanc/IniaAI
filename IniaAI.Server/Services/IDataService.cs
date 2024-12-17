using IniaAI.Server.Models;

namespace IniaAI.Server.Services
{

    /// <summary>
    /// Return data for a given country and subject id.
    /// </summary>
    public interface IDataService
    {
        Task<IEnumerable<DataPoint>> GetDataAsync(string country, string subject);
    }
}