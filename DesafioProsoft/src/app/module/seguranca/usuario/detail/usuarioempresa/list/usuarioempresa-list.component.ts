import { Component, Input, Output, EventEmitter } from '@angular/core'
import { FormBuilder, FormControl } from '@angular/forms';
import { BaseListChildComponent } from '@framework-core/BaseListChildComponent';
import { UsuarioEmpresaService } from '@app-core/service/seguranca/usuarioempresa.service';
import { UsuarioEmpresa } from '@app-core/models/seguranca/usuarioempresa.model';
import { BaseStore } from '@framework-core/utils/BaseStore';

@Component({
  selector: 'app-usuarioempresa-list',
  templateUrl: './usuarioempresa-list.component.html'
})

export class UsuarioEmpresaListComponent extends BaseListChildComponent<UsuarioEmpresa> {
  @Input("nameIdParent") _nameIdParent : string;
  @Input("idParent") _idParent: String;
  @Output() onClickAction = new EventEmitter<any>();
  titulo = 'Lista de empresa'
 
  constructor(protected store: BaseStore<UsuarioEmpresa>, 
    protected service: UsuarioEmpresaService, protected formBuilder?: FormBuilder) {
    super('UsuarioEmpresa', 'usuario', store, service, formBuilder);
    this.fieldsDate = [ 'data_dt_cadastro', 'data_dt_edicao' ]
  }

  createForm() {

    super.createForm();
    this.formFilter.addControl('situ_tx_situacao', new FormControl('A'));

  }

}