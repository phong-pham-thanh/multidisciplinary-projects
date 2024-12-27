import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import * as signalR from '@microsoft/signalr';
import { catchError } from 'rxjs/operators';


@Injectable({
  providedIn: 'root'
})
export class ControlService {

  
  private apiKey = '123123123';
  private username = 'ptpphamphong'; 
  private baseUrl = `https://io.adafruit.com/api/v2/${this.username}/feeds`; 

  private apiUrlStartConnection = 'https://localhost:7108/Adafruid/connect-serial'; 
  private apiUrlStopConnection = 'https://localhost:7108/Adafruid/close-serial'; 
  private apiChangeLightColor = 'https://localhost:7108/Adafruid/change-light-color'
  private apiChangeTemperatureAirCondition = 'https://localhost:7108/Adafruid/change-temperature-air-condition'
  private apiChangeFanSpeed = 'https://localhost:7108/Adafruid/change-fan-speed'

  // private feedNameLight = 'ptpphamphong/feeds/feed-color-picker';

  private hubConnection: signalR.HubConnection;
  private temperatureSource = new BehaviorSubject<number>(0); 
  currentTemperature = this.temperatureSource.asObservable(); 

  constructor(private http: HttpClient) {
    this.startConnection();
    this.addReceiveTemperatureListener();
   }

  openListeningConnection(): Observable<any> {
    return this.http.get(`${this.apiUrlStartConnection}`);
  }

  stopListeningConnection(): Observable<any> {
    return this.http.get(`${this.apiUrlStopConnection}`);
  }

  getFeedData(feedName: string): Observable<any> {
    const url = `${this.baseUrl}/${feedName}/data/last`;
    const headers = new HttpHeaders({
      'X-AIO-Key': this.apiKey
    });

    return this.http.get(url, { headers });
  }


  changeLightColor(color: string): Observable<any> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    const body = { color };  // Tạo object JSON với thuộc tính 'color'
  
    return this.http.post(`${this.apiChangeLightColor}`, body, { headers });
  }

  changeTemperatureAirCondition(temperature: number): Observable<any> {
    return this.http.get(`${this.apiChangeTemperatureAirCondition}/${temperature}`);
  }

  
  changeFanSpeed(speed: number): Observable<any> {
    return this.http.get(`${this.apiChangeFanSpeed}/${speed}`);
  }







  private startConnection(): void {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:7108/temperatureHub') 
      .build();

    this.hubConnection
      .start()
      .then(() => console.log('SignalR connection started'))
      .catch(err => console.log('Error while starting SignalR connection: ' + err));
  }

  private addReceiveTemperatureListener(): void {
    this.hubConnection.on('ReceiveTemperature', (temperature: number) => {
      // console.log('Nhiệt độ nhận từ server:', temperature);
      this.temperatureSource.next(temperature); 
    });
  }
}
