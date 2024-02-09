import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { ImageListModule } from './components/image-list/image-list.module';
import { PlaceholderComponent } from './components/placeholder/placeholder.component';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { ImageModule } from './components/image/image.module';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatIconModule } from '@angular/material/icon';
import { FolderExplorerModule } from './components/folder-explorer/folder-explorer.module';
import { ENVIRONMENT_CONFIG } from '../environments/environment-config';
import { environment } from '../environments/environment';
import { ScanManagerModule } from './components/scan-manager/scan-manager.module';

@NgModule({
    declarations: [PlaceholderComponent, AppComponent],
    imports: [
        CommonModule,
        BrowserModule,
        HttpClientModule,
        ImageModule,
        ImageListModule,
        FolderExplorerModule,
        ScanManagerModule,
        AppRoutingModule,
        BrowserAnimationsModule,
        MatIconModule,
    ],
    providers: [{ provide: ENVIRONMENT_CONFIG, useValue: environment }],
    bootstrap: [AppComponent],
})
export class AppModule {
    // Diagnostic only: inspect router configuration
    constructor(router: Router) {
        // Use a custom replacer to display function names in the route configs
        const replacer = (key: any, value: any) => (typeof value === 'function' ? value.name : value);

        console.log('Routes: ', JSON.stringify(router.config, replacer, 2));
    }
}
