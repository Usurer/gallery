/* eslint-disable @typescript-eslint/no-non-null-assertion */
import { Injectable } from '@angular/core';
import { ImageInfo } from '../dto/image-info';

export type Row = ImageInfo[];

@Injectable({
    providedIn: 'root',
})
export class GalleryLayoutService {
    // TODO: TESTS!!!!!!
    public groupIntoRows(images: ImageInfo[], containerWidth: number): Row[] {
        performance.mark('define-rows-start');

        const rows: Row[] = [];

        let buffer: ImageInfo[] = [];
        let lineWidth = 0;

        const rowHeight = 200;

        for (let i = 0; i < images.length; i++) {
            const current = {
                ...images[i],
            };

            if (!current.width || !current.height) continue;

            const ratio = current.width / current.height;

            const resizedWidth = ratio * rowHeight;
            const diff = lineWidth + resizedWidth - containerWidth;

            current.width = resizedWidth;
            current.height = rowHeight;

            if (diff <= 0) {
                // We can safely add current img to the row
                lineWidth = lineWidth + resizedWidth;
                buffer.push(current);
            } else {
                // The resulting row size is greater than the row size, but not too much
                // We can make all images smaller
                if (diff < resizedWidth / 2) {
                    buffer.push(current);
                    buffer = this.resizeBufferToSmaller(buffer, containerWidth);
                    rows.push(buffer);
                    buffer = [];
                    lineWidth = 0;
                } else {
                    // the resulting row width is too big, so we don't want to add this image
                    // instead make all previous images bigger
                    buffer = this.resizeBufferToBigger(buffer, containerWidth);
                    rows.push(buffer);
                    buffer = [current];
                    lineWidth = current.width;
                }
            }
        }

        // if the last row is not full we haven't added these items to the rows yet
        if (buffer.length > 0) {
            rows.push(buffer);
        }

        performance.mark('define-rows-end');
        const perf = performance.measure('my-measure', 'define-rows-start', 'define-rows-end');
        console.log(perf.duration);

        return rows;
    }

    public resizeBufferToSmaller(row: ImageInfo[], containerWidth: number): ImageInfo[] {
        // const widths = new Array<number>(buffer.length);
        const currentRowWidth = row.reduce((acc, item) => acc + (item.width ?? 0), 0);
        const ratio = currentRowWidth / containerWidth; // should be > 1
        const resizedImages = row.map((x) => {
            return {
                ...x,
                height: (x.height ?? 0) / ratio,
                width: (x.width ?? 0) / ratio,
            };
        });

        return resizedImages;
    }

    // It's identical to the resizeBufferToSmaller
    public resizeBufferToBigger(row: ImageInfo[], containerWidth: number): ImageInfo[] {
        const currentRowWidth = row.reduce((acc, item) => acc + (item.width ?? 0), 0);
        const ratio = currentRowWidth / containerWidth; // should be < 1
        const resizedImages = row.map((x) => {
            return {
                ...x,
                height: (x.height ?? 0) / ratio,
                width: (x.width ?? 0) / ratio,
            };
        });

        return resizedImages;
    }
}
