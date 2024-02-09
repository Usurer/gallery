import { Route, RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { ImageComponent } from './components/image/image.component';
import { FOLDER_ROUTE, IMAGE_ROUTE } from './app-routes';
import { FolderExplorerComponent } from './components/folder-explorer/folder-explorer/folder-explorer.component';
import { FolderExplorerModule } from './components/folder-explorer/folder-explorer.module';

const APP_ROUTES: Route[] = [
    { path: FOLDER_ROUTE, component: FolderExplorerComponent },
    { path: `${IMAGE_ROUTE}/:id`, component: ImageComponent },
    { path: '', redirectTo: FOLDER_ROUTE, pathMatch: 'full' },
];

@NgModule({
    imports: [
        RouterModule.forRoot(
            APP_ROUTES,
            { enableTracing: true, bindToComponentInputs: true } // <-- debugging purposes only
        ),
        FolderExplorerModule,
    ],
    exports: [RouterModule],
})
export class AppRoutingModule {}
