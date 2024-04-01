import type { Meta, StoryObj } from '@storybook/angular';
import { ScanListComponent } from './scan-list.component';

import { within } from '@storybook/testing-library';
import { expect } from '@storybook/jest';

const meta: Meta<ScanListComponent> = {
    component: ScanListComponent,
    title: 'ScanListComponent',
};
export default meta;
type Story = StoryObj<ScanListComponent>;

export const Primary: Story = {
    args: {
        items: [],
    },
};

export const Heading: Story = {
    args: {
        items: [],
    },
    play: async ({ canvasElement }) => {
        const canvas = within(canvasElement);
        expect(canvas.getByText(/scan-list works!/gi)).toBeTruthy();
    },
};
