import { Pipe, PipeTransform } from '@angular/core';
import { SettingsService } from '../services/settings.service';

@Pipe({
    name: 'imagePreviewUrl',
    pure: true,
})
export class ImagePreviewUrlPipe implements PipeTransform {
    constructor(private settings: SettingsService) {}

    // TODO: Rename timestamp, let it be updatedAtDate as in data model
    transform(id: number, height: number, timestamp: number) {
        return `${this.settings.environment.imagesApiUri}/${id}/preview?height=${height}&timestamp=${timestamp}`;
    }
}
