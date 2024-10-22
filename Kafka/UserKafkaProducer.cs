using Confluent.Kafka;
using Gvz.Laboratory.UserService.Dto;
using System.Text.Json;
using Gvz.Laboratory.UserService.Abstractions;

namespace Gvz.Laboratory.UserService.Kafka
{
    public class UserKafkaProducer : IUserKafkaProducer
    {
        private readonly IProducer<Null, string> _producer;

        public UserKafkaProducer(IProducer<Null, string> producer)
        {
            _producer = producer;
        }

        public async Task SendUserToKafka(UserDto user, string topic)
        {
            var serializedUser = JsonSerializer.Serialize(user);
            await _producer.ProduceAsync(topic, new Message<Null, string> { Value = serializedUser });
        }
    }
}