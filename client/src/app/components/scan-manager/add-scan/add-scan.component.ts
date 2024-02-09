import { ChangeDetectionStrategy, Component, EventEmitter, Output } from '@angular/core';

@Component({
    selector: 'glr-add-scan',
    templateUrl: './add-scan.component.html',
    styleUrls: ['./add-scan.component.scss'],
    changeDetection: ChangeDetectionStrategy.Default,
})
export class AddScanComponent {
    @Output()
    addScan = new EventEmitter<string>();

    public submitDisabled = true;

    public pickerChange(value: string): void {
        this.submitDisabled = value.length === 0;
    }

    public onSubmit(value: string): void {
        if (value.length > 0) {
            this.addScan.emit(value);
        }
    }
}
