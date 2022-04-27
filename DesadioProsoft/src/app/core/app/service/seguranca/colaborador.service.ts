import { Injectable } from '@angular/core';
import { BaseService } from '@framework-core/BaseService';
import { Colaborador } from '@app-core/models/seguranca/colaborador.model';

@Injectable({
  providedIn: 'root'
})

export class ColaboradorService extends BaseService<Colaborador> {

  constructor() {
    super( 'colaborador');
  }

}