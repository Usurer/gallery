import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ImageListComponent } from './image-list.component';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

describe('ImageListComponent', () => {
  let component: ImageListComponent;
  let fixture: ComponentFixture<ImageListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ImageListComponent],
      imports: [CommonModule, RouterModule.forRoot([])]
    }).compileComponents();

    fixture = TestBed.createComponent(ImageListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
