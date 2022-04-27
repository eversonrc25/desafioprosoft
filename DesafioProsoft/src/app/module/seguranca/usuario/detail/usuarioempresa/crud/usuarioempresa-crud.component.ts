import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core'
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormControl, Validators } from '@angular/forms';

import { BaseDetailChildComponent } from '@framework-core/BaseDetailChildComponent';
import { BaseStore } from '@framework-core/utils/BaseStore';

import { UsuarioEmpresa } from '@app-core/models/seguranca/usuarioempresa.model';
import { UsuarioEmpresaService } from '@app-core/service/seguranca/usuarioempresa.service';

import { Empresa } from '@app-core/models/seguranca/empresa.model'
import { EmpresaService } from '@app-core/service/seguranca/empresa.service'

@Component({
  selector: 'app-usuarioempresa-crud',
  templateUrl: './usuarioempresa-crud.component.html'
})
export class UsuarioEmpresaCrudComponent extends BaseDetailChildComponent<UsuarioEmpresa> implements OnInit {

  titulo = 'Associar empresa/usuário'
  listaEmpresa: Empresa[] = []
  @Input("nameIdParent") _nameIdParent: string;
  @Input("idParent") _idParent: string;
  @Input("idChild") _idChild: string;
  @Input("action") _action: string;
  @Output() onClickAction = new EventEmitter<any>();
  constructor(protected activatedRoute: ActivatedRoute,
              protected router: Router,
              protected store: BaseStore<UsuarioEmpresa>,
              protected service: UsuarioEmpresaService,
              protected serviceEmpresa: EmpresaService,
              protected formBuilder?: FormBuilder) {
    super('UsuarioEmpresa', 'usuario', activatedRoute, router, store, service, formBuilder);
    this.dataPage.name = 'UsuarioEmpresa';
    this.fieldsDate = [ 'data_dt_cadastro', 'data_dt_edicao' ]
  }

  getId(): string {
    return 'usem_nr_sequencia';
  }

  carregarCombo() {
    this.serviceEmpresa.getList({ }, 1, 1000 ).subscribe(retorno => { this.listaEmpresa = retorno.dados })

  }

  ngOnInit() {
    super.ngOnInit()
    this.carregarCombo()
  }

  createForm() {

    super.createForm();
    // this.formDetail.addControl('situ_tx_situacao', new FormControl({ value: 'A', disabled: this.readOnly },  [Validators.required]));

    this.formDetail.addControl('empr_nr_sequencia', new FormControl({ value: null, disabled: this.readOnly },  [Validators.required]));
    this.formDetail.addControl('usua_nr_sequencia', new FormControl(null, []));
    this.formDetail.addControl('situ_tx_situacao', new FormControl({ value: 'A', disabled: this.readOnly },  [Validators.required]));
    this.formDetail.addControl('usua_nr_cadastro', new FormControl('0x5F23177F5992E20038114A28', []));
    this.formDetail.addControl('usua_nr_edicao', new FormControl('0x5F23177F5992E20038114A28', []));

    this.listValidation = {
      empr_nr_sequencia: { required: 'Empresa deve ser informada' },
      situ_tx_situacao: { required: 'Situação deve ser informada' }
    };

  }

}
