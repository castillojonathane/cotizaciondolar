namespace CotizacionDolarBNA.Response;

public class ResponseListCotizacion : ResponseBase
{
    public List<ResponseCotizacionItem> listCotizacion { get; set; } = new List<ResponseCotizacionItem>();
}

public class ResponseCotizacionItem
{
    public DateTime? Fecha { get; set; }

    public string? BilleteCompra { get; set; }

    public string? BilleteVenta { get; set; }

    public string? DivisaCompra { get; set; }

    public string? DivisaVenta { get; set; }
}
