import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeControlComponent } from './home/home-control/home-control.component';
import { LoginComponent } from './home/login/login.component';
import { AuthGuard } from './home/service/auth.guard';
import { ConfigurationUserComponent } from './home/configuration-user/configuration-user.component';

const routes: Routes = [
  { path: '', redirectTo: 'app', pathMatch: 'full' },
  { path: 'app', component: HomeControlComponent, canActivate: [AuthGuard] },
  { path: 'config', component: ConfigurationUserComponent, canActivate: [AuthGuard] },
  { path: 'login', component: LoginComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
