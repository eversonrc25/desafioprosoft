import { HttpErrorResponse } from '@angular/common/http';

export class BaseState<T> {
  nameComponent?: string ='';
  action?: string  = 'list';
  pagina?: number = 1;
  error?: HttpErrorResponse = null; // NEW
  selectedItem?: T = null; // Projeto selecionado
  filterForm?: T = null; // Projeto selecionado
  id?: any = null;
}
