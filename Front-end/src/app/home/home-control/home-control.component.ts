import { Component, OnInit } from '@angular/core';
import { Product } from '../model/Product';
import { ControlService } from '../service/control.service';
import * as signalR from '@microsoft/signalr';
// import { SignalrService } from '../service/signalr.service';
import { BehaviorSubject } from 'rxjs';


@Component({
  selector: 'app-home-control',
  templateUrl: './home-control.component.html',
  styleUrls: ['./home-control.component.scss']
})
export class HomeControlComponent implements OnInit {

  constructor(private controlService: ControlService,
    // private signalrService: SignalrService
  ) {
   }

  
  private hubConnection: signalR.HubConnection;
  private temperatureSource = new BehaviorSubject<number>(0);
  currentTemperature = this.temperatureSource.asObservable();

  acTemperature: number = 24;

  roomTemperature: number = 0;

  // lightColors: string[] = ['red', 'green', 'blue', 'yellow', 'purple', 'orange', 'cyan'];
  lightColors: string[] = ['#FF0000', '#008000', '#0000FF', '#FFFF00', '#800080', '#FFA500', '#00FFFF'];

  currentColorIndex: number = 0;


  speedSettings: number[] = [0, 2, 1.5, 1, 0.5]; 
  currentSpeed: number = 0; 



  ngOnInit(): void {
  }


  changeLightColor() {
    this.currentColorIndex = (this.currentColorIndex + 1) % this.lightColors.length;
    this.controlService.changeLightColor(this.lightColors[this.currentColorIndex]).subscribe(
      response => {
        console.log('Đã thay đổi màu thành:', this.lightColors[this.currentColorIndex]);
      },
      error => {
        console.error('Lỗi khi thay đổi màu:', error);
      }
    );
  }

  increaseSpeed() {
    if (this.currentSpeed < this.speedSettings.length - 1) {
      this.currentSpeed++;
    }
  }

  decreaseSpeed() {
    if (this.currentSpeed > 0) {
      this.currentSpeed--;
    }
  }

  startListening(){
    this.controlService.openListeningConnection().subscribe();
    this.getDataTempurature();

    this.controlService.currentTemperature.subscribe(
      (temperature: number) => {
        this.roomTemperature = temperature;
      },
      (error) => {
        console.error('Lỗi khi nhận dữ liệu từ temperatureSource:', error);
      }
    );

  }
  
  getDataTempurature(){
    this.controlService.getFeedData("feed-slide-bar").subscribe(
      data => {
        this.roomTemperature = data.value;
      },
      error => {
        console.error('Lỗi khi lấy dữ liệu từ Adafruit:', error);
      }
    )
  }
}
