using System;
using System.Collections.Generic;

namespace CotizacionDolarBNA.Models;

public partial class CotizacionesBna
{
    public Guid CotizacionesId { get; set; }

    public DateTime Fecha { get; set; }

    public string? BilleteCompra { get; set; }

    public string? BilleteVenta { get; set; }

    public string? DivisaCompra { get; set; }

    public string? DivisaVenta { get; set; }
}
