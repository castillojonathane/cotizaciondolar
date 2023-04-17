namespace CotizacionDolarBNA.Response;

public class ResponseBase
{
    public bool Ok { get; set; }
    public string Error { get; set; } = null!;
    public string StatusCode { get; set; } = null!;
    public void SetError(string error)
    {
        Ok = false;
        Error = error;
    }
}
