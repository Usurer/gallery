import type { Meta, StoryObj } from '@storybook/angular';
import { ImageListComponent } from './image-list.component';

import { within } from '@storybook/testing-library';
import { expect } from '@storybook/jest';

const meta: Meta<ImageListComponent> = {
    component: ImageListComponent,
    title: 'ImageListComponent',
};
export default meta;
type Story = StoryObj<ImageListComponent>;

export const Primary: Story = {
    args: {
        rowsInfo: [],
    },
};

export const Heading: Story = {
    args: {
        rowsInfo: [],
    },
    play: async ({ canvasElement }) => {
        const canvas = within(canvasElement);
        expect(canvas.getByText(/image-list works!/gi)).toBeTruthy();
    },
};
