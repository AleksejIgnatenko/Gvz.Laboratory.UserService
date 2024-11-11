using Gvz.Laboratory.UserService.Dto;

namespace Gvz.Laboratory.UserService.Abstractions
{
    public interface IUserKafkaProducer
    {
        Task SendUserToKafkaAsync(UserDto user, string topic);
    }
}