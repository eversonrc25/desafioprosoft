import { NgModule } from '@angular/core'
import { CommonModule } from '@angular/common'
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from '../../shared/shared.module'
import { NgbModule } from '@ng-bootstrap/ng-bootstrap'
import { NgSelectModule } from '@ng-select/ng-select'; 
import { BaseStore } from '@framework-core/utils/BaseStore';
import { InputMaskModule } from 'racoon-mask-raw';

import { DevRoutingModule } from './dev-routing.module';
import { DevListComponent } from './dev/list/dev-list.component';
import { DevCrudComponent } from './dev/crud/dev-crud.component';
 

 
@NgModule({
  declarations: [
    DevListComponent,
    DevCrudComponent
 
  ],
  
  imports: [CommonModule,
    NgbModule,
    NgSelectModule, FormsModule,
    SharedModule,
    ReactiveFormsModule,
    InputMaskModule,
    DevRoutingModule],
  providers: [BaseStore]
})
export class DevModule { }
