import { ChangeDetectionStrategy, Component, Input, ViewEncapsulation } from '@angular/core';
import { FolderScan } from '../scan-manager.store';

@Component({
    selector: 'glr-scan-list',
    templateUrl: './scan-list.component.html',
    styleUrls: ['./scan-list.component.scss'],
    encapsulation: ViewEncapsulation.Emulated,
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ScanListComponent {
    @Input()
    public items: FolderScan[] | null = [];
}
