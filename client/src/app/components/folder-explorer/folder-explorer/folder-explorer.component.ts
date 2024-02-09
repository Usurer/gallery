import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { FOLDER_ROUTE } from 'src/app/app-routes';

@Component({
    selector: 'glr-folder-explorer',
    templateUrl: './folder-explorer.component.html',
    styleUrls: ['./folder-explorer.component.scss'],
})
export class FolderExplorerComponent {
    @Input()
    rootId!: number;

    constructor(private router: Router) {}

    onFolderSelected(folderId: number): void {
        console.log(`select ${folderId}`);
        if (folderId !== this.rootId) {
            this.router.navigate([`/${FOLDER_ROUTE}`, folderId]);
        }
    }
}
