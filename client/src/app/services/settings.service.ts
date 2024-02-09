import { Inject, Injectable } from '@angular/core';
import { ENVIRONMENT_CONFIG, EnvironmentConfig } from 'src/environments/environment-config';

@Injectable({ providedIn: 'root' })
export class SettingsService {
    constructor(@Inject(ENVIRONMENT_CONFIG) public environment: EnvironmentConfig) {}
}
