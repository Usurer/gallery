import type { Meta, StoryObj } from '@storybook/angular';
import { FolderBreadcrumbsComponent } from './folder-breadcrumbs.component';

import { within } from '@storybook/testing-library';
import { expect } from '@storybook/jest';

const meta: Meta<FolderBreadcrumbsComponent> = {
    component: FolderBreadcrumbsComponent,
    title: 'FolderBreadcrumbsComponent',
};
export default meta;
type Story = StoryObj<FolderBreadcrumbsComponent>;

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
        expect(canvas.getByText(/folder-breadcrumbs works!/gi)).toBeTruthy();
    },
};
