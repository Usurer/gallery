import { moduleMetadata, type Meta, type StoryObj } from '@storybook/angular';
import { ImageListComponent } from './image-list.component';

import { within } from '@storybook/testing-library';
import { expect } from '@storybook/jest';
import { SettingsService } from 'src/app/services/settings.service';
import { RowInfo } from '../row-info';
import { Pipe, PipeTransform } from '@angular/core';

const rows: RowInfo[] = [
    {
        row: [{ id: 1, name: 'Test img', height: 200, width: 300, updatedAtDate: 0 }],
        rowHeight: 200,
        visible: true,
    },
];

@Pipe({ name: 'imagePreviewUrl' })
class MockPipe implements PipeTransform {
    transform(value: number): string {
        //Do stuff here, if you want
        return '/birdie.jpg';
    }
}

const meta: Meta<ImageListComponent> = {
    component: ImageListComponent,
    title: 'ImageListComponent',
    decorators: [
        moduleMetadata({
            providers: [
                {
                    provide: SettingsService,
                    useValue: {
                        environment: { imagesApiUri: 'http://localhost:127.0.0.1' },
                    },
                },
            ],
            declarations: [MockPipe],
        }),
    ],
};
export default meta;
type Story = StoryObj<ImageListComponent>;

export const Primary: Story = {
    args: {
        rowsInfo: rows,
    },
};

export const Heading: Story = {
    args: {
        rowsInfo: rows,
    },
    play: async ({ canvasElement }) => {
        const canvas = within(canvasElement);
        expect(canvas.getByText(/image-list works!/gi)).toBeTruthy();
    },
};
