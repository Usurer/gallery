import { moduleMetadata, type Meta, type StoryObj } from '@storybook/angular';
import { ScanManagerComponent } from './scan-manager.component';

import { userEvent, within } from '@storybook/testing-library';
import { expect } from '@storybook/jest';
import { FolderScan, ScanManagerStore } from '../scan-manager.store';
import { BehaviorSubject,of } from 'rxjs';
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

let maxId = 3;

const scansMocks = [mockScanned, mockInvalid, mockNeither];

const scansMocksSubject = new BehaviorSubject<FolderScan[]>(scansMocks);

const mockStore = {
    scans$: scansMocksSubject.asObservable(),
    prefix$: of(''),
    getScans: () => scansMocksSubject.asObservable(),
    addScan: (path: string) => {
        const newScan: FolderScan = {
            id: ++maxId,
            path: path,
        };
        scansMocksSubject.next([...scansMocks, newScan]);
    },
    deleteScan: (id: number) => {
        const currentVal = scansMocksSubject.value;
        scansMocksSubject.next(currentVal.filter((x) => x.id !== id));
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

export const AddNewItem: Story = {
    args: {},
    play: async ({ canvasElement }) => {
        const canvas = within(canvasElement);
        const inputField = canvas.getByTestId('input-file');
        const submitBtn = canvas.getByTestId('btn-submit');

        expect(canvas.getAllByTestId('item-path').find((x) => x.innerText === 'Test 4')).toBeFalsy();

        await userEvent.type(inputField, 'Test 4');
        await userEvent.click(submitBtn);

        expect(canvas.getAllByTestId('item-path').find((x) => x.innerText === 'Test 4')).toBeTruthy();
    },
};

export const RemoveInvalidItem: Story = {
    args: {},
    play: async ({ canvasElement }) => {
        const canvas = within(canvasElement);

        const initialCount = canvas.getAllByTestId('item-path').length;

        const deleteButton = canvas.getByTestId('btn-delete');
        await userEvent.click(deleteButton);

        const count = canvas.getAllByTestId('item-path').length;
        expect(count).toEqual(initialCount - 1);
    },
};
