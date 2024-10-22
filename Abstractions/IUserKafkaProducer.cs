using Gvz.Laboratory.UserService.Dto;

namespace Gvz.Laboratory.UserService.Abstractions
{
    public interface IUserKafkaProducer
    {
        Task SendUserToKafka(UserDto user, string topic);
    }
}