import { ChangeDetectionStrategy, Component, EventEmitter, Input, Output, ViewEncapsulation } from '@angular/core';
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

    @Output()
    removeItem = new EventEmitter<number>();

    public onRemoveClick(id: number): void {
        this.removeItem.emit(id);
    }

    public trackById(idx: number, item: FolderScan) {
        return item.id;
    }
}
