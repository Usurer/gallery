import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ComponentStore } from '@ngrx/component-store';
import { EMPTY, Observable, Subject, catchError, map, mergeMap, switchMap, takeUntil, tap } from 'rxjs';
import { SettingsService } from 'src/app/services/settings.service';

interface FolderScan {
    id?: number;
    path: string;
    isScanned?: boolean;
    isInvalid?: boolean;
}

interface ScanManagerState {
    scans: FolderScan[];
}

@Injectable()
export class ScanManagerStore extends ComponentStore<ScanManagerState> {
    constructor(private httpClient: HttpClient, private settings: SettingsService) {
        super({ scans: [] });
    }

    readonly scans$ = this.select((x) => x.scans);

    readonly scanAdditionRequest$: Subject<string> = new Subject();

    private removePathFromState(path: string) {
        this.setState((currentState: ScanManagerState) => {
            return {
                ...currentState,
                scans: currentState.scans.filter((x) => x.path !== path),
            };
        });
    }

    private addPathToState(path: string): void {
        return this.setState((currentState: ScanManagerState) => {
            const newScan: FolderScan = {
                path: path,
            };
            return {
                ...currentState,
                scans: [...currentState.scans, newScan],
            };
        });
    }

    readonly getScans = this.effect<void>((trigger$) => {
        return trigger$.pipe(
            switchMap(() => {
                return this.httpClient.get<FolderScan[]>(`${this.settings.environment.scansApiUri}`);
            }),
            tap((scans) => {
                this.setState(() => {
                    return {
                        scans: scans,
                    };
                });
            })
        );
    });

    readonly addScan = this.effect((path$: Observable<string>) => {
        this.scanAdditionRequest$.next('xxx');

        return path$.pipe(
            tap((path) => this.addPathToState(path)),
            mergeMap((path) => {
                const headers = {
                    'Content-Type': 'application/json',
                };
                return this.httpClient
                    .put<number>(`${this.settings.environment.scansApiUri}`, JSON.stringify(path), {
                        headers,
                    })
                    .pipe(
                        map((response) => response),
                        catchError((error) => {
                            console.log(`Oops! An API access error! ${JSON.stringify(error)}`);

                            this.removePathFromState(path);

                            return EMPTY;
                        })
                    );
            }),
            switchMap(() => {
                return this.httpClient.get<FolderScan[]>(`${this.settings.environment.scansApiUri}`).pipe(
                    takeUntil(this.scanAdditionRequest$),
                    map((scans) => {
                        return this.setState(() => {
                            return {
                                scans: scans,
                            };
                        });
                    })
                );
            })
        );
    });
}

// readonly addScan = this.scanAdditionRequest$.pipe(
//     tap((path) => this.addPathToState(path)),
//     mergeMap((path) => {
//         const headers = {
//             'Content-Type': 'application/json',
//         };
//         return this.httpClient
//             .put<number>(`${this.settings.environment.scansApiUri}`, JSON.stringify(path), {
//                 headers,
//             })
//             .pipe(
//                 map((response) => response),
//                 catchError((error) => {
//                     console.log(`Oops! An API access error! ${JSON.stringify(error)}`);

//                     this.removePathFromState(path);

//                     return EMPTY;
//                 })
//             );
//     }),
//     switchMap(() => {
//         return this.httpClient.get<FolderScan[]>(`${this.settings.environment.scansApiUri}`).pipe(
//             takeUntil(this.scanAdditionRequest$),
//             map((scans) => {
//                 return this.setState(() => {
//                     return {
//                         scans: scans,
//                     };
//                 });
//             })
//         );
//     })
// );
