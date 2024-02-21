import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ComponentStore } from '@ngrx/component-store';
import { EMPTY, Observable, Subject, catchError, map, mergeMap, switchMap, takeUntil, tap } from 'rxjs';
import { SettingsService } from 'src/app/services/settings.service';

export interface FolderScan {
    id?: number;
    path: string;
    isScanned?: boolean;
    isInvalid?: boolean;
}

interface ScanManagerState {
    scans: FolderScan[];
    commonPathPrefix: string;
}

@Injectable()
export class ScanManagerStore extends ComponentStore<ScanManagerState> {
    constructor(private httpClient: HttpClient, private settings: SettingsService) {
        super({ scans: [], commonPathPrefix: '' });
    }

    readonly scans$ = this.select((x) => x.scans);
    readonly prefix$ = this.select((x) => x.commonPathPrefix);

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
                const prefix = this.getCommonPathPrefix(scans);
                this.setState(() => {
                    return {
                        scans: scans,
                        commonPathPrefix: prefix,
                    };
                });
            })
        );
    });

    readonly deleteScan = this.effect((id$: Observable<number>) => {
        return id$.pipe(
            mergeMap((id) => this.httpClient.delete(`${this.settings.environment.scansApiUri}/${id}`)),
            switchMap(() => {
                return this.httpClient.get<FolderScan[]>(`${this.settings.environment.scansApiUri}`).pipe(
                    takeUntil(this.scanAdditionRequest$),
                    map((scans) => {
                        const prefix = this.getCommonPathPrefix(scans);
                        return this.setState(() => {
                            return {
                                scans: scans,
                                commonPathPrefix: prefix,
                            };
                        });
                    })
                );
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
                        const prefix = this.getCommonPathPrefix(scans);
                        return this.setState(() => {
                            return {
                                scans: scans,
                                commonPathPrefix: prefix,
                            };
                        });
                    })
                );
            })
        );
    });

    private getCommonPathPrefix(scans: FolderScan[]): string {
        if (scans.length === 0) return '';

        const firstItem = scans[0].path;
        let result = '';

        for (let i = 1; i <= firstItem.length; i++) {
            const substring = firstItem.substring(0, i);
            for (const scan of scans) {
                if (scan.path.indexOf(substring) !== 0) {
                    return result.slice(0, result.lastIndexOf('\\') + 1);
                }
            }

            result = substring;
        }

        return result.slice(0, result.lastIndexOf('\\') + 1);
    }
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
