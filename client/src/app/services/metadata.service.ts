import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { SettingsService } from './settings.service';

export interface CollectionMetadata {
    rootId: number;
    itemsCount: number;
    iItemsPerMonth: object;
}

@Injectable({
    providedIn: 'root',
})
export class MetadataService {
    constructor(private httpClient: HttpClient, private settings: SettingsService) {}

    // TODO: don't like this name, rename here and in the API
    public getCollectionMetadata(parentId?: number): Observable<CollectionMetadata> {
        const root = this.settings.environment.metaApiUri;
        return this.httpClient.get<CollectionMetadata>(`${root}/folderImages/${parentId}`);
    }
}
