import { Component, EventEmitter, Input, OnChanges, Output } from '@angular/core';
import { FolderHierarchyStore } from './folder-hierarchy.store';
import { map } from 'rxjs';

@Component({
    selector: 'glr-folder-breadcrumbs',
    templateUrl: './folder-breadcrumbs.component.html',
    styleUrls: ['./folder-breadcrumbs.component.scss'],
    providers: [FolderHierarchyStore],
})
export class FolderBreadcrumbsComponent implements OnChanges {
    @Input()
    rootId!: number;

    @Output()
    folderSelected = new EventEmitter<number>();

    public items$ = this.store.select((x) => x.folders).pipe(map((x) => x.reverse()));

    constructor(private store: FolderHierarchyStore) {}

    ngOnChanges(): void {
        this.store.fetchHierarcy(this.rootId);
    }

    onSelected(id: number): void {
        this.folderSelected.emit(id);
    }
}
