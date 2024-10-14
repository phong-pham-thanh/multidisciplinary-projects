import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Product } from './model/Product';
@Injectable({
  providedIn: 'root'
})
export class ProductService {


  private apiUrl = 'https://localhost:7108/Adafruid'; // URL API từ .NET

  constructor(private http: HttpClient) { }

  openListeningConnection(): Observable<any> {
    return this.http.get(`${this.apiUrl}/open-listening-connection`);
  }
}
