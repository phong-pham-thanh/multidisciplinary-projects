import { Component, OnInit } from '@angular/core';
import { Product } from '../model/Product';
import { ProductService } from '../product.service';
import * as signalR from '@microsoft/signalr';
import { SignalrService } from '../service/signalr.service';


@Component({
  selector: 'app-home-control',
  templateUrl: './home-control.component.html',
  styleUrls: ['./home-control.component.scss']
})
export class HomeControlComponent implements OnInit {

  constructor(private fanService: ProductService,
    private signalrService: SignalrService
  ) { }

  
  private hubConnection: signalR.HubConnection;

  acTemperature: number = 24;

  roomTemperature: number = 28;

  lightColors: string[] = ['red', 'green', 'blue', 'yellow', 'purple', 'orange', 'cyan'];
  currentColorIndex: number = 0;


  speedSettings: number[] = [0, 2, 1.5, 1, 0.5]; // Tốc độ quạt: 0 -> dừng, 4 -> nhanh nhất
  currentSpeed: number = 0; // Mức tốc độ hiện tại (bắt đầu từ 0, tức là dừng)



  ngOnInit(): void {
    this.signalrService.currentTemperature.subscribe((temperature: number) => {
      console.log(temperature)
      this.roomTemperature = temperature;  // Cập nhật nhiệt độ trên giao diện
    });
  }


  changeLightColor() {
    this.currentColorIndex = (this.currentColorIndex + 1) % this.lightColors.length;
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
}
