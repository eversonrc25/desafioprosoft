import { Injectable } from '@angular/core';
import { BaseService } from '@framework-core/BaseService';
import { Perfil } from '@app-core/models/seguranca/perfil.model';

@Injectable({
  providedIn: 'root'
})

export class PerfilService extends BaseService<Perfil> {

  constructor() {
    super( 'perfil');
    this.baseURL = `${this.baseURL}/auth`;
  }

}