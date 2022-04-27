import { Component } from '@angular/core'
import { FormBuilder, FormControl } from '@angular/forms';

import { BaseListComponent } from '@framework-core/BaseListComponent';
import { Dev } from '@app-core/models/Dev/Dev.model';
import { BaseStore } from '@framework-core/utils/BaseStore';

import { catchError, retry, tap } from 'rxjs/operators';
import { Log_Acao } from '@app-core/models/seguranca/logacao.model';
import { LogAcaoService } from '@app-core/service/seguranca/logacao.service';
import { Usuario } from '@app-core/models/seguranca/usuario.model';
import { SistemaFuncionalidadeService } from '@app-core/service/seguranca/sistemafuncionalidade.service';
import { SistemaFuncionalidade } from '@app-core/models/seguranca/sistemafuncionalidade.model';

@Component({
  selector: 'app-logacao-list',
  templateUrl: './logacao-list.component.html'
})

export class LogacaoListComponent extends BaseListComponent<Log_Acao> {


  listaFuncionalidade: any[] = [
    { id: 'Dev', value:'Dev'}
   

  ];
  loac_nr_sequencia: String = "";
  colunas: string[] = [];
  listaUsuario: Usuario[] = [];
  soma = true;
  situ_tx_situacao = '';
  titulo = 'Log de Ações'
  totais: any = {
    'I': { valor: 0 },
    'T': { valor: 0 },
    'P': { valor: 0 },
    'V': { valor: 0 }
  };;
  Role = { Create: 'SIST_C', Update: 'SIST_U', Delete: 'SIST_D', Read: 'SIST_R' }
  acoes = [{ loac_txacao: '', tx_situacao: 'Todos' },
  { loac_txacao: 'create', tx_situacao: 'Criação' },
  { loac_txacao: 'update', tx_situacao: 'Atualização' },
  { loac_txacao: 'delete', tx_situacao: 'Deleção' }];

  constructor(protected store: BaseStore<Log_Acao>, protected service: LogAcaoService,
    protected formBuilder: FormBuilder, protected serviceFuncionalidade: SistemaFuncionalidadeService) {
    super(store, service, formBuilder);
    this.fieldsDate = ['data_dt_cadastro']

    this.dataPage.pageSize = 10000;


  }

  ngOnInit() {
    super.ngOnInit();
    this.carregarCombo()
  }

  createForm() {
    super.createForm();
    this.formFilter.addControl('loac_tx_funcionalidade', new FormControl(this.situ_tx_situacao));
    this.formFilter.addControl('loac_txacao', new FormControl(null, []));
    this.formFilter.addControl('loac_txdados', new FormControl(null, []));
    this.formFilter.addControl('usua_nr_cadastro', new FormControl(null, []));
    this.formFilter.addControl('data_dt_cadastro', new FormControl(null, []));

  }

  carregarCombo() {
    this.service.getListComboUsuario({}, 1, 1000).subscribe(retorno => {
      this.listaUsuario = retorno.dados
    })


    

    // this.serviceFuncionalidade.getList({}, 1, 1000).subscribe(retorno => {
    //   this.listaFuncionalidade = retorno.dados
    // })

  }



  trataColunas(json) {
    const jsonObject = JSON.parse(json);
    var arrayRetorno = [];
    Object.keys(jsonObject).map(function (k) {
      if (jsonObject[k])
        arrayRetorno.push({ id: k, value: jsonObject[k] });

    });

    return arrayRetorno;
  }




}