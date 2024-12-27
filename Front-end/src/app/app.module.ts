import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeControlComponent } from './home/home-control/home-control.component';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms'; // <-- Import FormsModule
import { MatSliderModule } from '@angular/material/slider';
import { ReactiveFormsModule } from '@angular/forms';
import { LoginComponent } from './home/login/login.component'; // Dùng cho Reactive Forms
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatTabsModule } from '@angular/material/tabs';
import { ConfigurationUserComponent } from './home/configuration-user/configuration-user.component';
import { NavbarComponent } from './home/navbar/navbar.component';
@NgModule({
  declarations: [
    AppComponent,
    HomeControlComponent,
    LoginComponent,
    ConfigurationUserComponent,
    NavbarComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    MatSliderModule,
    ReactiveFormsModule,
    FormsModule,
    BrowserAnimationsModule,
    MatTabsModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
