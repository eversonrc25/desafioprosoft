import { Injectable } from '@angular/core';
import { BaseService } from '@framework-core/BaseService';

import { Dev } from '@app-core/models/dev/dev.model';
import { Observable } from 'rxjs';
import { RetornoApi } from '@framework-core/models/RetornoApi';
import { catchError, retry } from 'rxjs/operators';


@Injectable({
  providedIn: 'root'
})

export class DevService extends BaseService<Dev> {

  constructor() {
    super('dev');
    this.baseURL = `${this.baseURL}/centralizador`;
  }




}