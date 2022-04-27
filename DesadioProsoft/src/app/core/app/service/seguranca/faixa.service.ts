import { Injectable } from '@angular/core';
import { BaseService } from '@framework-core/BaseService';
import { Faixa } from '@app-core/models/seguranca/faixa.model';

@Injectable({
  providedIn: 'root'
})

export class FaixaService extends BaseService<Faixa> {

  constructor() {
    super( 'faixa');
  }

}