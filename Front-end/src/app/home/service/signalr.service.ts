import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SignalrService {

  private hubConnection: signalR.HubConnection;
  private temperatureSource = new BehaviorSubject<number>(0);  // Khởi tạo một nguồn dữ liệu quan sát
  currentTemperature = this.temperatureSource.asObservable();  // Quan sát dữ liệu nhiệt độ

  constructor() {
    this.startConnection();
    this.addReceiveTemperatureListener();
  }

  // Bắt đầu kết nối SignalR
  private startConnection(): void {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:7108/temperatureHub')  // URL của SignalR Hub
      .build();

    this.hubConnection
      .start()
      .then(() => console.log('SignalR connection started'))
      .catch(err => console.log('Error while starting SignalR connection: ' + err));
  }

  // Lắng nghe sự kiện từ server và cập nhật nhiệt độ
  private addReceiveTemperatureListener(): void {
    this.hubConnection.on('ReceiveTemperature', (temperature: number) => {
      console.log('Nhiệt độ nhận từ server:', temperature);
      this.temperatureSource.next(temperature);  // Cập nhật giá trị nhiệt độ
    });
  }
}
