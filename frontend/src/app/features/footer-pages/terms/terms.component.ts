import { Component } from '@angular/core';
import {
  trigger,
  state,
  style,
  animate,
  transition,
  query,
  stagger,
} from '@angular/animations';

@Component({
  selector: 'app-terms',
  standalone: true,
  imports: [],
  templateUrl: './terms.component.html',
  styleUrl: './terms.component.css',
  animations: [
    trigger('fadeInOut', [
      state('in', style({ opacity: 1 })),
      transition(':enter', [style({ opacity: 0 }), animate('600ms ease-in')]),
    ]),
    trigger('slideIn', [
      transition(':enter', [
        style({ transform: 'translateY(-100%)' }),
        animate('500ms ease-out', style({ transform: 'translateY(0)' })),
      ]),
    ]),
    trigger('bounceIn', [
      transition(':enter', [
        style({ transform: 'scale(0.3)', opacity: 0 }),
        animate(
          '300ms ease-in-out',
          style({ transform: 'scale(1.1)', opacity: 0.7 })
        ),
        animate(
          '200ms ease-in-out',
          style({ transform: 'scale(1)', opacity: 1 })
        ),
      ]),
    ]),
    trigger('boxAnimation', [
      transition(':enter', [
        style({ transform: 'scale(0.8)', opacity: 0 }),
        animate('500ms ease-out', style({ transform: 'scale(1)', opacity: 1 })),
      ]),
    ]),
    trigger('staggered', [
      transition(':enter', [
        query(':enter', [
          style({ opacity: 0, transform: 'translateY(100px)' }),
          stagger('200ms', [
            animate('500ms ease-out', style({ opacity: 1, transform: 'none' })),
          ]),
        ]),
      ]),
    ]),
  ],
})
export class TermsComponent {}
