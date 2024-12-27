import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../service/auth.service';
import { Users } from '../model/Users';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  username: string = '';
  password: string = '';
  errorMessage: string = '';

  constructor(private authService: AuthService, private router: Router) { }

  onLogin(): void {
    this.authService.login(this.username, this.password).subscribe({
      next: (user: Users) => {
        if (user) {
          this.authService.saveUserToCookie(user);
          this.router.navigate(['/app']);
        } else {
          // Hiển thị thông báo nếu login thất bại
          this.errorMessage = 'Tên đăng nhập hoặc mật khẩu sai';
        }
      },
      error: (err) => {
        console.error(err);
        this.errorMessage = 'An error occurred during login.';
      },
    }); 
  }
}

