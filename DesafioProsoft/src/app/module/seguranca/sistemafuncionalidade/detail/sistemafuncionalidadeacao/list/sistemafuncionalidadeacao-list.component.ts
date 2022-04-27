import { Component, Input, Output, EventEmitter } from '@angular/core'
import { FormBuilder, FormControl } from '@angular/forms';
import { BaseListChildComponent } from '@framework-core/BaseListChildComponent';
import { SistemaFuncionalidadeAcaoService } from '@app-core/service/seguranca/sistemafuncionalidadeacao.service';
import { SistemaFuncionalidadeAcao } from '@app-core/models/seguranca/sistemafuncionalidadeacao.model';
import { BaseStore } from '@framework-core/utils/BaseStore';

@Component({
  selector: 'app-sistemafuncionalidadeacao-list',
  templateUrl: './sistemafuncionalidadeacao-list.component.html',
  inputs: ['nameIdParent', 'idParent']
})

export class SistemaFuncionalidadeAcaoListComponent extends BaseListChildComponent<SistemaFuncionalidadeAcao> {
  @Input("nameIdParent") _nameIdParent : string;
  @Input("idParent") _idParent: String;
  @Output() onClickAction = new EventEmitter<any>();
  titulo = 'Lista de ação'

  constructor(protected store: BaseStore<SistemaFuncionalidadeAcao>, 
    protected service: SistemaFuncionalidadeAcaoService,
    protected formBuilder?: FormBuilder ) {
    super('SistemaFuncionalidadeAcao', 'sistemafuncionalidade', store, service, formBuilder);
    this.fieldsDate = [ 'data_dt_cadastro', 'data_dt_edicao' ];
  }

  createForm() {

    super.createForm();
    this.formFilter.addControl('situ_tx_situacao', new FormControl('A'));
    this.formFilter.addControl('fuac_tx_descricao', new FormControl(null, []));
    this.formFilter.addControl('fuac_tx_nome', new FormControl(null, []));
    this.formFilter.addControl('fuac_tx_rota', new FormControl(null, []));

  }

}