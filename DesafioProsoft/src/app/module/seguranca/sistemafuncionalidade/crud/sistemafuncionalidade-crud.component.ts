import { Component, OnInit } from '@angular/core'
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormControl, Validators } from '@angular/forms';

import { BaseDetailComponent } from '@framework-core/BaseDetailComponent';
import { BaseStore } from '@framework-core/utils/BaseStore';

import { SistemaFuncionalidade } from '@app-core/models/seguranca/sistemafuncionalidade.model';
import { SistemaFuncionalidadeService } from '@app-core/service/seguranca/sistemafuncionalidade.service'

import { Sistema } from '@app-core/models/seguranca/sistema.model'
import { SistemaService } from '@app-core/service/seguranca/sistema.service'

@Component({
  selector: 'app-sistemafuncionalidade-crud',
  templateUrl: './sistemafuncionalidade-crud.component.html'
})
export class SistemaFuncionalidadeCrudComponent extends BaseDetailComponent<SistemaFuncionalidade> implements OnInit  {

  titulo = 'Funcionalidade'
  listaSistema: Sistema[] = []

  constructor(protected activatedRoute: ActivatedRoute,
              protected router: Router,
              protected store: BaseStore<SistemaFuncionalidade>,
              protected service: SistemaFuncionalidadeService,
              protected serviceSistema: SistemaService,
              protected formBuilder?: FormBuilder) {
    super(activatedRoute, router, store, service, formBuilder);
    this.dataPage.name = 'SistemaFuncionalidade';
    this.fieldsDate = [ 'data_dt_cadastro', 'data_dt_edicao' ]
  }

  getId(): string {
    return 'func_nr_sequencia';
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
    // this.formDetail.addControl('situ_tx_situacao', new FormControl({ value: 'A', disabled: this.readOnly },  [Validators.required]));

    this.formDetail.addControl('sist_nr_sequencia', new FormControl({ value: null, disabled: this.readOnly },  [Validators.required]));
    this.formDetail.addControl('func_tx_descricao', new FormControl({ value: null, disabled: this.readOnly },  
      [Validators.required, Validators.maxLength(100)]));
    this.formDetail.addControl('func_tx_nome', new FormControl({ value: null, disabled: this.readOnly }, 
      [Validators.required, Validators.maxLength(60)]));
    this.formDetail.addControl('func_tx_url', new FormControl({ value: null, disabled: this.readOnly },  
      [Validators.required, Validators.maxLength(80)]));
    this.formDetail.addControl('situ_tx_situacao', new FormControl({ value: 'A', disabled: this.readOnly },  [Validators.required]));
     
    this.listValidation = {
      sist_nr_sequencia: { required: 'Sistema deve ser informado' },
      func_tx_nome: { required: 'Nome deve ser informado', maxlength : 'Nome não pode ser maior que 60 caracteres' },
      func_tx_descricao: { required: 'Descrição deve ser informada', maxlength : 'Descrição não pode ser maior que 100 caracteres' },
      func_tx_url: { required: 'URL deve ser informada', maxlength : 'URL não pode ser maior que 80 caracteres' },
      situ_tx_situacao: { required: 'Situação deve ser informada' },

    };

  }

}
