import { Component } from '@angular/core'
import { FormBuilder, FormControl } from '@angular/forms';
import { DevService } from '@app-core/service/dev/dev.service';
import { BaseListComponent } from '@framework-core/BaseListComponent';

import { Dev } from '@app-core/models/dev/dev.model';
import { BaseStore } from '@framework-core/utils/BaseStore';


@Component({
  selector: 'app-dev-list',
  templateUrl: './dev-list.component.html'
})

export class DevListComponent extends BaseListComponent<Dev> {

  titulo = 'Devs'


  constructor(protected store: BaseStore<Dev>, protected service: DevService,
    protected formBuilder: FormBuilder) {
    super(store, service, formBuilder);
    //   this.fieldsDate = [ 'data_dt_cadastro', 'data_dt_edicao' ]

  }

  ngOnInit() {
    super.ngOnInit();

  }

  createForm() {
    super.createForm();
    //this.formFilter.addControl('id', new FormControl(null, []));
    this.formFilter.addControl('name', new FormControl(null, []));
    this.formFilter.addControl('squad', new FormControl(null, []));
    this.formFilter.addControl('login', new FormControl(null, []));
    this.formFilter.addControl('email', new FormControl(null, []));
  }


}
 


 
