import { moduleMetadata, type Meta, type StoryObj } from '@storybook/angular';
import { ScanManagerComponent } from './scan-manager.component';

import { within } from '@storybook/testing-library';
import { expect } from '@storybook/jest';
import { FolderScan, ScanManagerStore } from '../scan-manager.store';
import { BehaviorSubject, EMPTY, Observable, of } from 'rxjs';
import { ScanListComponent } from '../scan-list/scan-list.component';
import { AddScanComponent } from '../add-scan/add-scan.component';

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

let scansMocks = [mockScanned, mockInvalid, mockNeither];

const scansMocksSubject = new BehaviorSubject<FolderScan[]>(scansMocks);

const mockStore = {
    scans$: scansMocksSubject.asObservable(),
    prefix$: of(''),
    getScans: () => scansMocksSubject.asObservable(),
    addScan: (path: string) => {
        const newScan: FolderScan = {
            id: Math.max(...scansMocks.map((x) => x.id || 0)) + 1,
            path: path,
        };
        scansMocks.push(newScan);
        scansMocksSubject.next(scansMocks);
    },
    deleteScan: (id: number) => {
        scansMocks = scansMocks.filter((x) => x.id !== id);
        scansMocksSubject.next(scansMocks);
    },
};

const meta: Meta<ScanManagerComponent> = {
    component: ScanManagerComponent,
    title: 'ScanManagerComponent',
    decorators: [
        moduleMetadata({
            providers: [
                {
                    provide: ScanManagerStore,
                    useValue: mockStore,
                },
            ],
            declarations: [ScanListComponent, AddScanComponent],
            imports: [MatIconModule],
        }),
    ],
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
