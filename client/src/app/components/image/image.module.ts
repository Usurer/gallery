import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { BrowserModule } from '@angular/platform-browser';
import { CommonModule } from '@angular/common';
import { ImageComponent } from './image.component';

@NgModule({
  declarations: [
    ImageComponent,
  ],
  imports: [
    CommonModule,
    BrowserModule,
    RouterModule,
  ],
  exports: [
    ImageComponent
  ]
})
export class ImageModule {}
