import { ChangeDetectionStrategy, Component, Input, ViewEncapsulation } from '@angular/core';
import { ImageInfo } from '../../../dto/image-info';
import { animate, style, transition, trigger } from '@angular/animations';
import { RowInfo } from '../row-info';
import { IMAGE_ROUTE } from 'src/app/app-routes';

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

    trackImage(_: number, itemInfo: ImageInfo): string {
        return `${itemInfo.id}`;
    }

    trackRow(idx: number, rowInfo: RowInfo): string {
        return `${idx}_${rowInfo.rowHeight}_${rowInfo.visible}`;
    }
}
