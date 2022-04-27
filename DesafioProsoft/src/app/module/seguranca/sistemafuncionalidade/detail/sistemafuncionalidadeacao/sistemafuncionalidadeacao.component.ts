import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core'

@Component({
  selector: 'app-sistemafuncionalidadeacao',
  templateUrl: './sistemafuncionalidadeacao.component.html'
})

export class SistemaFuncionalidadeAcaoComponent implements OnInit  {

  action : string = null;
  idChild : string = null;
  @Input("nameIdParent") _nameIdParent : String;
  @Input("idParent") _idParent : String;
  @Output() onClickAction = new EventEmitter<any>();


  ngOnInit(): void { }

  clickAction( event: { action: string, idChild?:string} ) {
    this.idChild = event.idChild;
     this.action = event.action;

  }

}
