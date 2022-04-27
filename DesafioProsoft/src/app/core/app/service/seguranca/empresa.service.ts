import { Injectable } from '@angular/core';
import { Empresa } from '@app-core/models/seguranca/empresa.model';
import { BaseService } from '@framework-core/BaseService';

@Injectable({
  providedIn: 'root'
})

export class EmpresaService extends BaseService<Empresa> {
  
  //public moduloURL = 'auth';
  constructor() {
    super( 'empresa');
    this.baseURL = `${this.baseURL}/auth`;
     
  }

}