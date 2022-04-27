import { Injectable } from '@angular/core';
import { BaseService } from '@framework-core/BaseService';
import { EquipeColaborador } from '@app-core/models/seguranca/equipecolaborador.model';

@Injectable({
  providedIn: 'root'
})

export class EquipeColaboradorService extends BaseService<EquipeColaborador> {

  constructor() {
    super( 'equipecolaborador');
  }

}