﻿using Back_end.ExtensionHub;
using Microsoft.AspNetCore.SignalR;
using MQTTnet;
using MQTTnet.Client;
using System;
using System.Text;
using System.Threading.Tasks;
using RJCP.IO.Ports;
using Back_end.Repository;
using Microsoft.Extensions.DependencyInjection;
namespace Back_end.Service
{
    public interface IAdafruidService
    {
        public Task ConnectToMqttServer();
        public Task StartListening(Func<string, Task> onMessageReceived);
        public Task StopListening();
        public Task SendDataToFeed(string data, string feedName, bool needSendBackToSerial = false);
        public Task ChangeLightColor(string data, string feedName);
        public bool IsClientConnected();
        public Task DisconnectFromMqttServer();
        public void StartListeningSerialCom8(Func<string, Task> onMessageReceived);
        public void CloseSerial();
        //public Task<string> GetDataFromFeed(string feedName);
    }

    public class AdafruidService : IAdafruidService
    {
        private IMqttClient _client;
        private MqttClientOptions _options;
        private readonly IHubContext<TemperatureHub> _hubContext;
        private ITemperatureRecordRepository _temperatureRepository;
        private readonly IServiceProvider _serviceProvider;
        private readonly ISerialPortManager _serialPortManagement;

        public AdafruidService(IHubContext<TemperatureHub> hubContext,
            ITemperatureRecordRepository temperatureRepository,
            ISerialPortManager serialPortManagement,
            IServiceProvider serviceProvider
            )
        {
            _hubContext = hubContext;
            _temperatureRepository = temperatureRepository;
            _serviceProvider = serviceProvider;
            _serialPortManagement = serialPortManagement;


            var factory = new MqttFactory();
            _client = factory.CreateMqttClient();
            _options = new MqttClientOptionsBuilder()
                .WithClientId(Guid.NewGuid().ToString())
                .WithTcpServer("io.adafruit.com", 1883)
                .WithCredentials("ptpphamphong", "123123123")
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
                string payload = Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment);
                Console.WriteLine("Nhận dữ liệu từ Adafruit: " + payload);

                await _hubContext.Clients.All.SendAsync("ReceiveTemperature", payload);

                await onMessageReceived(payload);
            };

            await _client.SubscribeAsync("ptpphamphong/feeds/temperature-enviroment");
            Console.WriteLine("Đã subscribe tới feed: temperature-enviroment");
        }

        public async Task StopListening()
        {
            await _client.UnsubscribeAsync("ptpphamphong/feeds/kenh-du-lieu-moi");
            Console.WriteLine("Đã hủy đăng ký khỏi feed: kenh-du-lieu-moi");
        }


        public async Task SendDataToFeed(string data, string feedName, bool needSendBackToSerial = false)
        {
            var message = new MqttApplicationMessageBuilder()
                .WithTopic(feedName)
                .WithPayload(data)
                .Build();
            if(needSendBackToSerial)
            {
                this.SendDataToCOM7(data);
            }

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
                if (_serialPortManagement.GetSerialPort() == null || !_serialPortManagement.GetSerialPort().IsOpen)
                {
                    return;
                }

                _serialPortManagement.GetSerialPort().Close();
                Console.WriteLine("Đã ngắt kết nối.");

                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return;
            }
        }


        public void StartListeningSerialCom8(Func<string, Task> onMessageReceived)
        {
            string portName = GetPort();

            if (portName != "None")
            {
                try
                {

                    if (_serialPortManagement.GetSerialPort() != null && _serialPortManagement.GetSerialPort().IsOpen)
                    {
                        return;
                    }

                    _serialPortManagement.SetSerialPort(new SerialPortStream(portName, 115200));
                    _serialPortManagement.SetIrgnore(true);
                    _serialPortManagement.GetSerialPort().DataReceived += new EventHandler<SerialDataReceivedEventArgs>(SerialPort_DataReceived);

                    _serialPortManagement.GetSerialPort().Open();
                    _serialPortManagement.GetSerialPort().DiscardInBuffer();

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



        public async void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //if (_serialPortManagement.GetIrgnore())
            //{
            //    _serialPortManagement.SetIrgnore(false);
            //    return;
            //}
            SerialPortStream serialPort = (SerialPortStream)sender;
            byte[] buffer = new byte[serialPort.BytesToRead];
            int bytesRead = serialPort.Read(buffer, 0, buffer.Length);

            string receivedData = System.Text.Encoding.ASCII.GetString(buffer, 0, bytesRead);


            await _hubContext.Clients.All.SendAsync("ReceiveTemperature", receivedData);

            string feedName = "ptpphamphong/feeds/temperature-enviroment";
            await SendDataToFeed(receivedData, feedName);

             _temperatureRepository.AddNewRecord(_serialPortManagement.GetCurrentUserId(), Int32.Parse(receivedData));
            Console.WriteLine("hahahaha: " + receivedData);
        }

        public void SendDataToCOM7(string data)
        {
            string portName = "COM7";
            int baudRate = 115200;

                try
                {
                    _serialPortManagement.GetSerialPort().Write(data);
                    Console.WriteLine($"Đã gửi dữ liệu: {data}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Lỗi khi gửi dữ liệu: " + ex.Message);
                }
                finally
                {
                }
            //}
        }

        public string GetPort()
        {
            string commPort = "None";
            string[] ports = SerialPortStream.GetPortNames();

            foreach (string port in ports)
            {
                if (port.Contains("COM8"))
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
        //    feedName = "ptpphamphong/feeds/temperature-enviroment";
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
