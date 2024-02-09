import { NgModule } from '@angular/core';
import { FolderBreadcrumbsComponent } from './folder-breadcrumbs.component';
import { CommonModule } from '@angular/common';

@NgModule({
    declarations: [FolderBreadcrumbsComponent],
    exports: [FolderBreadcrumbsComponent],
    imports: [CommonModule],
})
export class FolderBreadcrumbsModule {}
