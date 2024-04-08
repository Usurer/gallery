import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { BrowserModule } from '@angular/platform-browser';
import { CommonModule } from '@angular/common';
import { ImageComponent } from './image.component';
import { PipesModule } from 'src/app/pipes/pipes.module';

@NgModule({
    declarations: [ImageComponent],
    imports: [CommonModule, BrowserModule, RouterModule, PipesModule],
    exports: [ImageComponent],
})
export class ImageModule {}
