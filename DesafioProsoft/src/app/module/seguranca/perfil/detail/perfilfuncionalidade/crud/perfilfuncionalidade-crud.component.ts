import { Component, Input, Output, EventEmitter } from '@angular/core'
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormControl, Validators } from '@angular/forms';

import { BaseDetailChildComponent } from '@framework-core/BaseDetailChildComponent';
import { BaseStore } from '@framework-core/utils/BaseStore';

import { PerfilFuncionalidade } from '@app-core/models/seguranca/perfilfuncionalidade.model';
import { PerfilFuncionalidadeService } from '@app-core/service/seguranca/perfilfuncionalidade.service';

@Component({
  selector: 'app-perfilfuncionalidade-crud',
  templateUrl: './perfilfuncionalidade-crud.component.html'
})
export class PerfilFuncionalidadeCrudComponent extends BaseDetailChildComponent<PerfilFuncionalidade>  {

  titulo = 'Associar funcionalidade'

  @Input("nameIdParent") _nameIdParent: string;
  @Input("idParent") _idParent: string;
  @Input("idChild") _idChild: string;
  @Input("action") _action: string;
  @Output() onClickAction = new EventEmitter<any>();
  /************************ Area temporaria subistiuir por query  */
  cbfunc_nr_sequencia = [{ func_nr_sequencia: '', tx_func_nr_sequencia: 'Selecione' },
  { func_nr_sequencia: '0x5F20F86BD626D000365FD1D9', tx_func_nr_sequencia: 'Opcao 1' },
  { func_nr_sequencia: '0x5F1A5135D626D0003FF509C9', tx_func_nr_sequencia: 'Opcao 2' }];

  constructor(protected activatedRoute: ActivatedRoute,
    protected router: Router,
    protected store: BaseStore<PerfilFuncionalidade>,
    protected service: PerfilFuncionalidadeService,
    protected formBuilder?: FormBuilder) {
    super('PerfilFuncionalidade', 'perfil', activatedRoute, router, store, service);
    this.dataPage.name = 'PerfilFuncionalidade';
    this.fieldsDate = [ 'data_dt_cadastro', 'data_dt_edicao' ]
  }

  getId(): string {
    return 'pefu_nr_sequencia';
  }

  createForm() {

    super.createForm();
    // this.formDetail.addControl('situ_tx_situacao', new FormControl({ value: 'A', disabled: this.readOnly },  [Validators.required]));

    this.formDetail.addControl('func_nr_sequencia', new FormControl(null, []));
    this.formDetail.addControl('perf_nr_sequencia', new FormControl(null, []));
    this.formDetail.addControl('situ_tx_situacao', new FormControl(null, []));
    // this.formDetail.addControl('usua_nr_cadastro', new FormControl(null, []));
    // this.formDetail.addControl('usua_nr_edicao', new FormControl(null, []));

    this.listValidation = {
                                                      
    };

  }

}
