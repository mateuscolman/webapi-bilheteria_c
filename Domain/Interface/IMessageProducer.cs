using webapi_bilheteria_c.Domain.Models;

namespace webapi_bilheteria_c.Domain.Interface
{
    public interface IMessageProducer
    {
        void SendMessage<T> (T message, string queue );
    }
}