import { Injectable } from '@angular/core';
import { BaseService } from '@framework-core/BaseService';
import { VariavelFormula } from '@app-core/models/seguranca/variavelformula.model';

@Injectable({
  providedIn: 'root'
})

export class VariavelFormulaService extends BaseService<VariavelFormula> {

  constructor() {
    super( 'variavelformula');
  }

}