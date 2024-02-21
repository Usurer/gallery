import { NgModule } from '@angular/core';
import { ScanManagerComponent } from './scan-manager/scan-manager.component';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { AddScanComponent } from './add-scan/add-scan.component';
import { ScanListComponent } from './scan-list/scan-list.component';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatIconModule } from '@angular/material/icon';

@NgModule({
    declarations: [ScanManagerComponent, AddScanComponent, ScanListComponent],
    exports: [ScanManagerComponent],
    imports: [CommonModule, HttpClientModule, MatCheckboxModule, MatIconModule],
})
export class ScanManagerModule {}
