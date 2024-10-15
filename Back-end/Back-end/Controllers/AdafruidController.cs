using Back_end.Service;
using Microsoft.AspNetCore.Mvc;
using Back_end.Service;
using Back_end.ExtensionHub;
using Microsoft.AspNetCore.SignalR;

namespace Back_end.Controllers
{

    public class ColorRequest
    {
        public string Color { get; set; }
    }

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
                await _adafruidService.ConnectToMqttServer();

                await _adafruidService.StartListening(async (string message) =>
                {
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



        //[HttpPost]
        //[Route("send-data")]
        //public async Task<IActionResult> SendData([FromBody] string data)
        //{
        //    if (!_adafruidService.IsClientConnected())
        //    {
        //        await _adafruidService.ConnectToMqttServer();
        //    }
        //    string feedName = "ptpphamphong/feeds/feed-color-picker";
        //    await _adafruidService.SendRandomDataToServer(data, feedName);

        //    return Ok($"Đã gửi dữ liệu: {data}");
        //}

        [HttpPost]
        [Route("change-light-color")]
        public async Task<IActionResult> ChangeLightColor([FromBody] ColorRequest colorRequest)
        {
            if (!_adafruidService.IsClientConnected())
            {
                await _adafruidService.ConnectToMqttServer();
            }
            string feedName = "ptpphamphong/feeds/feed-color-picker";
            await _adafruidService.SendDataToFeed(colorRequest.Color, feedName);
            return Ok(new { message = $"Done Change to: {colorRequest.Color}" });
        }

        [HttpGet]
        [Route("change-temperature-air-condition/{temperature}")]
        public async Task<IActionResult> ChangeTemperatureAirCondition(int temperature)
        {
            if (!_adafruidService.IsClientConnected())
            {
                await _adafruidService.ConnectToMqttServer();
            }
            string feedName = "ptpphamphong/feeds/feed-air-condition";
            await _adafruidService.SendDataToFeed(temperature.ToString(), feedName);
            return Ok(new { message = $"Done temperature: {temperature}" });
        }

        [HttpGet]
        [Route("change-fan-speed/{speed}")]
        public async Task<IActionResult> ChangeFanSpeed(int speed)
        {
            if (!_adafruidService.IsClientConnected())
            {
                await _adafruidService.ConnectToMqttServer();
            }
            string feedName = "ptpphamphong/feeds/feed-fan-speed";
            await _adafruidService.SendDataToFeed(speed.ToString(), feedName);
            return Ok(new { message = $"Done temperature: {speed}" });
        }

        [HttpGet]
        [Route("close-connection")]
        public async Task<IActionResult> CloseConnection()
        {
            // Ngắt kết nối khỏi MQTT broker
            await _adafruidService.DisconnectFromMqttServer();

            return Ok("Đã ngắt kết nối khỏi MQTT server.");
        }

        //[HttpGet("get-feed-data")]
        //public async Task<IActionResult> GetFeedData()
        //{
        //    try
        //    {
        //        // Kết nối tới Adafruit MQTT
        //        await _adafruidService.ConnectToMqttServer();

        //        // Lấy dữ liệu từ feed
        //        var feedData = await _adafruidService.GetDataFromFeed("123");

        //        // Trả về dữ liệu nhận được từ feed
        //        return Ok(new { feedData });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new { error = ex.Message });
        //    }
        //}


    }
}
