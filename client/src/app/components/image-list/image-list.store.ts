import { ComponentStore, tapResponse } from '@ngrx/component-store';
import { switchMap, catchError, Observable, EMPTY, withLatestFrom, map, combineLatest, filter } from 'rxjs';
import { Inject, Injectable } from '@angular/core';
import { ImageInfo } from '../../dto/image-info';
import { HttpClient } from '@angular/common/http';
import { GalleryLayoutService } from '../../services/gallery-layout.service';
import { RowInfo } from './row-info';
import { ListItemsQuery } from '../../common/list-items-query';
import { SettingsService } from 'src/app/services/settings.service';

interface ImagesState {
    images: ImageInfo[];
}

@Injectable()
export class ImageListStore extends ComponentStore<ImagesState> {
    constructor(
        private settings: SettingsService,
        private httpClient: HttpClient,
        private readonly galleryLayoutService: GalleryLayoutService
    ) {
        super({ images: [] });
    }

    readonly fetchImages = this.effect((query$: Observable<ListItemsQuery>) => {
        return query$.pipe(
            withLatestFrom(this.select((state) => state.images)),
            switchMap(([query, images]) => {
                const parentId = query.parentId ? `${query.parentId}` : '';
                const take = query.take ?? 10;
                const skip = query.skip ?? images.length ?? 0;
                const extensions = ['.jpg', '.jpeg'].map((x) => `&extensions=${x}`).join('');
                const url = `${this.settings.environment.imagesApiUri}/ListItems/${parentId}?take=${take}&skip=${skip}${extensions}`;

                return this.httpClient.get<ImageInfo[]>(url).pipe(
                    tapResponse(
                        (imageList) => {
                            if (imageList?.length) {
                                this.addItems(imageList);
                            }
                        },
                        (error) => {
                            throw error;
                        }
                    )
                );
            }),
            catchError((error) => {
                console.log(`Oops! An API access error! ${JSON.stringify(error)}`);
                return EMPTY;
            })
        );
    });

    readonly clearState = this.updater(() => ({
        images: [],
    }));

    private addItems = this.updater((state, items: ImageInfo[]) => ({
        images: [...state.images, ...items],
    }));

    private resizeRows = (rowWidth$: Observable<number>) => {
        return rowWidth$.pipe(
            switchMap((size) =>
                this.select((state) => state.images).pipe(
                    map((images) => this.galleryLayoutService.groupIntoRows(images, size))
                )
            )
        );
    };

    readonly getVisibleRows = (
        rowWidth$: Observable<number>,
        scrollTop$: Observable<number>,
        container: HTMLElement
    ): Observable<RowInfo[]> => {
        const rows$ = this.resizeRows(rowWidth$);
        return combineLatest([rows$, scrollTop$]).pipe(
            map(([rows, scrollTop]) => {
                const containerHeight = container.clientHeight ?? container.getBoundingClientRect().height;
                return rows && rows.length > 0 ? this.setRowsVisibility(rows, scrollTop, containerHeight) : [];
            })
        );
    };

    private setRowsVisibility(rows: ImageInfo[][], scrollTop: number, containerHeight: number): RowInfo[] {
        const result = [];
        const rowHeight = rows[0][0].height ?? 0;

        const rowsInView = Math.ceil(containerHeight / rowHeight);

        const numberOfScrolledRows = Math.floor(scrollTop / rowHeight);

        const visibilityStartIdx = numberOfScrolledRows - rowsInView * 2;
        const visibilityEndIdx = numberOfScrolledRows + rowsInView * 2;

        for (let i = 0; i < rows.length; i++) {
            const rowInfo: RowInfo = {
                row: rows[i],
                visible: i >= visibilityStartIdx && i <= visibilityEndIdx,
                rowHeight: rowHeight,
            };
            result.push(rowInfo);
        }

        return result;
    }
}
