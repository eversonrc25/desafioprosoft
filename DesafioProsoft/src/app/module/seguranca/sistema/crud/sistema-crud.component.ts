import { Component } from '@angular/core'
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormControl, Validators } from '@angular/forms';

import { BaseDetailComponent } from '@framework-core/BaseDetailComponent';
import { BaseStore } from '@framework-core/utils/BaseStore';

import { Sistema } from '@app-core/models/seguranca/sistema.model';
import { SistemaService } from '@app-core/service/seguranca/sistema.service';

@Component({
  selector: 'app-sistema-crud',
  templateUrl: './sistema-crud.component.html'
})
export class SistemaCrudComponent extends BaseDetailComponent<Sistema>  {

  titulo = 'Sistema'

  constructor(protected activatedRoute: ActivatedRoute,
    protected router: Router,
    protected store: BaseStore<Sistema>,
    protected service: SistemaService,
    protected formBuilder: FormBuilder ) {
    super(activatedRoute, router, store, service, formBuilder);
    this.dataPage.name = 'Sistema';
    this.fieldsDate = [ 'data_dt_cadastro', 'data_dt_edicao' ]
  }

  getId(): string {
    return 'sist_nr_sequencia'
  }

  createForm() {

    super.createForm();
    // this.formDetail.addControl('situ_tx_situacao', new FormControl({ value: 'A', disabled: this.readOnly },  [Validators.required]));

    this.formDetail.addControl('sist_tx_descricao', new FormControl({ value: null, disabled: this.readOnly },  
      [Validators.required, Validators.maxLength(100)]));
    this.formDetail.addControl('situ_tx_situacao', new FormControl({ value: 'A', disabled: this.readOnly },  [Validators.required]));
 

    this.listValidation = {
      sist_tx_descricao: { required: 'Descrição deve ser informada', maxlength : 'Descrição não pode ser maior que 80 caracteres' },
      situ_tx_situacao: { required: 'Situação deve ser informada' }
    };

  }

}
