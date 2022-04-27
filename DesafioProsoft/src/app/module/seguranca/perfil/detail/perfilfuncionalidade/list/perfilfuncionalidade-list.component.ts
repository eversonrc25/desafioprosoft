import { Component, Input, Output, EventEmitter } from '@angular/core'
import { FormBuilder, FormControl } from '@angular/forms';
import { BaseListChildComponent } from '@framework-core/BaseListChildComponent';
import { PerfilFuncionalidadeService } from '@app-core/service/seguranca/perfilfuncionalidade.service';
import { PerfilFuncionalidade } from '@app-core/models/seguranca/perfilfuncionalidade.model';
import { BaseStore } from '@framework-core/utils/BaseStore';

@Component({
  selector: 'app-perfilfuncionalidade-list',
  templateUrl: './perfilfuncionalidade-list.component.html',
  inputs: ['nameIdParent', 'idParent']
})

export class PerfilFuncionalidadeListComponent extends BaseListChildComponent<PerfilFuncionalidade> {

  titulo = 'Funcionalidade do perfil'
  @Input("nameIdParent") _nameIdParent : string;
  @Input("idParent") _idParent: String;
  @Output() onClickAction = new EventEmitter<any>();
  constructor(protected store: BaseStore<PerfilFuncionalidade>, protected service: PerfilFuncionalidadeService,
    protected formBuilder: FormBuilder ) {
    super('PerfilFuncionalidade', 'perfil', store, service, formBuilder);
    this.fieldsDate = [ 'data_dt_cadastro', 'data_dt_edicao' ]
  }

  createForm() {

    super.createForm();
    this.formFilter.addControl('situ_tx_situacao', new FormControl('A'));
    this.formFilter.addControl('situ_tx_situacao', new FormControl(null, []));

  }

}