using Back_end.Service;
using Microsoft.AspNetCore.Mvc;
using Back_end.Service;
using Back_end.ExtensionHub;
using Microsoft.AspNetCore.SignalR;

namespace Back_end.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdafruidController : ControllerBase
    {
        private IAdafruidService _adafruidService;
        private readonly IHubContext<TemperatureHub> _hubContext;  // Inject SignalR Hub context

        public AdafruidController(
            IAdafruidService adafruidService,
            IHubContext<TemperatureHub> hubContext)
        {
            _adafruidService = adafruidService;
            _hubContext = hubContext;  // Khởi tạo SignalR Hub context
        }

        [HttpGet]
        [Route("open-listening-connection")]
        public async Task<IActionResult> OpenListeningConnection()
        {
            try
            {
                // Kết nối tới MQTT server
                await _adafruidService.ConnectToMqttServer();

                // Bắt đầu lắng nghe và gửi dữ liệu về client qua SignalR
                await _adafruidService.StartListening(async (string message) =>
                {
                    // Khi nhận được message từ Adafruit, gửi message đó tới tất cả client
                    await _hubContext.Clients.All.SendAsync("ReceiveTemperature", message);
                });

                return Ok(new { message = "MQTT connection established and listening started." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Error occurred: {ex.Message}" });
            }
        }

        [HttpGet]
        [Route("close-listening-connection")]
        public async Task<IActionResult> CloseListeningConnection()
        {
            await _adafruidService.ConnectToMqttServer();
            await _adafruidService.StopListening();

            return Ok("Đã ngắt nhận dữ liệu thành công .");
        }



        [HttpPost]
        [Route("send-data")]
        public async Task<IActionResult> SendData([FromBody] int number)
        {
            if (!_adafruidService.IsClientConnected())
            {
                await _adafruidService.ConnectToMqttServer();
            }

            await _adafruidService.SendRandomDataToServer(number);

            return Ok($"Đã gửi dữ liệu: {number}");
        }

        [HttpGet]
        [Route("close-connection")]
        public async Task<IActionResult> CloseConnection()
        {
            // Ngắt kết nối khỏi MQTT broker
            await _adafruidService.DisconnectFromMqttServer();

            return Ok("Đã ngắt kết nối khỏi MQTT server.");
        }


    }
}
