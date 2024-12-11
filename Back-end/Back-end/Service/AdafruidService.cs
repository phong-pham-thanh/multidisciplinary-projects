using Back_end.ExtensionHub;
using Microsoft.AspNetCore.SignalR;
using MQTTnet;
using MQTTnet.Client;
using System;
using System.Text;
using System.Threading.Tasks;
using RJCP.IO.Ports;
namespace Back_end.Service
{
    public interface IAdafruidService
    {
        public Task ConnectToMqttServer();
        public Task StartListening(Func<string, Task> onMessageReceived);
        public Task StopListening();
        public Task SendDataToFeed(string data, string feedName);
        public Task ChangeLightColor(string data, string feedName);
        public bool IsClientConnected();
        public Task DisconnectFromMqttServer();
        public void TestController();
        public void CloseSerial();
        //public Task<string> GetDataFromFeed(string feedName);
    }

    public class AdafruidService : IAdafruidService
    {
        private IMqttClient _client;
        private MqttClientOptions _options;
        private readonly IHubContext<TemperatureHub> _hubContext;
        private SerialPortStream _serialPort;

        public AdafruidService(IHubContext<TemperatureHub> hubContext)
        {
            _hubContext = hubContext;


            var factory = new MqttFactory();
            _client = factory.CreateMqttClient();
            _options = new MqttClientOptionsBuilder()
                .WithClientId(Guid.NewGuid().ToString())
                .WithTcpServer("io.adafruit.com", 1883)
                .WithCredentials("ptpphamphong", "123123123")
                .WithCleanSession()
                .Build();
            _serialPort = new SerialPortStream();
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


        public async Task SendDataToFeed(string data, string feedName)
        {
            var message = new MqttApplicationMessageBuilder()
                .WithTopic(feedName)
                .WithPayload(data)
                .Build();

            await _client.PublishAsync(message);
        }

        public async Task ChangeLightColor(string data, string feedName)
        {
            var message = new MqttApplicationMessageBuilder()
                .WithTopic(feedName)
                .WithPayload(data)
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


        public void CloseSerial()
        {
            try
            {
                if (_serialPort == null || !_serialPort.IsOpen)
                {
                    return;
                }

                _serialPort.Close();
                Console.WriteLine("Đã ngắt kết nối.");

                return;
            }
            catch (Exception ex)
            {
                return;
            }
        }




        public void TestController()
        {
            string portName = GetPort();

            if (portName != "None")
            {
                try
                {

                    if (_serialPort != null && _serialPort.IsOpen)
                    {
                        return;
                    }
                    _serialPort = new SerialPortStream(portName, 115200);

                    _serialPort.DataReceived += new EventHandler<SerialDataReceivedEventArgs>(SerialPort_DataReceived);

                    _serialPort.Open();
                    Console.WriteLine("Cổng nối tiếp đã được mở: " + portName);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Lỗi: " + ex.Message);
                }
            }
            else
            {
                Console.WriteLine("Không tìm thấy cổng nối tiếp phù hợp.");
            }
        }



        public void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPortStream serialPort = (SerialPortStream)sender;

            // Đọc dữ liệu từ cổng nối tiếp
            byte[] buffer = new byte[serialPort.BytesToRead];
            int bytesRead = serialPort.Read(buffer, 0, buffer.Length);

            // Chuyển dữ liệu thành chuỗi và in ra console
            string receivedData = System.Text.Encoding.ASCII.GetString(buffer, 0, bytesRead);
            Console.WriteLine("Đã nhận dữ liệu: " + receivedData);
        }

        public string GetPort()
        {
            string commPort = "None";
            string[] ports = SerialPortStream.GetPortNames();

            foreach (string port in ports)
            {
                if (port.Contains("COM6"))
                {
                    commPort = port;
                    break;
                }
            }

            return commPort;
        }









        // Phương thức để lấy dữ liệu từ feed trên Adafruit
        //public async Task<string> GetDataFromFeed(string feedName)
        //{
        //    feedName = "ptpphamphong/feeds/feed-slide-bar";
        //    var tcs = new TaskCompletionSource<string>();

        //    _client.ApplicationMessageReceivedAsync += e =>
        //    {
        //        string payload = Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment);
        //        Console.WriteLine($"Nhận dữ liệu từ feed: {payload}");

        //        tcs.SetResult(payload);
        //        return Task.CompletedTask;
        //    };

        //    await _client.SubscribeAsync(feedName);

        //    string result = await tcs.Task;

        //    return result;
        //}

    }
}
