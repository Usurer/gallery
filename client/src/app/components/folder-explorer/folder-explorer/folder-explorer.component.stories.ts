import type { Meta, StoryObj } from '@storybook/angular';
import { FolderExplorerComponent } from './folder-explorer.component';

import { within } from '@storybook/testing-library';
import { expect } from '@storybook/jest';

const meta: Meta<FolderExplorerComponent> = {
    component: FolderExplorerComponent,
    title: 'FolderExplorerComponent',
};
export default meta;
type Story = StoryObj<FolderExplorerComponent>;

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
        expect(canvas.getByText(/folder-explorer works!/gi)).toBeTruthy();
    },
};
