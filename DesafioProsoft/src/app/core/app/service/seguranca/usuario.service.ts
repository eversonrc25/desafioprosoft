import { Injectable } from '@angular/core';
import { BaseService } from '@framework-core/BaseService';
import { Usuario } from '@app-core/models/seguranca/usuario.model';

@Injectable({
  providedIn: 'root'
})

export class UsuarioService extends BaseService<Usuario> {

  constructor() {
    super( 'usuario');
    this.baseURL = `${this.baseURL}/auth`;
  }

}