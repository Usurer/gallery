import { ChangeDetectionStrategy, Component } from '@angular/core';
import { ScanManagerStore } from '../scan-manager.store';

@Component({
    selector: 'glr-scan-manager',
    templateUrl: './scan-manager.component.html',
    styleUrls: ['./scan-manager.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
    providers: [ScanManagerStore],
})
export class ScanManagerComponent {
    public scans$ = this.store.scans$;

    constructor(private store: ScanManagerStore) {}

    public addScan(value: string): void {
        const data = value.split(';').filter(x => x.length);
        for(const d of data) {
            this.store.addScan(d)
        }
    }
}
