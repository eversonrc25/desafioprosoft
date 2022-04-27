import { Injectable } from '@angular/core';
import { BaseService } from '@framework-core/BaseService';
import { FaixaParametro } from '@app-core/models/seguranca/faixaparametro.model';

@Injectable({
  providedIn: 'root'
})

export class FaixaParametroService extends BaseService<FaixaParametro> {

  constructor() {
    super( 'faixaparametro');
  }

}