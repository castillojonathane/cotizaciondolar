using System;
using System.Data.Common;
using System.Threading.Tasks;
using System.Threading;
using System.Globalization;
using CotizacionDolarBNA.Models;
using CotizacionDolarBNA.Repository.Interface;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using CotizacionDolarBNA.Response;

namespace CotizacionDolarBNA.Services;

public class CotizacionDolarService
{
    private readonly ICotizacionDolar _repository;
    private Timer _timer;

    public CotizacionDolarService(ICotizacionDolar repository)
    {
        _repository = repository;
        _timer = new Timer(async _ => await addCotizacion(), null, TimeSpan.Zero, TimeSpan.FromMinutes(5));
    }

    // public async Task addCotizacion()
    // {
    //     var result = new CotizacionesBna();
    //     try
    //     {
    //         using (var context = new ErreparContext())
    //         {
    //             // Descargar la página de cotizaciones del Banco Nación Argentina
    //             var url = "https://www.bna.com.ar/Personas";
    //             var web = new HtmlWeb();
    //             var doc = await web.LoadFromWebAsync(url);

    //             // Obtener la cotización del billete
    //             var billeteCompra = doc.DocumentNode.SelectSingleNode("//div[@id='billetes']//table[@class='table cotizacion']//tr[1]//td[2]")?.InnerText;
    //             var billeteVenta = doc.DocumentNode.SelectSingleNode("//div[@id='billetes']//table[@class='table cotizacion']//tr[1]//td[3]")?.InnerText;

    //             // Obtener la cotización de divisas del dolar
    //             var divisaCompra = doc.DocumentNode.SelectSingleNode("//div[@id='divisas']//table[@class='table cotizacion']//tr[1]//td[2]")?.InnerText;
    //             var divisaVenta = doc.DocumentNode.SelectSingleNode("//div[@id='divisas']//table[@class='table cotizacion']//tr[1]//td[3]")?.InnerText;


    //             // Convertir las cotizaciones a decimal y guardar en el objeto result
    //             result.CotizacionesId = Guid.NewGuid();
    //             result.Fecha = DateTime.Now;
    //             result.BilleteCompra = decimal.TryParse(billeteCompra, out decimal billeteCompraDecimal) ? (billeteCompraDecimal / 10000m).ToString("0.0000", CultureInfo.GetCultureInfo("en-US")) : null;
    //             result.BilleteVenta = decimal.TryParse(billeteVenta, out decimal billeteVentaDecimal) ? (billeteVentaDecimal / 10000m).ToString("0.0000", CultureInfo.GetCultureInfo("en-US")) : null;
    //             result.DivisaCompra = decimal.TryParse(divisaCompra, out decimal divisaCompraDecimal) ? divisaCompraDecimal.ToString("0.0000", CultureInfo.GetCultureInfo("en-US")) : null;
    //             result.DivisaVenta = decimal.TryParse(divisaVenta, out decimal divisaVentaDecimal) ? divisaVentaDecimal.ToString("0.0000", CultureInfo.GetCultureInfo("en-US")) : null;

    //             // Guardar el objeto result en la base de datos
    //             await context.CotizacionesBnas.AddAsync(result);
    //             await context.SaveChangesAsync();
    //         }
    //     }
    //     catch (Exception ex)
    //     {
    //         throw new Exception("Ha ocurrido un problema guardando la cotizacion actual. ", ex);
    //     }
    // }
    public async Task addCotizacion()
    {
        var result = new CotizacionesBna();
        try
        {
            using (var context = new ErreparContext())
            {
                // Descargar la página de cotizaciones del Banco Nación Argentina
                var url = "https://www.bna.com.ar/Personas";
                var web = new HtmlWeb();
                var doc = await web.LoadFromWebAsync(url);

                // Obtener la cotización del billete
                var billeteCompra = doc.DocumentNode.SelectSingleNode("//div[@id='billetes']//table[@class='table cotizacion']//tr[1]//td[2]")?.InnerText;
                var billeteVenta = doc.DocumentNode.SelectSingleNode("//div[@id='billetes']//table[@class='table cotizacion']//tr[1]//td[3]")?.InnerText;

                // Obtener la cotización de divisas del dolar
                var divisaCompra = doc.DocumentNode.SelectSingleNode("//div[@id='divisas']//table[@class='table cotizacion']//tr[1]//td[2]")?.InnerText;
                var divisaVenta = doc.DocumentNode.SelectSingleNode("//div[@id='divisas']//table[@class='table cotizacion']//tr[1]//td[3]")?.InnerText;

                // Validar que los valores obtenidos no sean null o vacíos
                if (string.IsNullOrEmpty(billeteCompra) || string.IsNullOrEmpty(billeteVenta) || string.IsNullOrEmpty(divisaCompra) || string.IsNullOrEmpty(divisaVenta))
                {
                    return;
                }

                // Convertir las cotizaciones a decimal y guardar en el objeto result
                result.CotizacionesId = Guid.NewGuid();
                result.Fecha = DateTime.Now;
                result.BilleteCompra = decimal.TryParse(billeteCompra, out decimal billeteCompraDecimal) ? (billeteCompraDecimal / 10000m).ToString("0.0000", CultureInfo.GetCultureInfo("en-US")) : null;
                result.BilleteVenta = decimal.TryParse(billeteVenta, out decimal billeteVentaDecimal) ? (billeteVentaDecimal / 10000m).ToString("0.0000", CultureInfo.GetCultureInfo("en-US")) : null;
                result.DivisaCompra = decimal.TryParse(divisaCompra, out decimal divisaCompraDecimal) ? divisaCompraDecimal.ToString("0.0000", CultureInfo.GetCultureInfo("en-US")) : null;
                result.DivisaVenta = decimal.TryParse(divisaVenta, out decimal divisaVentaDecimal) ? divisaVentaDecimal.ToString("0.0000", CultureInfo.GetCultureInfo("en-US")) : null;

                // Guardar el objeto result en la base de datos
                await context.CotizacionesBnas.AddAsync(result);
                await context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Ha ocurrido un problema guardando la cotizacion actual. ", ex);
        }
    }

    public async Task<ActionResult<ResponseCotizacion>> GetLastCotizacion()
    {
        var result = new ResponseCotizacion();
        try
        {
            var cotizaciones = await _repository.GetLastCotizacion();
            if (cotizaciones != null)
            {
                result.Fecha = cotizaciones.Fecha;
                result.BilleteCompra = cotizaciones.BilleteCompra;
                result.BilleteVenta = cotizaciones.BilleteVenta;
                result.DivisaCompra = cotizaciones.DivisaCompra;
                result.DivisaVenta = cotizaciones.DivisaVenta;
                result.Ok = true;
                result.StatusCode = "200";
            }
            else
            {
                result.SetError("Error interno en el servidor.");
                result.StatusCode = "500";
            }
            return result;
        }
        catch (Exception ex)
        {
            throw new Exception("Ha ocurrido un problema obteniendo la ultima cotización del dia de hoy. ", ex);
        }
    }

    public async Task<ActionResult<ResponseListCotizacion>> GetLastMonthCotizacion()
    {
        var result = new ResponseListCotizacion();
        try
        {
            var cotizaciones = await _repository.GetLastMonthCotizacion();
            if (cotizaciones != null)
            {
                foreach (var cotizacion in cotizaciones)
                {
                    var resultAux = new ResponseCotizacionItem
                    {
                        Fecha = cotizacion.Fecha,
                        BilleteCompra = cotizacion.BilleteCompra,
                        BilleteVenta = cotizacion.BilleteVenta,
                        DivisaCompra = cotizacion.DivisaCompra,
                        DivisaVenta = cotizacion.DivisaVenta
                    };
                    result.Ok = true;
                    result.StatusCode = "200";
                    result.listCotizacion.Add(resultAux);
                }
            }
            else
            {
                result.SetError("Error interno en el servidor.");
                result.StatusCode = "500";
            }
            return result;
        }
        catch (Exception ex)
        {
            throw new Exception("Ha ocurrido un problema obteniendo la ultima cotización del mes. ", ex);
        }
    }

    public async Task<ActionResult<ResponseListCotizacion>> GetLastWeekCotizacion()
    {
        var result = new ResponseListCotizacion();
        try
        {
            var cotizaciones = await _repository.GetLastWeekCotizacion();
            if (cotizaciones != null)
            {
                foreach (var cotizacion in cotizaciones)
                {
                    var resultAux = new ResponseCotizacionItem
                    {
                        Fecha = cotizacion.Fecha,
                        BilleteCompra = cotizacion.BilleteCompra,
                        BilleteVenta = cotizacion.BilleteVenta,
                        DivisaCompra = cotizacion.DivisaCompra,
                        DivisaVenta = cotizacion.DivisaVenta
                    };
                    result.Ok = true;
                    result.StatusCode = "200";
                    result.listCotizacion.Add(resultAux);
                }
            }
            else
            {
                result.SetError("Error interno en el servidor.");
                result.StatusCode = "500";
            }
            return result;
        }
        catch (Exception ex)
        {
            throw new Exception("Ha ocurrido un problema obteniendo la ultima cotización de la semana. ", ex);
        }
    }

    public async Task<ActionResult<ResponseListCotizacion>> GetCotizacionBNA()
    {
        var result = new ResponseListCotizacion();
        try
        {
            var cotizaciones = await _repository.GetCotizacionBNA();
            if (cotizaciones != null)
            {
                foreach (var cotizacion in cotizaciones)
                {
                    var resultAux = new ResponseCotizacionItem
                    {
                        Fecha = cotizacion.Fecha,
                        BilleteCompra = cotizacion.BilleteCompra,
                        BilleteVenta = cotizacion.BilleteVenta,
                        DivisaCompra = cotizacion.DivisaCompra,
                        DivisaVenta = cotizacion.DivisaVenta
                    };
                    result.Ok = true;
                    result.StatusCode = "200";
                    result.listCotizacion.Add(resultAux);
                }
            }
            else
            {
                result.SetError("Error interno en el servidor.");
                result.StatusCode = "500";
            }
            return result;
        }
        catch (Exception ex)
        {
            throw new Exception("Ha ocurrido un problema obteniendo toda la cotización completa. ", ex);
        }
    }

    public async Task<ActionResult<ResponseListCotizacion>> GetCotizacionesByDateRange(DateTime fechaInicial, DateTime fechaFinal)
    {
        var result = new ResponseListCotizacion();
        try
        {
            var cotizaciones = await _repository.GetCotizacionesByDateRange(fechaInicial, fechaFinal);
            if (cotizaciones != null)
            {
                foreach (var cotizacion in cotizaciones)
                {
                    var resultAux = new ResponseCotizacionItem
                    {
                        Fecha = cotizacion.Fecha,
                        BilleteCompra = cotizacion.BilleteCompra,
                        BilleteVenta = cotizacion.BilleteVenta,
                        DivisaCompra = cotizacion.DivisaCompra,
                        DivisaVenta = cotizacion.DivisaVenta
                    };
                    result.Ok = true;
                    result.StatusCode = "200";
                    result.listCotizacion.Add(resultAux);
                }
            }
            else
            {
                result.SetError("Error interno en el servidor.");
                result.StatusCode = "500";
            }
            return result;
        }
        catch (Exception ex)
        {
            throw new Exception("Ha ocurrido un problema obteniendo la ultima cotización entre esas fechas. ", ex);
        }
    }
}
