import { ChangeDetectionStrategy, Component } from '@angular/core';

@Component({
  selector: 'glr-placeholder',
  templateUrl: './placeholder.component.html',
  styleUrls: ['./placeholder.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PlaceholderComponent {}
