import { Injectable } from '@angular/core';
import { BaseService } from '@framework-core/BaseService';
import { UsuarioPerfil } from '@app-core/models/seguranca/usuarioperfil.model';

@Injectable({
  providedIn: 'root'
})

export class UsuarioPerfilService extends BaseService<UsuarioPerfil> {

  constructor() {
    super( 'usuarioperfil');
    this.baseURL = `${this.baseURL}/auth`;
  }

}