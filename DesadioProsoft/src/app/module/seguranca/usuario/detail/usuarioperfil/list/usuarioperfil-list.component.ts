import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core'
import { FormBuilder, FormControl } from '@angular/forms';
import { BaseListChildComponent } from '@framework-core/BaseListChildComponent';
import { UsuarioPerfilService } from '@app-core/service/seguranca/usuarioperfil.service';
import { UsuarioPerfil } from '@app-core/models/seguranca/usuarioperfil.model';
import { BaseStore } from '@framework-core/utils/BaseStore';

import { Perfil } from '@app-core/models/seguranca/perfil.model'
import { PerfilService } from '@app-core/service/seguranca/perfil.service'

@Component({
  selector: 'app-usuarioperfil-list',
  templateUrl: './usuarioperfil-list.component.html'
})

export class UsuarioPerfilListComponent extends BaseListChildComponent<UsuarioPerfil> implements OnInit {

  titulo = 'Lista de perfil'
  listaPerfil: Perfil[] = []
  @Input("nameIdParent") _nameIdParent : string;
  @Input("idParent") _idParent: String;
  @Output() onClickAction = new EventEmitter<any>();
  constructor(protected store: BaseStore<UsuarioPerfil>, protected service: UsuarioPerfilService,
              protected servicePerfil: PerfilService,
              protected formBuilder?: FormBuilder) {
    super('UsuarioPerfil', 'usuario', store, service, formBuilder);
    this.fieldsDate = [ 'data_dt_cadastro', 'data_dt_edicao' ]
  }

  carregarCombo() {
    this.servicePerfil.getList({ }, 1, 1000 ).subscribe(retorno => { this.listaPerfil = retorno.dados })

  }

  ngOnInit() { 
    super.ngOnInit()
    this.carregarCombo()
  }

  createForm() {

    super.createForm();
    this.formFilter.addControl('situ_tx_situacao', new FormControl('A'));
    this.formFilter.addControl('perf_nr_sequencia', new FormControl());

  }

}