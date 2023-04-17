using CotizacionDolarBNA.Models;
using CotizacionDolarBNA.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace CotizacionDolarBNA.Repository.Repository;

public class CotizacionDolar : ICotizacionDolar
{
    private readonly ErreparContext _context;
    public CotizacionDolar(ErreparContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CotizacionesBna>> GetCotizacionBNA()
    {
        var cotizacion = await _context.CotizacionesBnas
        .OrderBy(c => c.Fecha)
        .ToListAsync();

        var ultimasCotizaciones = cotizacion
        .GroupBy(c => c.Fecha.Date) // agrupar por fecha
        .Select(g => g.Last()) // seleccionar el último registro de cada grupo
        .ToList();

        return ultimasCotizaciones;
    }

    public async Task<IEnumerable<CotizacionesBna>> GetCotizacionesByDateRange(DateTime fechaInicial, DateTime fechaFinal)
    {
        var cotizaciones = await _context.CotizacionesBnas
        .Where(c => c.Fecha >= fechaInicial && c.Fecha.Date <= fechaFinal)
        .OrderBy(c => c.Fecha)
        .ToListAsync();

        var ultimasCotizaciones = cotizaciones
            .GroupBy(c => c.Fecha.Date) // Agrupar por fecha sin tener en cuenta la hora
            .Select(g => g.Last()) // Seleccionar el último registro de cada grupo
            .ToList();

        return ultimasCotizaciones;
    }

    public async Task<CotizacionesBna> GetLastCotizacion()
    {
        var cotizacion = await _context.CotizacionesBnas
            .OrderBy(c => c.Fecha)
            .LastOrDefaultAsync();
        if (cotizacion.BilleteCompra == null || cotizacion.BilleteVenta == null || cotizacion.DivisaCompra == null || cotizacion.DivisaVenta == null)
        {
            var cotizacionAnterior = await _context.CotizacionesBnas
                .Where(c => c.Fecha < cotizacion.Fecha)
                .OrderByDescending(c => c.Fecha)
                .FirstOrDefaultAsync();

            if (cotizacionAnterior == null)
            {
                throw new Exception("La pagina https://bna.com.ar/Personas no se encuentra disponible para obtener los datos.");
            }
            else
            {
                return cotizacionAnterior;
            }
        }
        else
        {
            return cotizacion;
        }
    }

    public async Task<IEnumerable<CotizacionesBna>> GetLastMonthCotizacion()
    {
        DateTime fechaActual = DateTime.Now.Date;
        DateTime fechaHace30Dias = fechaActual.AddDays(-30);

        var cotizaciones = await _context.CotizacionesBnas
        .Where(c => c.Fecha >= fechaHace30Dias && c.Fecha.Date <= fechaActual)
        .OrderBy(c => c.Fecha)
        .ToListAsync();

        var ultimasCotizaciones = cotizaciones
        .GroupBy(c => c.Fecha.Date) // Agrupar por fecha sin tener en cuenta la hora
        .Select(g => g.Last()) // Seleccionar el último registro de cada grupo
        .ToList();

        return ultimasCotizaciones;
    }

    public async Task<IEnumerable<CotizacionesBna>> GetLastWeekCotizacion()
    {
        DateTime fechaActual = DateTime.Now.Date;
        DateTime fechaSemanaAtras = fechaActual.AddDays(-7);

        var cotizaciones = await _context.CotizacionesBnas
        .Where(c => c.Fecha >= fechaSemanaAtras && c.Fecha.Date <= fechaActual)
        .OrderBy(c => c.Fecha)
        .ToListAsync();

        var ultimasCotizaciones = cotizaciones
        .GroupBy(c => c.Fecha.Date) // Agrupar por fecha sin tener en cuenta la hora
        .Select(g => g.Last()) // Seleccionar el último registro de cada grupo
        .ToList();

        return ultimasCotizaciones;
    }
}
