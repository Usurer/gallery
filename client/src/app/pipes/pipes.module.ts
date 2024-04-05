import { NgModule } from '@angular/core';
import { ImagePreviewUrlPipe } from './image-preview-url.pipe';

@NgModule({
    declarations: [ImagePreviewUrlPipe],
    exports: [ImagePreviewUrlPipe],
})
export class PipesModule {}
