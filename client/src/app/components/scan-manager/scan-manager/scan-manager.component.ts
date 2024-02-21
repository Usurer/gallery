import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { ScanManagerStore } from '../scan-manager.store';

@Component({
    selector: 'glr-scan-manager',
    templateUrl: './scan-manager.component.html',
    styleUrls: ['./scan-manager.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
    providers: [ScanManagerStore],
})
export class ScanManagerComponent implements OnInit {
    public scans$ = this.store.scans$;

    constructor(private store: ScanManagerStore) {
        // this.store.addScan.subscribe();
    }
    ngOnInit(): void {
        this.store.getScans();
    }

    public addScan(value: string): void {
        const data = value.split(';').filter((x) => x.length);
        for (const d of data) {
            this.store.addScan(d);
        }
    }

    public onRemoveItem(item: number): void {
        this.store.deleteScan(item);
    }
}
