import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { SettingsService } from './services/settings.service';
import { first, tap } from 'rxjs';

@Component({
    selector: 'glr-app',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.scss'],
})
export class AppComponent {
    constructor(private httpClient: HttpClient, private settings: SettingsService) {}

    public purge(): void {
        this.httpClient
            .delete(`${this.settings.environment.internalApiUri}/database/purge`)
            .pipe(
                tap(() => {
                    window.location.reload();
                }),
                first()
            )
            .subscribe();
    }
}
