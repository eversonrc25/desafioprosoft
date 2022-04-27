import { Component } from '@angular/core'
import { FormBuilder, FormControl } from '@angular/forms';
import { PerfilService } from '@app-core/service/seguranca/perfil.service';
import { BaseListComponent } from '@framework-core/BaseListComponent';
import { Perfil } from '@app-core/models/seguranca/perfil.model';
import { BaseStore } from '@framework-core/utils/BaseStore';

@Component({
  selector: 'app-perfil-list',
  templateUrl: './perfil-list.component.html'
})

export class PerfilListComponent extends BaseListComponent<Perfil> {

  titulo = 'Lista de Perfil'
  Role = { Create: 'PERF_C', Update : 'PERF_U',  Delete : 'PERF_D', Read: 'PERF_R' }

  constructor(protected store: BaseStore<Perfil>, protected service: PerfilService, 
     protected formBuilder?: FormBuilder ) {
    super(store, service, formBuilder);
    this.fieldsDate = [ 'data_dt_cadastro', 'data_dt_edicao' ]
  }

  createForm() {

    super.createForm();
    this.formFilter.addControl('situ_tx_situacao', new FormControl('A'));
    this.formFilter.addControl('perf_tx_descricao', new FormControl(null, []));
    this.formFilter.addControl('situ_tx_situacao', new FormControl(null, []));

  }

}