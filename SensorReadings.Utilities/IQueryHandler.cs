using System.Threading.Tasks;

namespace SensorReadings.Utilities
{
    public interface IQueryHandler<TIn, TOut>
    {
        Task<TOut> Execute(TIn query);
    }
}
