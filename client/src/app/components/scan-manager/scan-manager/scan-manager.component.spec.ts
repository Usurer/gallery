import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ScanManagerComponent } from './scan-manager.component';

describe('ScanManagerComponent', () => {
    let component: ScanManagerComponent;
    let fixture: ComponentFixture<ScanManagerComponent>;

    beforeEach(async () => {
        await TestBed.configureTestingModule({
            declarations: [ScanManagerComponent],
        }).compileComponents();

        fixture = TestBed.createComponent(ScanManagerComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
