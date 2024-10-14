using Back_end.ExtensionHub;
using Microsoft.AspNetCore.SignalR;
using MQTTnet;
using MQTTnet.Client;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Back_end.Service
{
    public interface IAdafruidService
    {
        public Task ConnectToMqttServer();
        public Task StartListening(Func<string, Task> onMessageReceived);
        public Task StopListening();
        public Task SendRandomDataToServer(int number);
        public bool IsClientConnected();
        public Task DisconnectFromMqttServer();
    }

    public class AdafruidService : IAdafruidService
    {
        private IMqttClient _client;
        private MqttClientOptions _options;
        private readonly IHubContext<TemperatureHub> _hubContext;

        public AdafruidService(IHubContext<TemperatureHub> hubContext)
        {
            _hubContext = hubContext;


            var factory = new MqttFactory();
            _client = factory.CreateMqttClient();
            _options = new MqttClientOptionsBuilder()
                .WithClientId(Guid.NewGuid().ToString())
                .WithTcpServer("io.adafruit.com", 1883)
                .WithCleanSession()
                .Build();
        }

        public bool IsClientConnected()
        {
            return _client.IsConnected;
        }

        public async Task ConnectToMqttServer()
        {
            if (!_client.IsConnected)
            {
                await _client.ConnectAsync(_options);
                Console.WriteLine("Kết nối thành công...");
            }
        }

        public async Task StartListening(Func<string, Task> onMessageReceived)
        {
            _client.ApplicationMessageReceivedAsync += async e =>
            {
                string payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                Console.WriteLine("Nhận dữ liệu từ Adafruit: " + payload);

                // Gửi dữ liệu đến tất cả các client qua SignalR
                await _hubContext.Clients.All.SendAsync("ReceiveTemperature", payload);

                // Callback để xử lý thêm logic nếu cần
                await onMessageReceived(payload);
            };

            await _client.SubscribeAsync("ptpphamphong/feeds/feed-slide-bar");
            Console.WriteLine("Đã subscribe tới feed: feed-slide-bar");
        }

        public async Task StopListening()
        {
            await _client.UnsubscribeAsync("ptpphamphong/feeds/kenh-du-lieu-moi");
            Console.WriteLine("Đã hủy đăng ký khỏi feed: kenh-du-lieu-moi");
        }


        public async Task SendRandomDataToServer(int number)
        {
            var message = new MqttApplicationMessageBuilder()
                .WithTopic("ptpphamphong/feeds/feed-send-data")
                .WithPayload(number.ToString())
                .Build();

            await _client.PublishAsync(message);
        }

        public async Task DisconnectFromMqttServer()
        {
            if (_client.IsConnected)
            {
                await _client.DisconnectAsync();
                Console.WriteLine("Đã ngắt kết nối.");
            }
        }
    }
}
