import {
    AfterViewInit,
    ChangeDetectionStrategy,
    Component,
    HostListener,
    Input,
    NgZone,
    OnChanges,
    OnDestroy,
    OnInit,
    SimpleChanges,
    ViewContainerRef,
    ViewEncapsulation,
} from '@angular/core';
import { ImageListStore } from '../image-list.store';
import { BehaviorSubject, Observable, Subscription, debounceTime, distinctUntilChanged } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { ClickNotificationService } from '../../../services/click-notification.service';
import { RowInfo } from '../row-info';

@Component({
    selector: 'glr-image-list-container',
    templateUrl: './image-list-container.component.html',
    styleUrls: ['./image-list-container.component.scss'],
    encapsulation: ViewEncapsulation.Emulated,
    changeDetection: ChangeDetectionStrategy.OnPush,
    providers: [ClickNotificationService, ImageListStore],
})
export class ImageListContainerComponent implements OnDestroy, AfterViewInit, OnChanges {
    private topPosition$ = new BehaviorSubject<number>(0);

    private resizeObserver?: ResizeObserver;

    private componentWidthSubject$ = new BehaviorSubject<number>(500);
    readonly componentWidth$ = this.componentWidthSubject$.pipe(debounceTime(100), distinctUntilChanged());

    private overlayClickSubscription?: Subscription;

    readonly rowsInfo$: Observable<RowInfo[]> = this.imagesStore.getVisibleRows(
        this.componentWidth$,
        this.topPosition$.pipe(debounceTime(200)),
        this.viewContainerRef.element.nativeElement
    );

    @HostListener('scroll', ['$event.target'])
    handleScroll(element: HTMLElement): void {
        this.topPosition$.next(element.scrollTop);
    }

    @Input()
    rootId!: number;

    constructor(
        private readonly imagesStore: ImageListStore,
        private readonly zone: NgZone,
        private readonly router: Router,
        private readonly route: ActivatedRoute,
        private readonly viewContainerRef: ViewContainerRef,
        clickNotification: ClickNotificationService
    ) {
        this.overlayClickSubscription = clickNotification.clicks.subscribe({
            next: () => this.router.navigate(['./'], { relativeTo: this.route }),
        });
    }

    ngOnDestroy(): void {
        this.overlayClickSubscription?.unsubscribe();
        this.resizeObserver?.disconnect();
    }

    ngAfterViewInit() {
        this.attachResizeObserver();
    }

    ngOnChanges(): void {
        this.imagesStore.clearState();
        this.imagesStore.fetchImages({
            parentId: this.rootId,
            take: 1000,
            skip: 0,
        });
    }

    private attachResizeObserver() {
        this.resizeObserver = new ResizeObserver((entries) => {
            const first = entries[0];
            this.zone.run(() => {
                const width = first.contentRect.width;
                this.componentWidthSubject$.next(width - 15);
            });
        });

        const wrapperEl = this.viewContainerRef.element.nativeElement;
        this.resizeObserver.observe(wrapperEl);
    }
}
