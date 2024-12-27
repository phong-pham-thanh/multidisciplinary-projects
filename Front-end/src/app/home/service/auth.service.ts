import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CookieService } from 'ngx-cookie-service';
import { Observable } from 'rxjs';
import { Users } from '../model/Users';
import { map } from 'rxjs/operators';
@Injectable({
  providedIn: 'root'
})
export class AuthService {
  constructor(
    private http: HttpClient,
    private cookieService: CookieService
  ) {}
  private apiUrl = 'https://localhost:7108/User/login'; 
  private apiSetMaxTemperature = 'https://localhost:7108/User/setWarningTempurate';
  private apiGetCurrentUserInfo = 'https://localhost:7108/User/getUserInfo';
  private apiTestSession = 'https://localhost:7108/User/test'; 
  private apiSetSession = `https://localhost:7108/User/setSession`;

  isAuthenticated(): boolean {
    return this.cookieService.check('auth-token');
  }

  login(username: string, password: string): Observable<Users> {
    const payload = { username, password };
    return this.http.post<Users>(this.apiUrl, payload, {withCredentials: true});
  }

  test(): Observable<Users> {
    return this.http.get<Users>(this.apiTestSession, {withCredentials: true});
  }

  logout(): void {
    this.cookieService.delete('auth-token');
  }

  getUserFromCookie(): Users | null {

    const userCookieValue = this.cookieService.get('auth-token');
    if (userCookieValue) {
      try {
        return JSON.parse(userCookieValue);
      } catch (error) {
        console.error('Failed to parse cookie value', error);
      }
    } else {
      console.log('No user cookie found');
      return null;
    }
    return null;
  }

  saveUserToCookie(user: Users): void {
    this.cookieService.set('auth-token', JSON.stringify(user), 1);
  }

  setSession(id: number): Observable<void> {
    return this.http.get<void>(`${this.apiSetSession}/${id}`, {withCredentials: true});
  }

  setWarningTemperature(saveObject: Users): Observable<boolean> {

    return this.http.post<boolean>(`${this.apiSetMaxTemperature}`,saveObject, {withCredentials: true});
  }

  getCurrentUserInfo(id: number): Observable<Users> {
    return this.http.get<Users>(`${this.apiGetCurrentUserInfo}/${id}`, {withCredentials: true});
  }
  
}
