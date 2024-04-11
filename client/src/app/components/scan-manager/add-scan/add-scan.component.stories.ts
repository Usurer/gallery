import type { Meta, StoryObj } from '@storybook/angular';
import { AddScanComponent } from './add-scan.component';

import { within } from '@storybook/testing-library';
import { expect } from '@storybook/jest';

const meta: Meta<AddScanComponent> = {
    component: AddScanComponent,
    title: 'AddScanComponent',
};
export default meta;
type Story = StoryObj<AddScanComponent>;

export const Primary: Story = {
    args: {},
};

export const Heading: Story = {
    args: {},
    play: async ({ canvasElement }) => {
        const canvas = within(canvasElement);
        expect(canvas.getByText(/add-scan works!/gi)).toBeTruthy();
    },
};
