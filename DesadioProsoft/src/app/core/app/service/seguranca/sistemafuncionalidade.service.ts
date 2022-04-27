import { Injectable } from '@angular/core';
import { BaseService } from '@framework-core/BaseService';
import { SistemaFuncionalidade } from '@app-core/models/seguranca/sistemafuncionalidade.model';

@Injectable({
  providedIn: 'root'
})

export class SistemaFuncionalidadeService extends BaseService<SistemaFuncionalidade> {

  constructor() {
    super( 'sistemafuncionalidade');
    this.baseURL = `${this.baseURL}/auth`;
  }

}