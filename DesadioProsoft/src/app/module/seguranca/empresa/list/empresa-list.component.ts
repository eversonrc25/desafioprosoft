import { Component } from '@angular/core'
import { FormBuilder, FormControl } from '@angular/forms';
import { EmpresaService } from '@app-core/service/seguranca/empresa.service';
import { BaseListComponent } from '@framework-core/BaseListComponent';
import { Empresa } from '@app-core/models/seguranca/empresa.model';
import { BaseStore } from '@framework-core/utils/BaseStore';

@Component({
  selector: 'app-empresa-list',
  templateUrl: './empresa-list.component.html'
})

export class EmpresaListComponent extends BaseListComponent<Empresa> {

  titulo = "Lista de Empresa";
  Role = { Create: 'EMPR_C', Update : 'EMPR_U',  Delete : 'EMPR_D', Read: 'EMPR_R' }


  constructor(protected store: BaseStore<Empresa>, protected service: EmpresaService 
    , protected formBuilder?: FormBuilder  ) {
    super(store, service, formBuilder );
    this.fieldsDate = [ 'data_dt_cadastro', 'data_dt_edicao' ];
  }

  createForm() {

    super.createForm();
    this.formFilter.addControl('situ_tx_situacao', new FormControl("A"));
    this.formFilter.addControl('empr_tx_nome', new FormControl(null, []));
    this.formFilter.addControl('empr_tx_razao_social', new FormControl(null, []));
    this.formFilter.addControl('empr_tx_cnpj', new FormControl(null, []));
    this.formFilter.addControl('empr_tx_bd', new FormControl(null, []));
    this.formFilter.addControl('empr_tx_esquema', new FormControl(null, []));


 
  }

}