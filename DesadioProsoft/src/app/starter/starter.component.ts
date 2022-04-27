import {
  Component,
  ChangeDetectionStrategy,
  OnInit
} from '@angular/core';




 
@Component({
  selector: 'app-starter',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './starter.component.html'  
})
export class StarterComponent  implements OnInit {
 
 
 
 
  constructor( ) { }

  public ngOnInit() { }

  
 
}
