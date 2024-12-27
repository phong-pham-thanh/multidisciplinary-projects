import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from './home/service/auth.service';
import { CookieService } from 'ngx-cookie-service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  showNavbar: boolean = true;

  sessionLoaded = false;

  constructor(
    private authService: AuthService,
    private router: Router,
    private cookieService: CookieService,
    // private spinnerService: SpinnerService
  ) {}

  ngOnInit(): void {
    this.router.events.subscribe(() => {
      this.showNavbar = this.router.url !== '/login';
    });
    this.initializeSession();

  }

  initializeSession() {

    const user = this.authService.getUserFromCookie();
  
    if (user && user.id) {
      this.authService.setSession(user.id).subscribe(
        (result) => {
          // this.cookieService.set('auth-token', JSON.stringify(result), { path: '/' });
          this.sessionLoaded = true;
        },
        (error) => {
          console.error("Session setup failed:", error);
          this.router.navigate(['/login']);
          this.sessionLoaded = true;
        }
      );
    } else {
      this.router.navigate(['/login']);
      this.sessionLoaded = true;
    }
  }
}
