using System.Text.Json.Serialization;

namespace webapi_bilheteria_c.Domain.Models
{
    public class QrCodeResponse
    {
        [JsonPropertyName("qrcode")]
        public string? Qrcode { get; set; }

        [JsonPropertyName("imagemQrcode")]
        public string? ImagemQrcode { get; set; }
    }
}