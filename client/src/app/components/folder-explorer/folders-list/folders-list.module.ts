import { NgModule } from '@angular/core';
import { FoldersListContainerComponent } from './folders-list-container/folders-list.container.component';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';

@NgModule({
    declarations: [FoldersListContainerComponent],
    exports: [FoldersListContainerComponent],
    imports: [CommonModule, MatIconModule],
})
export class FoldersListModule {}
