import { Component, OnInit } from '@angular/core';
import { ControlService } from '../service/control.service';
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject } from 'rxjs';
import { FormControl } from '@angular/forms';
import { Users } from '../model/Users';
import { AuthService } from '../service/auth.service';
import _ from 'lodash';

@Component({
  selector: 'app-home-control',
  templateUrl: './home-control.component.html',
  styleUrls: ['./home-control.component.scss']
})
export class HomeControlComponent implements OnInit {

  constructor(private controlService: ControlService,
    private authService: AuthService,
  ) {
   }

  
  private hubConnection: signalR.HubConnection;
  private temperatureSource = new BehaviorSubject<number>(0);
  currentTemperature = this.temperatureSource.asObservable();
  sliderControl = new FormControl(50);
  acTemperature: number = 24;

  roomTemperature: number = 0;
  showAlert: boolean = false;

  lightColors: string[] = ['#FF0000', '#008000', '#0000FF', '#FFFF00', '#800080', '#FFA500', '#00FFFF', '#000000'];

  currentColorIndex: number = 0;
  currentUser: Users;


  speedSettings: number[] = [0, 2, 1.5, 1, 0.5]; 
  currentSpeed: number = 0; 

  formatLabel(value: number): string {
    if (value >= 1000) {
      return Math.round(value / 1000) + 'k';
    }
    return `${value}`;
  }


  ngOnInit(): void {
    var userCookies: Users | null = this.authService.getUserFromCookie();
    if(userCookies){
      this.authService.getCurrentUserInfo(userCookies.id).subscribe({
        next: (result) => {
          this.currentUser = _.cloneDeep(result);
        }
      })
    }
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
    this.changeFanSpeed()
  }

  decreaseSpeed() {
    if (this.currentSpeed > 0) {
      this.currentSpeed--;
    }
    this.changeFanSpeed()
  }

  changeFanSpeed(fanSpeed?: number){
    if(!fanSpeed){
      fanSpeed = this.currentSpeed;
    }
    else{
      this.currentSpeed = fanSpeed as number;
    }
    this.controlService.changeFanSpeed(this.currentSpeed).subscribe(
      response => {
      },
      error => {
        console.error('Lỗi khi thay đổi tốc độ quạt:', error);
      }
    );;
  }

  startListening(){
    this.controlService.openListeningConnection().subscribe();
    this.getDataTempurature();

    this.controlService.currentTemperature.subscribe(
      (temperature: number) => {
        this.roomTemperature = temperature;
        this.showAlert = !this.isNullOrEmpty(this.currentUser?.temperatureWarning) && this.currentUser.temperatureWarning as number < this.roomTemperature;
        if(this.showAlert && this.currentUser.autoRunFanWhenOverHeat && this.currentUser.autoRunFanWhenOverHeat as number > 0){
          this.changeFanSpeed(this.currentUser.autoRunFanWhenOverHeat);
        }

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

  closeListening(){
    this.controlService.stopListeningConnection().subscribe();
  }

  onSliderChange(data: any){
    console.log(data.target.value)
    this.controlService.changeTemperatureAirCondition(data.target.value as number).subscribe(
      response => {
        alert('Đã thay đổi nhiệt độ máy lạnh thành: '+ data.target.value );
      },
      error => {
        console.error('Lỗi khi thay đổi nhiệt độ máy lạnh:', error);
      }
    );
  }


  isNull(obj: any): boolean {
    if ((obj === 0) || (obj === false) || (obj === "")) {
        return false;
    }
    return (!obj || typeof obj === 'undefined' || obj === null);
  }

  isNullOrEmpty(obj: any): boolean {
    return this.isNull(obj) || obj === '';
  }
}
