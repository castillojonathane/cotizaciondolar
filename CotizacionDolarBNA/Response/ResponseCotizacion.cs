namespace CotizacionDolarBNA.Response;

public class ResponseCotizacion : ResponseBase
{
    public DateTime? Fecha { get; set; }

    public string? BilleteCompra { get; set; }

    public string? BilleteVenta { get; set; }

    public string? DivisaCompra { get; set; }

    public string? DivisaVenta { get; set; }
}
