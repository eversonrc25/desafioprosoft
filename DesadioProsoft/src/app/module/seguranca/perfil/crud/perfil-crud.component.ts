import { Component } from '@angular/core'
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormControl, Validators } from '@angular/forms';

import { BaseDetailComponent } from '@framework-core/BaseDetailComponent';
import { BaseStore } from '@framework-core/utils/BaseStore';

import { Perfil } from '@app-core/models/seguranca/perfil.model';
import { PerfilService } from '@app-core/service/seguranca/perfil.service';
import { Observable } from 'rxjs';
import { RetornoApi } from '@framework-core/models/RetornoApi';
import { PerfilSistema } from '@app-core/models/seguranca/acoesperfil';
import { PerfilFuncionalidadeService } from '@app-core/service/seguranca/perfilfuncionalidade.service';
import { retry, catchError } from 'rxjs/operators';

@Component({
  selector: 'app-perfil-crud',
  templateUrl: './perfil-crud.component.html'
})
export class PerfilCrudComponent extends BaseDetailComponent<Perfil>  {

  titulo = 'Cadastro de Perfil'
  acoesDictionary = {};

  dadosSistema$: Observable<RetornoApi<PerfilSistema[]>>;
  constructor(protected activatedRoute: ActivatedRoute,
    protected router: Router,
    protected store: BaseStore<Perfil>,
    protected service: PerfilService, 
    protected serviceFuncionalidade : PerfilFuncionalidadeService,
    protected formBuilder: FormBuilder) {
    super(activatedRoute, router, store, service, formBuilder);
    this.dataPage.name = 'Perfil';
    this.fieldsDate = [ 'data_dt_cadastro', 'data_dt_edicao' ]
  }

  getId(): string {
    return 'perf_nr_sequencia'
  }

  onPosActivateRoute():void {
    //getListaSistemas(model: PerfilFuncionalidade, idParent: any, routeParent: String
      this.dadosSistema$ = this.serviceFuncionalidade.getListaSistemas(  this.dictID["id"],   'Perfil' ).pipe(
        //tap(record => this.setFormDetail(record.dados)),
        retry(0),
        catchError(
          (e) => this.handleError(e)));
    
  }

  createForm() {

    super.createForm();
    // this.formDetail.addControl('situ_tx_situacao', new FormControl({ value: 'A', disabled: this.readOnly },  [Validators.required]));

    this.formDetail.addControl('perf_tx_descricao', new FormControl({ value: null, disabled: this.readOnly },
      [Validators.required, Validators.maxLength(80)]));
    this.formDetail.addControl('situ_tx_situacao', new FormControl({ value: 'A', disabled: this.readOnly },  [Validators.required]));
    // this.formDetail.addControl('usua_nr_cadastro', new FormControl('0x5F23177F5992E20038114A28', []));
    // this.formDetail.addControl('usua_nr_edicao', new FormControl('0x5F23177F5992E20038114A28', []));

    this.listValidation = {
      perf_tx_descricao: { required: 'Descrição deve ser informada', maxlength : 'Descrição não pode ser maior que 80 caracteres' },
      situ_tx_situacao: { required: 'Situação deve ser informada' }
    };

  }

  clickFuncionalidadeAcao( acao, funcionalidade, sistema ) {
    
   
    acao.ehAssociado = ( acao.ehAssociado == 'S' ? 'N' : 'S' );
     
    let acaoBanco  = (  acao.perf_nr_sequencia === "" ? 'C' : ( acao.ehAssociado == 'S' ? 'U' : 'D' )  );
    
      this.acoesDictionary[acao.fuac_nr_sequencia] = { acaobanco : acaoBanco, perf_nr_sequencia: this.dictID["id"], fuac_nr_sequencia: acao.fuac_nr_sequencia, sist_nr_sequencia: sistema.sist_nr_sequencia, func_nr_sequencia: funcionalidade.func_nr_sequencia ,
      pefu_nr_sequencia: acao.pefu_nr_sequencia  };
 
      if ( ( acaoBanco == 'C' ) && ( acao.ehAssociado == 'N')  )
         delete this.acoesDictionary[acao.fuac_nr_sequencia];
   
   

  }
 


  onPreFormSubmit(formDetail: any): any {

    formDetail.AcoesPerfil = [];
    for (let key in this.acoesDictionary) {
       formDetail.AcoesPerfil.push( {...this.acoesDictionary[key]} )
    }
    return formDetail;
  }
  

}
