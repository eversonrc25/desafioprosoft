import { Component, OnInit } from '@angular/core'
import { FormBuilder, FormControl } from '@angular/forms';
import { SistemaFuncionalidadeService } from '@app-core/service/seguranca/sistemafuncionalidade.service';
import { BaseListComponent } from '@framework-core/BaseListComponent';
import { SistemaFuncionalidade } from '@app-core/models/seguranca/sistemafuncionalidade.model';
import { BaseStore } from '@framework-core/utils/BaseStore';

import { Sistema } from '@app-core/models/seguranca/sistema.model'
import { SistemaService } from '@app-core/service/seguranca/sistema.service'

@Component({
  selector: 'app-sistemafuncionalidade-list',
  templateUrl: './sistemafuncionalidade-list.component.html'
})

export class SistemaFuncionalidadeListComponent extends BaseListComponent<SistemaFuncionalidade> implements OnInit {

  titulo = 'Funcionalidade';
  Role = { Create: 'FUNC_C', Update : 'FUNC_U',  Delete : 'FUNC_D', Read: 'FUNC_R' }

  listaSistema: Sistema[] = []

  constructor(protected store: BaseStore<SistemaFuncionalidade>, protected service: SistemaFuncionalidadeService,
              protected serviceSistema: SistemaService,
              protected formBuilder?: FormBuilder) {
    super(store, service, formBuilder);
    this.fieldsDate = [ 'data_dt_cadastro', 'data_dt_edicao' ]
  }
 
  carregarCombo() {
    this.serviceSistema.getList({ situ_tx_situacao: 'A' }, 1, 1000 ).subscribe(retorno => { this.listaSistema = retorno.dados })

  }

  ngOnInit() {
    super.ngOnInit()
    this.carregarCombo()
  }

  createForm() {

    super.createForm();
    this.formFilter.addControl('situ_tx_situacao', new FormControl('A'));
    this.formFilter.addControl('sist_nr_sequencia', new FormControl(null, []));
    this.formFilter.addControl('func_tx_descricao', new FormControl(null, []));
    this.formFilter.addControl('func_tx_nome', new FormControl(null, []));
    this.formFilter.addControl('func_tx_url', new FormControl(null, []));

  }

}