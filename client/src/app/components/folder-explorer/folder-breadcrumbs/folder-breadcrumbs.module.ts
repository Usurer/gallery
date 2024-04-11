import { NgModule } from '@angular/core';
import { FolderBreadcrumbsComponent } from './folder-breadcrumbs.component';
import { CommonModule } from '@angular/common';
import { FolderHierarchyStore } from './folder-hierarchy.store';

@NgModule({
    declarations: [FolderBreadcrumbsComponent],
    exports: [FolderBreadcrumbsComponent],
    imports: [CommonModule],
    providers: [FolderHierarchyStore]
})
export class FolderBreadcrumbsModule {}
