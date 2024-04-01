import type { Meta, StoryObj } from '@storybook/angular';
import { FoldersListContainerComponent } from './folders-list.container.component';

import { within } from '@storybook/testing-library';
import { expect } from '@storybook/jest';

const meta: Meta<FoldersListContainerComponent> = {
    component: FoldersListContainerComponent,
    title: 'FoldersListContainerComponent',
};
export default meta;
type Story = StoryObj<FoldersListContainerComponent>;

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
        expect(canvas.getByText(/folders-list.container works!/gi)).toBeTruthy();
    },
};
