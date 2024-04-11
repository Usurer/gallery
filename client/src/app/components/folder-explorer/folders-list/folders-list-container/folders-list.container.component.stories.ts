import { moduleMetadata, type Meta, type StoryObj } from '@storybook/angular';
import { FoldersListContainerComponent } from './folders-list.container.component';

import { within } from '@storybook/testing-library';
import { expect } from '@storybook/jest';
import { FoldersListStore } from '../folders-list.store';
import { ListItemsQuery } from 'src/app/common/list-items-query';
import { of } from 'rxjs';
import { FolderInfo } from 'src/app/dto/folder-info';

const folderInfos: FolderInfo[] = [
    {
        id: 1,
        name: 'Folder 1',
        updatedAtDate: 0,
    },
    {
        id: 2,
        name: 'Folder 2',
        updatedAtDate: 0,
    },
];

const meta: Meta<FoldersListContainerComponent> = {
    component: FoldersListContainerComponent,
    title: 'FoldersListContainerComponent',
    decorators: [
        moduleMetadata({
            providers: [
                {
                    provide: FoldersListStore,
                    useValue: {
                        fetchFolders: of<FolderInfo[]>(folderInfos),
                        setItems: () => undefined,
                        select: () => of<FolderInfo[]>(folderInfos),
                    },
                },
            ],
        }),
    ],
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
