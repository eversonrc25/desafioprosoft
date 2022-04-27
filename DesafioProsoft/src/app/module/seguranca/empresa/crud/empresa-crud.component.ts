import { Component } from '@angular/core'
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormControl, Validators } from '@angular/forms';

import { BaseDetailComponent } from '@framework-core/BaseDetailComponent';
import { BaseStore } from '@framework-core/utils/BaseStore';

import { Empresa } from '@app-core/models/seguranca/empresa.model';
import { EmpresaService } from '@app-core/service/seguranca/empresa.service';

@Component({
  selector: 'app-empresa-crud',
  templateUrl: './empresa-crud.component.html'
})
export class EmpresaCrudComponent extends BaseDetailComponent<Empresa>  {

  titulo = 'Cadastro de Empresa'


  constructor(protected activatedRoute: ActivatedRoute,
    protected router: Router,
    protected store: BaseStore<Empresa>,
    protected service: EmpresaService,
    protected formBuilder: FormBuilder) {
    super(activatedRoute, router, store, service, formBuilder);
    this.dataPage.name = 'Empresa';
    this.fieldsDate = ['data_dt_cadastro', 'data_dt_edicao']
  }

  getId(): string {
    return 'empr_nr_sequencia';
  }

  createForm() {

    super.createForm();
    // this.formDetail.addControl('situ_tx_situacao', new FormControl({ value: 'A', disabled: this.readOnly },  [Validators.required]));

    this.formDetail.addControl('empr_nr_sequencia', new FormControl(null, []));
    this.formDetail.addControl('empr_tx_nome', new FormControl({ value: null, disabled: this.readOnly },
      [Validators.required, Validators.maxLength(100)]));
    this.formDetail.addControl('empr_tx_razao_social', new FormControl({ value: null, disabled: this.readOnly },
      [Validators.required, Validators.maxLength(100)]));
    this.formDetail.addControl('empr_tx_cnpj', new FormControl({ value: null, disabled: this.readOnly },
      [Validators.required, Validators.maxLength(14)]));
    this.formDetail.addControl('empr_tx_logradouro', new FormControl({ value: null, disabled: this.readOnly },
      [Validators.required, Validators.maxLength(80)]));
    this.formDetail.addControl('empr_tx_numero', new FormControl({ value: null, disabled: this.readOnly },
      [Validators.required, Validators.maxLength(15)]));
    this.formDetail.addControl('empr_tx_bairro', new FormControl({ value: null, disabled: this.readOnly },
      [Validators.required, Validators.maxLength(50)]));
    this.formDetail.addControl('empr_tx_municipio', new FormControl({ value: null, disabled: this.readOnly },
      [Validators.required, Validators.maxLength(80)]));
    this.formDetail.addControl('empr_tx_uf', new FormControl({ value: null, disabled: this.readOnly },
      [Validators.required, Validators.maxLength(2)]));
    this.formDetail.addControl('empr_tx_cep', new FormControl({ value: null, disabled: this.readOnly },
      [Validators.required, Validators.maxLength(9)]));
    this.formDetail.addControl('empr_tx_telefone', new FormControl({ value: null, disabled: this.readOnly },
      [Validators.maxLength(20)]));
    this.formDetail.addControl('empr_tx_email', new FormControl({ value: null, disabled: this.readOnly },
      [Validators.required, Validators.maxLength(100)]));
    // this.formDetail.addControl('empr_tx_bd', new FormControl({ value: null, disabled: this.readOnly }, []));
    // this.formDetail.addControl('empr_tx_esquema', new FormControl({ value: null, disabled: this.readOnly }, []));
    // this.formDetail.addControl('empr_tx_senha', new FormControl({ value: null, disabled: this.readOnly }, []));
    this.formDetail.addControl('situ_tx_situacao', new FormControl({ value: 'A', disabled: this.readOnly }, [Validators.required]));
    // this.formDetail.addControl('usua_nr_cadastro', new FormControl('0x5F23177F5992E20038114A28', []));
    // this.formDetail.addControl('usua_nr_edicao', new FormControl('0x5F23177F5992E20038114A28', []));
    // this.formDetail.addControl('data_dt_cadastro', new FormControl(null, []));
    // this.formDetail.addControl('data_dt_edicao', new FormControl(null, []));

    this.listValidation = {
      empr_tx_nome: { required: 'Nome deve ser informado', maxlength: 'Nome não pode ser maior que 100 caracteres' },
      empr_tx_razao_social: { required: 'Razão Social deve ser informada', 
      maxlength: 'Razão Social não pode ser maior que 100 caracteres' },
      empr_tx_cnpj: { required: 'CNPJ deve ser informado', maxlength: 'CNPJ não pode ser maior que 14 caracteres' },
      empr_tx_logradouro: { required: 'Logradouro deve ser informado', maxlength: 'Logradouro não pode ser maior que 80 caracteres' },
      empr_tx_numero: { required: 'Número deve ser informado', maxlength: 'Número não pode ser maior que 15 caracteres' },
      empr_tx_bairro: { required: 'Bairro deve ser informado', maxlength: 'Bairro não pode ser maior que 50 caracteres' },
      empr_tx_municipio: { required: 'Município deve ser informado', maxlength: 'Município não pode ser maior que 80 caracteres' },
      empr_tx_uf: { required: 'UF deve ser informado', maxlength: 'UF não pode ser maior que 2 caracteres' },
      empr_tx_cep: { required: 'CEP deve ser informado', maxlength: 'CEP não pode ser maior que 9 caracteres' },
      empr_tx_telefone: { maxlength: 'Nome não pode ser maior que 20 caracteres' },
      empr_tx_email: { required: 'E-mail deve ser informado', maxlength: 'E-mail não pode ser maior que 100 caracteres' },
      situ_tx_situacao: { required: 'Situação deve ser informada' }
    };

  }

}
