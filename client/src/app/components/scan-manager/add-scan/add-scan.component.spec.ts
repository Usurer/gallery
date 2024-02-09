import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AddScanComponent } from './add-scan.component';

describe('AddScanComponent', () => {
    let component: AddScanComponent;
    let fixture: ComponentFixture<AddScanComponent>;

    beforeEach(async () => {
        await TestBed.configureTestingModule({
            declarations: [AddScanComponent],
        }).compileComponents();

        fixture = TestBed.createComponent(AddScanComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
