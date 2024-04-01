import type { Meta, StoryObj } from '@storybook/angular';
import { ImageListContainerComponent } from './image-list-container.component';

import { within } from '@storybook/testing-library';
import { expect } from '@storybook/jest';

const meta: Meta<ImageListContainerComponent> = {
    component: ImageListContainerComponent,
    title: 'ImageListContainerComponent',
};
export default meta;
type Story = StoryObj<ImageListContainerComponent>;

export const Primary: Story = {
    args: {
        rootId: 0,
    },
};

export const Heading: Story = {
    args: {
        rootId: 0,
    },
    play: async ({ canvasElement }) => {
        const canvas = within(canvasElement);
        expect(canvas.getByText(/image-list-container works!/gi)).toBeTruthy();
    },
};
