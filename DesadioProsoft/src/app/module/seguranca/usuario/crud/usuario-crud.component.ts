import { Component } from '@angular/core'
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormControl, Validators } from '@angular/forms';

import { BaseDetailComponent } from '@framework-core/BaseDetailComponent';
import { BaseStore } from '@framework-core/utils/BaseStore';

import { Usuario } from '@app-core/models/seguranca/usuario.model';
import { UsuarioService } from '@app-core/service/seguranca/usuario.service';

@Component({
  selector: 'app-usuario-crud',
  templateUrl: './usuario-crud.component.html'
})
export class UsuarioCrudComponent extends BaseDetailComponent<Usuario>  {

  titulo = 'Cadastro de Usuário'

  notifica = [{ usua_tx_notifica: '', tx_notifica: 'Selecione' },
  { usua_tx_notifica: 'S', tx_notifica: 'Sim' },
  { usua_tx_notifica: 'N', tx_notifica: 'Não' }];


  constructor(protected activatedRoute: ActivatedRoute,
    protected router: Router,
    protected store: BaseStore<Usuario>,
    protected service: UsuarioService,
    protected formBuilder?: FormBuilder
    ) {
    super(activatedRoute, router, store, service, formBuilder);
    this.dataPage.name = 'Usuario';
    this.fieldsDate = [ 'data_dt_cadastro', 'data_dt_edicao' ]
  }

  getId(): string {
    return 'usua_nr_sequencia';
  }

  createForm() {

    super.createForm();
    // this.formDetail.addControl('situ_tx_situacao', new FormControl({ value: 'A', disabled: this.readOnly },  [Validators.required]));

    this.formDetail.addControl('usua_tx_nome', new FormControl({ value: null, disabled: this.readOnly }, 
                                                               [Validators.required, Validators.maxLength(80)]));
    this.formDetail.addControl('usua_tx_apelido', new FormControl({ value: null, disabled: this.readOnly }, 
                                                              [Validators.required, Validators.maxLength(40)]));
    this.formDetail.addControl('usua_tx_email', new FormControl({ value: null, disabled: this.readOnly }, 
                                                              [Validators.required, Validators.maxLength(100)]));
    this.formDetail.addControl('usua_tx_telefone', new FormControl({ value: null, disabled: this.readOnly },  
                                                              [Validators.maxLength(20)]));
    this.formDetail.addControl('situ_tx_situacao', new FormControl({ value: 'A', disabled: this.readOnly }, [Validators.required]));
    this.formDetail.addControl('usua_tx_notifica', new FormControl({ value: '', disabled: this.readOnly }, [Validators.required]));
   
    this.formDetail.addControl('usua_nr_cadastro', new FormControl('0x5F23177F5992E20038114A28', []));
    this.formDetail.addControl('usua_nr_edicao', new FormControl('0x5F23177F5992E20038114A28', []));
    this.formDetail.addControl('usua_tx_ad',new FormControl({ value: null, disabled: this.readOnly }, 
      [ Validators.maxLength(100)]));

    this.listValidation = {
      usua_tx_nome: { required: 'Nome deve ser informado', maxlength : 'Nome não pode ser maior que 80 caracteres' },
      usua_tx_apelido: { required: 'Apelido deve ser informado', maxlength : 'Apelido não pode ser maior que 40 caracteres' },
      usua_tx_email: { required: 'E-mail deve ser informado', maxlength : 'Email não pode ser maior que 100 caracteres' },
      usua_tx_telefone: {  maxlength : 'Telefone não pode ser maior que 20 caracteres' },
      situ_tx_situacao: { required: 'Situação deve ser informada' },
      usua_tx_notifica: { required: 'Notifica deve ser informado' },
      usua_tx_ad: { maxlength: 'Usuário AD não pode ser maior que 100 caracteres' }
    };

  }

}
