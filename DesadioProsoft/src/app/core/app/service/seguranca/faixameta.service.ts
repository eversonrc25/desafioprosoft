import { Injectable } from '@angular/core';
import { BaseService } from '@framework-core/BaseService';
import { FaixaMeta } from '@app-core/models/seguranca/faixameta.model';

@Injectable({
  providedIn: 'root'
})

export class FaixaMetaService extends BaseService<FaixaMeta> {

  constructor() {
    super( 'faixameta');
  }

}