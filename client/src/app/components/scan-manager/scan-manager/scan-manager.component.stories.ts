import type { Meta, StoryObj } from '@storybook/angular';
import { ScanManagerComponent } from './scan-manager.component';

import { within } from '@storybook/testing-library';
import { expect } from '@storybook/jest';

const meta: Meta<ScanManagerComponent> = {
    component: ScanManagerComponent,
    title: 'ScanManagerComponent',
};
export default meta;
type Story = StoryObj<ScanManagerComponent>;

export const Primary: Story = {
    args: {},
};

export const Heading: Story = {
    args: {},
    play: async ({ canvasElement }) => {
        const canvas = within(canvasElement);
        expect(canvas.getByText(/scan-manager works!/gi)).toBeTruthy();
    },
};
