import { Injectable } from '@angular/core';
import { BaseService } from '@framework-core/BaseService';
import { VariavelLista } from '@app-core/models/seguranca/variavellista.model';

@Injectable({
  providedIn: 'root'
})

export class VariavelListaService extends BaseService<VariavelLista> {

  constructor() {
    super( 'variavellista');
  }

}