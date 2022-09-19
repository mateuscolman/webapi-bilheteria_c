using System.Text.Json.Serialization;

namespace webapi_bilheteria_c.Domain.Models
{
    public class Loc
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("location")]
        public string? Location { get; set; }

        [JsonPropertyName("tipoCob")]
        public string? TipoCob { get; set; }

        [JsonPropertyName("criacao")]
        public DateTime Criacao { get; set; }
    }

    public class ChargeResponse
    {
        [JsonPropertyName("calendario")]
        public Calendario? Calendario { get; set; }

        [JsonPropertyName("txid")]
        public string? Txid { get; set; }

        [JsonPropertyName("revisao")]
        public int Revisao { get; set; }

        [JsonPropertyName("loc")]
        public Loc? Loc { get; set; }

        [JsonPropertyName("location")]
        public string? Location { get; set; }

        [JsonPropertyName("status")]
        public string? Status { get; set; }

        [JsonPropertyName("devedor")]
        public Devedor? Devedor { get; set; }

        [JsonPropertyName("valor")]
        public Valor? Valor { get; set; }

        [JsonPropertyName("chave")]
        public string? Chave { get; set; }

        [JsonPropertyName("solicitacaoPagador")]
        public string? SolicitacaoPagador { get; set; }
    }
}