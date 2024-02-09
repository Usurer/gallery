import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ImagePopupComponent } from './image-popup.component';
import { ImageModule } from '../image/image.module';
import { RouterModule } from '@angular/router';

@NgModule({
  declarations: [
    ImagePopupComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
    ImageModule
  ],
  exports: [
    ImagePopupComponent
  ]
})
export class ImagePopupModule {}
