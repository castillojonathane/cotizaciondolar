using System.Data.Common;
using System.Threading;
using CotizacionDolarBNA.Models;
using CotizacionDolarBNA.Response;
using CotizacionDolarBNA.Services;
using Microsoft.AspNetCore.Mvc;

namespace CotizacionDolarBNA.Controllers;

[ApiController]
public class CotizacionBNAController : ControllerBase
{
    private readonly CotizacionDolarService _service;

    public CotizacionBNAController(CotizacionDolarService service)
    {
        _service = service;
    }

    [HttpPost]
    [Route("CotizacionBNA")]
    public async Task AddCotizacion()
    {
        await _service.addCotizacion();
    }
    
    [HttpGet]
    [Route("CotizacionBNA")]
    public async Task<ActionResult<ResponseListCotizacion>> GetCotizacionBNA()
    {
        try
        {
            return Ok(await _service.GetCotizacionBNA());
        }
        catch (Exception ex)
        {
            return BadRequest("Ha ocurrido un problema obteniendo la ultima cotización del dia de hoy. " + ex);
        }
    }

    [HttpGet]
    [Route("CotizacionBNA/LastCotizacion")]
    public async Task<ActionResult<ResponseCotizacion>> GetLastCotizacion()
    {
        try
        {
            return Ok(await _service.GetLastCotizacion());
        }
        catch (Exception ex)
        {
            return BadRequest("Ha ocurrido un problema obteniendo la ultima cotización del dia de hoy. " + ex);
        }
    }

    [HttpGet]
    [Route("CotizacionBNA/LastMonthCotizacion")]
    public async Task<ActionResult<ResponseListCotizacion>> GetLastMonthCotizacion()
    {
        try
        {
            return Ok(await _service.GetLastMonthCotizacion());
        }
        catch (Exception ex)
        {
            return BadRequest("Ha ocurrido un problema obteniendo la ultima cotización del dia de hoy. " + ex);
        }
    }

    [HttpGet]
    [Route("CotizacionBNA/LastWeekCotizacion")]
    public async Task<ActionResult<ResponseListCotizacion>> GetLastWeekCotizacion()
    {
        try
        {
            return Ok(await _service.GetLastWeekCotizacion());
        }
        catch (Exception ex)
        {
            return BadRequest("Ha ocurrido un problema obteniendo la ultima cotización del dia de hoy. " + ex);
        }
    }

    [HttpGet]
    [Route("CotizacionBNA/CotizacionesByDateRange/{fechaInicial}/{fechaFinal}")]
    public async Task<ActionResult<ResponseListCotizacion>> GetCotizacionesByDateRange(DateTime fechaInicial, DateTime fechaFinal)
    {
        try
        {
            return Ok(await _service.GetCotizacionesByDateRange(fechaInicial, fechaFinal));
        }
        catch (Exception ex)
        {
            return BadRequest("Ha ocurrido un problema obteniendo la ultima cotización del dia de hoy. " + ex);
        }
    }
}
