import { EnvironmentConfig } from './environment-config';

export const environment: EnvironmentConfig = {
    production: false,
    imagesApiUri: 'http://localhost:5279/Images',
    foldersApiUri: 'http://localhost:5279/Folders',
    scansApiUri: 'http://localhost:5279/Scans',
    metaApiUri: 'http://localhost:5279/Meta',
    internalApiUri: 'http://localhost:5279/internals',
};
