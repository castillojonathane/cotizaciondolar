using CotizacionDolarBNA.Models;

namespace CotizacionDolarBNA.Repository.Interface;

public interface ICotizacionDolar
{
    Task<IEnumerable<CotizacionesBna>> GetCotizacionBNA();
    Task<CotizacionesBna> GetLastCotizacion();
    Task<IEnumerable<CotizacionesBna>> GetLastWeekCotizacion();
    Task<IEnumerable<CotizacionesBna>> GetLastMonthCotizacion();
    Task<IEnumerable<CotizacionesBna>> GetCotizacionesByDateRange(DateTime fechaInicial, DateTime fechaFinal);
}
