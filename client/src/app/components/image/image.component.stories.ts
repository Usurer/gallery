import { moduleMetadata, type Meta, type StoryObj } from '@storybook/angular';
import { ImageComponent } from './image.component';

import { within } from '@storybook/testing-library';
import { expect } from '@storybook/jest';
import { Pipe, PipeTransform } from '@angular/core';
import { SettingsService } from 'src/app/services/settings.service';

@Pipe({ name: 'imageUrl' })
class MockPipe implements PipeTransform {
    transform(value: number): string {
        return `/${value}.jpg`;
    }
}

const meta: Meta<ImageComponent> = {
    component: ImageComponent,
    title: 'ImageComponent',
    decorators: [
        moduleMetadata({
            declarations: [MockPipe],
        }),
    ],
};
export default meta;
type Story = StoryObj<ImageComponent>;

export const Primary: Story = {
    args: {
        id: 1,
    },
};

export const Heading: Story = {
    args: {},
    play: async ({ canvasElement }) => {
        const canvas = within(canvasElement);
        expect(canvas.getByText(/image works!/gi)).toBeTruthy();
    },
};
