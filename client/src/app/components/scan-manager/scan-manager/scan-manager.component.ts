import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { ScanManagerStore } from '../scan-manager.store';
import { map, withLatestFrom } from 'rxjs';

@Component({
    selector: 'glr-scan-manager',
    templateUrl: './scan-manager.component.html',
    styleUrls: ['./scan-manager.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ScanManagerComponent implements OnInit {
    public scans$ = this.store.scans$;
    public prefix$ = this.store.prefix$;

    public scansShortened$ = this.scans$.pipe(
        withLatestFrom(this.prefix$),
        map(([scans, prefix]) => {
            return scans.map((scan) => {
                return {
                    ...scan,
                    path: scan.path.slice(prefix.length),
                };
            });
        })
    );

    constructor(private store: ScanManagerStore) {}

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
