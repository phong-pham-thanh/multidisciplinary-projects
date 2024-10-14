using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Back_end.ExtensionHub
{
    public class TemperatureHub : Hub
    {
        // Gửi nhiệt độ tới tất cả các client
        public async Task SendTemperature(int temperature)
        {
            await Clients.All.SendAsync("ReceiveTemperature", temperature);
        }
    }
}
