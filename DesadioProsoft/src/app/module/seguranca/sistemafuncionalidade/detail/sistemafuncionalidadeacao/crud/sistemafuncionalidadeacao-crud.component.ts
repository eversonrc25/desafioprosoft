import { Component, Input, Output, EventEmitter } from '@angular/core'
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormControl, Validators } from '@angular/forms';

import { BaseDetailChildComponent } from '@framework-core/BaseDetailChildComponent';
import { BaseStore } from '@framework-core/utils/BaseStore';

import { SistemaFuncionalidadeAcao } from '@app-core/models/seguranca/sistemafuncionalidadeacao.model';
import { SistemaFuncionalidadeAcaoService } from '@app-core/service/seguranca/sistemafuncionalidadeacao.service';

@Component({
  selector: 'app-sistemafuncionalidadeacao-crud',
  templateUrl: './sistemafuncionalidadeacao-crud.component.html'
})
export class SistemaFuncionalidadeAcaoCrudComponent extends BaseDetailChildComponent<SistemaFuncionalidadeAcao>  {

  titulo = 'Cadastro de ação'
  @Input("nameIdParent") _nameIdParent: string;
  @Input("idParent") _idParent: string;
  @Input("idChild") _idChild: string;
  @Input("action") _action: string;
  @Output() onClickAction = new EventEmitter<any>();
  constructor(protected activatedRoute: ActivatedRoute,
    protected router: Router,
    protected store: BaseStore<SistemaFuncionalidadeAcao>,
    protected service: SistemaFuncionalidadeAcaoService,
    protected formBuilder?: FormBuilder ) {
    super('SistemaFuncionalidadeAcao', 'sistemafuncionalidade', activatedRoute, router, store, service, formBuilder);
    this.dataPage.name = 'SistemaFuncionalidadeAcao';
    this.fieldsDate = ['data_dt_cadastro', 'data_dt_edicao']
  }

  getId(): string {
    return 'fuac_nr_sequencia';
  }

  createForm() {

    super.createForm();
    // this.formDetail.addControl('situ_tx_situacao', new FormControl({ value: 'A', disabled: this.readOnly },  [Validators.required]));

    this.formDetail.addControl('func_nr_sequencia', new FormControl(null, []));
    this.formDetail.addControl('fuac_tx_descricao', new FormControl({ value: null, disabled: this.readOnly },
      [Validators.required, Validators.maxLength(50)]));
    this.formDetail.addControl('fuac_tx_nome', new FormControl({ value: null, disabled: this.readOnly },
      [Validators.required, Validators.maxLength(50)]));
    this.formDetail.addControl('fuac_tx_rota', new FormControl({ value: null, disabled: this.readOnly }, 
      [Validators.required, Validators.maxLength(255)]));
    this.formDetail.addControl('fuac_tx_regra', new FormControl({ value: null, disabled: this.readOnly }, 
      [Validators.required, Validators.maxLength(255)]));
    this.formDetail.addControl('situ_tx_situacao', new FormControl({ value: 'A', disabled: this.readOnly }, [Validators.required]));
    this.formDetail.addControl('usua_nr_cadastro', new FormControl('0x5F23177F5992E20038114A28', []));
    this.formDetail.addControl('usua_nr_edicao', new FormControl('0x5F23177F5992E20038114A28', []));

    this.listValidation = {
      fuac_tx_descricao: { required: 'Descrição deve ser informada', maxlength : 'Descrição não pode ser maior que 50 caracteres' },
      fuac_tx_nome: { required: 'Nome deve ser informado', maxlength : 'Nome não pode ser maior que 50 caracteres' },
      fuac_tx_rota: { required: 'Rota deve ser informada', maxlength : 'Rota não pode ser maior que 255 caracteres' },
      fuac_tx_regra: { required: 'Regra deve ser informada' , maxlength : 'Regra não pode ser maior que 255 caracteres' },
      situ_tx_situacao: { required: 'Situação deve ser informada' },
    };

  }

}
