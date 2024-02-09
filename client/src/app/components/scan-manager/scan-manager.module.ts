import { NgModule } from '@angular/core';
import { ScanManagerComponent } from './scan-manager/scan-manager.component';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { AddScanComponent } from './add-scan/add-scan.component';

@NgModule({
    declarations: [ScanManagerComponent, AddScanComponent],
    imports: [CommonModule, HttpClientModule],
})
export class ScanManagerModule {}
