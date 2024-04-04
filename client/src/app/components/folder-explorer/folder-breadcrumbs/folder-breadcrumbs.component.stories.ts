import { moduleMetadata, type Meta, type StoryObj } from '@storybook/angular';
import { FolderBreadcrumbsComponent } from './folder-breadcrumbs.component';

import { FolderHierarchyStore } from './folder-hierarchy.store';
import { of } from 'rxjs';
import { FolderInfo } from 'src/app/dto/folder-info';

const meta: Meta<FolderBreadcrumbsComponent> = {
    component: FolderBreadcrumbsComponent,
    title: 'FolderBreadcrumbsComponent',
    decorators: [
        moduleMetadata({
            providers: [
                {
                    provide: FolderHierarchyStore,
                    useValue: {
                        select: () => of<FolderInfo[]>([{ id: 1, name: 'test', updatedAtDate: 0 }]),
                        fetchHierarcy: (): void => undefined,
                    },
                },
            ],
        }),
    ],
};

export default meta;

type Story = StoryObj<FolderBreadcrumbsComponent>;

const Base: Story = {
    args: {
        rootId: 0,
    },
};

export const Primary: Story = {
    ...Base,
};
