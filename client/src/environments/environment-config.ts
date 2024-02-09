import { InjectionToken } from '@angular/core';

export interface EnvironmentConfig {
    production: boolean;
    imagesApiUri: string;
    foldersApiUri: string;
    scansApiUri: string;
}

export const ENVIRONMENT_CONFIG = new InjectionToken<EnvironmentConfig>('environment-config');
