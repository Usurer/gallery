import { ChangeDetectionStrategy, Component, Input, OnInit, ViewEncapsulation } from '@angular/core';
import { ImageListStore } from '../image-list/image-list.store';
import { ActivatedRoute, ParamMap, Router } from '@angular/router';
import { Observable, map, tap } from 'rxjs';

@Component({
    selector: 'glr-image',
    templateUrl: './image.component.html',
    styleUrls: ['./image.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
    encapsulation: ViewEncapsulation.Emulated,
})
export class ImageComponent {
    constructor(private route: ActivatedRoute, private router: Router) {}
    private _id!: string; // TODO: Should it be nullable?
    public query: string | undefined;

    @Input()
    get id(): string | undefined {
        return this._id;
    }

    set id(value: string) {
        this._id = value;
        this.query = `http://localhost:5279/Images/GetImage?id=${value}`;
    }

    @Input()
    width: number | undefined;

    @Input()
    height: number | undefined;
}
