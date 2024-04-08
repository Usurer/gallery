import { Pipe, PipeTransform } from '@angular/core';
import { SettingsService } from '../services/settings.service';

@Pipe({
    name: 'imageUrl',
    pure: true,
})
export class ImageUrlPipe implements PipeTransform {
    constructor(private settings: SettingsService) {}

    transform(id: string) {
        return `${this.settings.environment.imagesApiUri}/${id}`;
    }
}
