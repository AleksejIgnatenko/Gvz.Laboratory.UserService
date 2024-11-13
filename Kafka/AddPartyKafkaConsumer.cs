﻿using Confluent.Kafka;
using Gvz.Laboratory.UserService.Abstractions;
using Gvz.Laboratory.UserService.Dto;
using Serilog;
using System.Text.Json;

namespace Gvz.Laboratory.UserService.Kafka
{
    public class AddPartyKafkaConsumer : IHostedService
    {
        private readonly ConsumerConfig _config;
        private IConsumer<Ignore, string> _consumer;
        private CancellationTokenSource _cts;
        private readonly IPartyRepository _partyRepository;
        public AddPartyKafkaConsumer(ConsumerConfig config, IPartyRepository partyRepository = null)
        {
            _config = config;
            _partyRepository = partyRepository;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _consumer = new ConsumerBuilder<Ignore, string>(_config).Build();
            _consumer.Subscribe("add-party-topic");
            Task.Run(() => ConsumeMessages(cancellationToken));
            return Task.CompletedTask;
        }
        private async void ConsumeMessages(CancellationToken cancellationToken)
        {
            try
            {
                while (true)
                {
                    try
                    {
                        var cr = _consumer.Consume(cancellationToken);
                        var addPartyDto = JsonSerializer.Deserialize<PartyDto>(cr.Message.Value)
                            ?? throw new InvalidOperationException("Deserialization failed: Party is null.");
                        var addPartyDtoId = await _partyRepository.CreatePartyAsync(addPartyDto);
                    }
                    catch (ConsumeException e)
                    {
                        Log.Error($"Error occurred: {e.Error.Reason}");
                    }
                }
            }
            catch (OperationCanceledException)
            {
                _consumer.Close();
            }
            catch (InvalidOperationException ex)
            {
                Log.Error(ex.Message);
            }
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
