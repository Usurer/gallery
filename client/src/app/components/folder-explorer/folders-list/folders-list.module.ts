import { NgModule } from '@angular/core';
import { FoldersListContainerComponent } from './folders-list-container/folders-list.container.component';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { FoldersListStore } from './folders-list.store';

@NgModule({
    declarations: [FoldersListContainerComponent],
    exports: [FoldersListContainerComponent],
    imports: [CommonModule, MatIconModule],
    providers: [FoldersListStore],
})
export class FoldersListModule {}
