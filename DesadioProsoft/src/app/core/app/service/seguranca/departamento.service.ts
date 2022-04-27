import { Injectable } from '@angular/core';
import { BaseService } from '@framework-core/BaseService';
import { Departamento } from '@app-core/models/seguranca/departamento.model';

@Injectable({
  providedIn: 'root'
})

export class DepartamentoService extends BaseService<Departamento> {

  constructor() {
    super( 'departamento');
  }

}