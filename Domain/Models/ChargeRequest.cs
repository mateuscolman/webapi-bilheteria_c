using Newtonsoft.Json;

namespace webapi_bilheteria_c.Domain.Models
{       
    public class Calendario
    {
        [JsonProperty("expiracao")]
        public int Expiracao { get; set; } = 3600;
    }

    public class Devedor
    {
        [JsonProperty("cpf")]
        public string? Cpf { get; set; }
        [JsonProperty("nome")]
        public string? Nome { get; set; }
    }

    public class Valor
    {
        [JsonProperty("original")]
        public string? Original { get; set; }
    }

    public class ChargeRequest
    {
        [JsonProperty("calendario")]
        public Calendario? Calendario { get; set; }

        [JsonProperty("devedor")]
        public Devedor? Devedor { get; set; }

        [JsonProperty("valor")]
        public Valor? Valor { get; set; }

        [JsonProperty("chave")]
        public string? Chave { get; set; }

        [JsonProperty("solicitacaoPagador")]
        public string? SolicitacaoPagador { get; set; }
    }
}