import { Injectable } from "@angular/core";
import { Observable, Subject } from "rxjs";

@Injectable()
export class ClickNotificationService {
    private click$ = new Subject<boolean>();

    get clicks(): Observable<boolean> {
        return this.click$.asObservable();
    }

    click(): void {
        this.click$.next(true);
    }
}