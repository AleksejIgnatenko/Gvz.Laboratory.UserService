namespace Gvz.Laboratory.UserService.Kafka
{
    public class KafkaProducerConfig
    {
        public string BootstrapServers { get; set; } = string.Empty;
        public int MessageMaxBytes { get; set; }
    }
}
