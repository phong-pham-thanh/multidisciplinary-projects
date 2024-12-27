import { Component } from '@angular/core';
import { AuthService } from '../service/auth.service';
import { Users } from '../model/Users';

@Component({
  selector: 'app-configuration-user',
  templateUrl: './configuration-user.component.html',
  styleUrls: ['./configuration-user.component.scss']
})
export class ConfigurationUserComponent {

  temperature: number = 0;
  warningWhenOverHeat: boolean = false;
  autoRunFanWhenOverHeat: number = 0;

  constructor(private authService: AuthService) {

  }

  ngOnInit() {

    var userCookies: Users | null = this.authService.getUserFromCookie();
    if (userCookies) {
      this.authService.getCurrentUserInfo(userCookies.id).subscribe({
        next: (result) => {
          this.temperature = result.temperatureWarning ? result.temperatureWarning : 0;
          this.warningWhenOverHeat = result.warningWhenOverHeat ? result.warningWhenOverHeat : false;
          this.autoRunFanWhenOverHeat = result.autoRunFanWhenOverHeat ? result.autoRunFanWhenOverHeat : 0;
        }
      })
    }
  }


  updateTemperature(): void {
    if (this.temperature <= 0) {
      this.warningWhenOverHeat = false;
      this.autoRunFanWhenOverHeat = 0;
    }
    const saveObject: Users = {
      id: 0,
      temperatureWarning: this.temperature,
      autoRunFanWhenOverHeat: this.autoRunFanWhenOverHeat,
      warningWhenOverHeat: this.warningWhenOverHeat,
    }
    this.authService.setWarningTemperature(saveObject).subscribe({
      next: () => {
        alert('Cập nhật thành công!');
        window.location.reload();
      },
      error: (err) => {
        alert('Đã xảy ra lỗi ');
      }
    });
  }

  testSession() {

    this.authService.test().subscribe({
      next: () => {

      },
      error: (err) => {
        console.error(err);
      },
    });
  }
}
