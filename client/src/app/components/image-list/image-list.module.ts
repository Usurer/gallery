import { NgModule } from '@angular/core';
import { ImageListComponent } from './image-list/image-list.component';
import { RouterModule } from '@angular/router';
import { BrowserModule } from '@angular/platform-browser';
import { CommonModule } from '@angular/common';
import { ImagePopupModule } from '../image-popup/image-popup.module';
import { MatIconModule } from '@angular/material/icon';
import { ScrollingModule } from '@angular/cdk/scrolling';
import { ImageListContainerComponent } from './image-list-container/image-list-container.component';
import { ClickNotificationService } from 'src/app/services/click-notification.service';
import { ImageListStore } from './image-list.store';

@NgModule({
    declarations: [ImageListComponent, ImageListContainerComponent],
    imports: [CommonModule, BrowserModule, RouterModule, ImagePopupModule, MatIconModule, ScrollingModule],
    exports: [ImageListContainerComponent],
    providers: [ClickNotificationService, ImageListStore],
})
export class ImageListModule {}
