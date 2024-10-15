import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeControlComponent } from './home/home-control/home-control.component';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms'; // <-- Import FormsModule
import { MatSliderModule } from '@angular/material/slider';
import { ReactiveFormsModule } from '@angular/forms'; // Dùng cho Reactive Forms

@NgModule({
  declarations: [
    AppComponent,
    HomeControlComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    MatSliderModule,
    ReactiveFormsModule,
    FormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
