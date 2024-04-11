import { moduleMetadata, type Meta, type StoryObj } from '@storybook/angular';
import { ScanListComponent } from './scan-list.component';

import { within } from '@storybook/testing-library';
import { expect } from '@storybook/jest';
import { FolderScan } from '../scan-manager.store';
import { MatIconModule } from '@angular/material/icon';

const mockScanned: FolderScan = {
    id: 1,
    path: 'Test 1',
    isScanned: true,
};

const mockInvalid: FolderScan = {
    id: 2,
    path: 'Test 2',
    isInvalid: true,
};

const mockNeither: FolderScan = {
    id: 3,
    path: 'Test 3',
};

const scansMocks = [mockScanned, mockInvalid, mockNeither];

const meta: Meta<ScanListComponent> = {
    component: ScanListComponent,
    title: 'ScanListComponent',
    decorators: [
        moduleMetadata({
            imports: [MatIconModule],
        }),
    ],
};
export default meta;
type Story = StoryObj<ScanListComponent>;

export const Primary: Story = {
    args: {
        items: scansMocks,
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
