import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ComponentStore } from '@ngrx/component-store';
import { EMPTY, Observable, catchError, map, mergeMap, switchMap, tap } from 'rxjs';
import { SettingsService } from 'src/app/services/settings.service';

interface ScanManagerState {
    scans: string[];
}

@Injectable()
export class ScanManagerStore extends ComponentStore<ScanManagerState> {
    constructor(private httpClient: HttpClient, private settings: SettingsService) {
        super({ scans: [] });
    }

    readonly scans$ = this.select((x) => x.scans);

    readonly addScan = this.effect((path$: Observable<string>) => {
        return path$.pipe(
            tap((path) =>
                this.setState((currentState: ScanManagerState) => {
                    return {
                        ...currentState,
                        scans: [...currentState.scans, path],
                    };
                })
            ),
            mergeMap((path) => {
                const headers = {
                    'Content-Type': 'application/json',
                };
                return this.httpClient
                    .put(`${this.settings.environment.scansApiUri}/AddScan`, JSON.stringify(path), {
                        headers,
                    })
                    .pipe(
                        map((response) => response),
                        catchError((error) => {
                            console.log(`Oops! An API access error! ${JSON.stringify(error)}`);

                            this.setState((currentState: ScanManagerState) => {
                                return {
                                    ...currentState,
                                    scans: currentState.scans.filter((x) => x !== path),
                                };
                            });

                            return EMPTY;
                        })
                    );
            })
        );
    });
}
