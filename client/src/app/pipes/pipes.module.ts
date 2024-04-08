import { NgModule } from '@angular/core';
import { ImagePreviewUrlPipe } from './image-preview-url.pipe';
import { ImageUrlPipe } from './image-url.pipe';

@NgModule({
    declarations: [ImagePreviewUrlPipe, ImageUrlPipe],
    exports: [ImagePreviewUrlPipe, ImageUrlPipe],
})
export class PipesModule {}
