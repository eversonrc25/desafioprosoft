import { Injectable } from '@angular/core';
import { BaseService } from '@framework-core/BaseService';
import { Variavel } from '@app-core/models/seguranca/variavel.model';

@Injectable({
  providedIn: 'root'
})

export class VariavelService extends BaseService<Variavel> {

  constructor() {
    super( 'variavel');
  }

}