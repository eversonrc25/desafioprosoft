import { Injectable } from '@angular/core';
import { BaseService } from '@framework-core/BaseService';
import { SistemaFuncionalidadeAcao } from '@app-core/models/seguranca/sistemafuncionalidadeacao.model';

@Injectable({
  providedIn: 'root'
})

export class SistemaFuncionalidadeAcaoService extends BaseService<SistemaFuncionalidadeAcao> {

  constructor() {
    super( 'sistemafuncionalidadeacao');
    this.baseURL = `${this.baseURL}/auth`;
  }

}