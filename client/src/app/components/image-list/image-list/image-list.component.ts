import { ChangeDetectionStrategy, Component, Input, ViewEncapsulation } from '@angular/core';
import { ImageInfo } from '../../../dto/image-info';
import { animate, style, transition, trigger } from '@angular/animations';
import { RowInfo } from '../row-info';
import { IMAGE_ROUTE } from 'src/app/app-routes';
import { SettingsService } from 'src/app/services/settings.service';

@Component({
    selector: 'glr-image-list',
    templateUrl: './image-list.component.html',
    styleUrls: ['./image-list.component.scss'],
    encapsulation: ViewEncapsulation.Emulated,
    changeDetection: ChangeDetectionStrategy.OnPush,
    animations: [
        trigger('myInsertRemoveTrigger', [
            transition(':enter', [style({ opacity: 0 }), animate('300ms', style({ opacity: 1 }))]),
            transition(':leave', [animate('100ms', style({ opacity: 0 }))]),
        ]),
    ],
})
export class ImageListComponent {
    @Input()
    rowsInfo: RowInfo[] = [];

    public imageRootPrefix = IMAGE_ROUTE;

    public imagePreviewUrl = '';

    constructor(settings: SettingsService) {
        //TODO: We have to invalidate browser cache for changed images
        // As the most simple way to do that - add Updated timestamp for every image in the DB
        // This field should be set to the current datetime on record creation or update
        // This field should be passed from API to the cliend and image api requests should include it as &ts=timestamp
        // Also this field should be included into a disk cache key
        this.imagePreviewUrl = `${settings.environment.imagesApiUri}/GetImagePreview?height=200&id=`;
    }

    trackImage(_: number, itemInfo: ImageInfo): string {
        return `${itemInfo.id}`;
    }

    trackRow(idx: number, rowInfo: RowInfo): string {
        return `${idx}_${rowInfo.rowHeight}_${rowInfo.visible}`;
    }
}
