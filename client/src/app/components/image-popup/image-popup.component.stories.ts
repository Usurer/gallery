import { moduleMetadata, type Meta, type StoryObj, componentWrapperDecorator } from '@storybook/angular';
import { ImagePopupComponent } from './image-popup.component';

import { within } from '@storybook/testing-library';
import { expect } from '@storybook/jest';
import { CUSTOM_ELEMENTS_SCHEMA, Pipe, PipeTransform } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { of } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { ImageInfo } from 'src/app/dto/image-info';
import { SettingsService } from 'src/app/services/settings.service';
import { ClickNotificationService } from 'src/app/services/click-notification.service';
import { ImageComponent } from '../image/image.component';

@Pipe({ name: 'imageUrl' })
class MockPipe implements PipeTransform {
    transform(value: number): string {
        return `/birdie.jpg`;
    }
}

const meta: Meta<ImagePopupComponent> = {
    component: ImagePopupComponent,
    title: 'ImagePopupComponent',
    decorators: [
        moduleMetadata({
            declarations: [MockPipe, ImageComponent],
            providers: [
                {
                    provide: ActivatedRoute,
                    useValue: {
                        paramMap: of(new Map([['id', 1]])),
                    },
                },
                {
                    provide: HttpClient,
                    useValue: {
                        get: () => {
                            const info: ImageInfo = {
                                id: 1,
                                height: 200,
                                width: 200,
                                name: 'test',
                                updatedAtDate: 0,
                            };
                            return of(info);
                        },
                    },
                },
                {
                    provide: SettingsService,
                    useValue: {
                        environment: {
                            metaApiUri: 'http://meta',
                        },
                    },
                },
                {
                    provide: ClickNotificationService,
                    useValue: {
                        click: () => console.log('click'),
                    },
                },
            ],
            schemas: [CUSTOM_ELEMENTS_SCHEMA],
        }),
        componentWrapperDecorator((story) => `<glr-image-list-container>${story}</glr-image-list-container>`),
    ],
};

export default meta;
type Story = StoryObj<ImagePopupComponent>;

export const Primary: Story = {
    args: {},
};

export const Heading: Story = {
    args: {},
    play: async ({ canvasElement }) => {
        const canvas = within(canvasElement);
        expect(canvas.getByText(/image-popup works!/gi)).toBeTruthy();
    },
};
