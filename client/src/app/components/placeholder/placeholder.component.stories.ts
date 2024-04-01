import type { Meta, StoryObj } from '@storybook/angular';
import { PlaceholderComponent } from './placeholder.component';

import { within } from '@storybook/testing-library';
import { expect } from '@storybook/jest';

const meta: Meta<PlaceholderComponent> = {
    component: PlaceholderComponent,
    title: 'PlaceholderComponent',
};
export default meta;
type Story = StoryObj<PlaceholderComponent>;

export const Primary: Story = {
    args: {},
};

export const Heading: Story = {
    args: {},
    play: async ({ canvasElement }) => {
        const canvas = within(canvasElement);
        expect(canvas.getByText(/placeholder works!/gi)).toBeTruthy();
    },
};
