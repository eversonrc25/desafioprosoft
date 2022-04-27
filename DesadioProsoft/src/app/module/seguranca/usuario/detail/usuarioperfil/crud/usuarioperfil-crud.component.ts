import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core'
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormControl, Validators } from '@angular/forms';

import { BaseDetailChildComponent } from '@framework-core/BaseDetailChildComponent';
import { BaseStore } from '@framework-core/utils/BaseStore';

import { UsuarioPerfil } from '@app-core/models/seguranca/usuarioperfil.model';
import { UsuarioPerfilService } from '@app-core/service/seguranca/usuarioperfil.service';

import { Perfil } from '@app-core/models/seguranca/perfil.model'
import { PerfilService } from '@app-core/service/seguranca/perfil.service'

@Component({
  selector: 'app-usuarioperfil-crud',
  templateUrl: './usuarioperfil-crud.component.html'
})
export class UsuarioPerfilCrudComponent extends BaseDetailChildComponent<UsuarioPerfil> implements OnInit  {

  titulo = 'Associar pefil a usuário'
  listaPerfil: Perfil[] = [] 

  @Input("nameIdParent") _nameIdParent: string;
  @Input("idParent") _idParent: string;
  @Input("idChild") _idChild: string;
  @Input("action") _action: string;
  @Output() onClickAction = new EventEmitter<any>();
  constructor(protected activatedRoute: ActivatedRoute,
              protected router: Router,
              protected store: BaseStore<UsuarioPerfil>,
              protected service: UsuarioPerfilService,
              protected servicePerfil: PerfilService,
              protected formBuilder?: FormBuilder) {
    super('UsuarioPerfil', 'usuario', activatedRoute, router, store, service, formBuilder);
    this.dataPage.name = 'UsuarioPerfil';
    this.fieldsDate = [ 'data_dt_cadastro', 'data_dt_edicao' ]
  }

  getId(): string {
    return 'uspe_nr_sequencia'
  }

  carregarCombo() {
    this.servicePerfil.getList({ situ_tx_situacao: 'A' }, 1, 1000 ).subscribe(retorno => { this.listaPerfil = retorno.dados })
  }

  ngOnInit() {
    super.ngOnInit()
    this.carregarCombo()
  }
  createForm() {

    super.createForm();
    // this.formDetail.addControl('situ_tx_situacao', new FormControl({ value: 'A', disabled: this.readOnly },  [Validators.required]));

    this.formDetail.addControl('perf_nr_sequencia', new FormControl({ value: null, disabled: this.readOnly },  [Validators.required]));
    this.formDetail.addControl('usua_nr_sequencia', new FormControl(null, []));
    this.formDetail.addControl('situ_tx_situacao', new FormControl({ value: 'A', disabled: this.readOnly },  [Validators.required]));
    this.formDetail.addControl('usua_nr_cadastro', new FormControl('0x5F23177F5992E20038114A28', []));
    this.formDetail.addControl('usua_nr_edicao', new FormControl('0x5F23177F5992E20038114A28', []));

    this.listValidation = {
      perf_nr_sequencia: { required: 'Perfil deve ser informado' },
      situ_tx_situacao: { required: 'Situação deve ser informada' }
    };

  }

}
