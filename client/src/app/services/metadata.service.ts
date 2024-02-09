import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

export interface CollectionMetadata {
    rootId: number;
    itemsCount: number;
    iItemsPerMonth: object;
}

@Injectable({
    providedIn: 'root',
})
export class MetadataService {
    constructor(private httpClient: HttpClient) {}

    // TODO: don't like this name, rename here and in the API
    public getImagesMetadata(parentId?: number): Observable<CollectionMetadata> {
        return this.httpClient.get<CollectionMetadata>(
            parentId
                ? `http://localhost:5279/Meta/GetImagesMetadata?parentId=${parentId}`
                : `http://localhost:5279/Meta/GetImagesMetadata`
        );
    }
}
