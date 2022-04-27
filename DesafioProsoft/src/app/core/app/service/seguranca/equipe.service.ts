import { Injectable } from '@angular/core';
import { BaseService } from '@framework-core/BaseService';
import { Equipe } from '@app-core/models/seguranca/equipe.model';

@Injectable({
  providedIn: 'root'
})

export class EquipeService extends BaseService<Equipe> {

  constructor() {
    super( 'equipe');
  }

}