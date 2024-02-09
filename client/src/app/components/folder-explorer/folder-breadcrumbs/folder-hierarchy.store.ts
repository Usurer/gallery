import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ComponentStore, tapResponse } from '@ngrx/component-store';
import { FolderInfo } from '../../../dto/folder-info';
import { Observable, switchMap } from 'rxjs';
import { SettingsService } from 'src/app/services/settings.service';

interface Store {
    folders: FolderInfo[];
}

@Injectable()
export class FolderHierarchyStore extends ComponentStore<Store> {
    constructor(private httpClient: HttpClient, private settings: SettingsService) {
        super({ folders: [] });
    }

    readonly fetchHierarcy = this.effect((folderId$: Observable<number>) => {
        return folderId$.pipe(
            switchMap((folderId) => {
                const folderParam = folderId ? `${folderId}` : '';
                return this.httpClient.get<FolderInfo[]>(`${this.settings.environment.foldersApiUri}/GetAncestors/${folderParam}`);
            }),
            tapResponse(
                (folders) => this.setItems(folders),
                (error) => {
                    throw error;
                }
            )
        );
    });

    private setItems = this.updater((state, items: FolderInfo[]) => ({
        folders: items,
    }));
}
