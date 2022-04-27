import { Component } from '@angular/core'
import { FormBuilder, FormControl } from '@angular/forms';
import { SistemaService } from '@app-core/service/seguranca/sistema.service';
import { BaseListComponent } from '@framework-core/BaseListComponent';
import { Sistema } from '@app-core/models/seguranca/sistema.model';
import { BaseStore } from '@framework-core/utils/BaseStore';

@Component({
  selector: 'app-sistema-list',
  templateUrl: './sistema-list.component.html'
})

export class SistemaListComponent extends BaseListComponent<Sistema> {

  titulo = 'Sistema'
   
  Role = { Create: 'SIST_C', Update : 'SIST_U',  Delete : 'SIST_D', Read: 'SIST_R' }

  constructor(protected store: BaseStore<Sistema>, protected service: SistemaService,
    protected formBuilder: FormBuilder  ) {
    super(store, service, formBuilder);
    this.fieldsDate = [ 'data_dt_cadastro', 'data_dt_edicao' ]
    
  }

  createForm() {

    super.createForm();
    this.formFilter.addControl('situ_tx_situacao', new FormControl('A'));
    this.formFilter.addControl('sist_tx_descricao', new FormControl(null, []));

  }

}