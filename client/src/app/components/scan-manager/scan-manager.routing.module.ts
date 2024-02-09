import { SCAN_MANAGER_ROUTE } from 'src/app/app-routes';
import { ScanManagerComponent } from './scan-manager/scan-manager.component';
import { Route, RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';

const ROUTE_ROOT = SCAN_MANAGER_ROUTE;

const ROUTES: Route[] = [
    {
        path: `${ROUTE_ROOT}`,
        component: ScanManagerComponent,
    },
];

@NgModule({
    imports: [RouterModule.forChild(ROUTES)],
    exports: [RouterModule],
})
export class ScanManagerRoutingModule {}
