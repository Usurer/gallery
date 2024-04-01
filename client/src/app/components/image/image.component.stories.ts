import type { Meta, StoryObj } from '@storybook/angular';
import { ImageComponent } from './image.component';

import { within } from '@storybook/testing-library';
import { expect } from '@storybook/jest';

const meta: Meta<ImageComponent> = {
    component: ImageComponent,
    title: 'ImageComponent',
};
export default meta;
type Story = StoryObj<ImageComponent>;

export const Primary: Story = {
    args: {},
};

export const Heading: Story = {
    args: {},
    play: async ({ canvasElement }) => {
        const canvas = within(canvasElement);
        expect(canvas.getByText(/image works!/gi)).toBeTruthy();
    },
};
