import type { Meta, StoryObj } from '@storybook/angular';
import { ImagePopupComponent } from './image-popup.component';

import { within } from '@storybook/testing-library';
import { expect } from '@storybook/jest';

const meta: Meta<ImagePopupComponent> = {
    component: ImagePopupComponent,
    title: 'ImagePopupComponent',
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
