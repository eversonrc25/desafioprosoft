import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-showerror',
  templateUrl: './showerror.component.html',
  styleUrls: ['./showerror.component.scss']
})
export class ShowerrorComponent implements OnInit {

  @Input('message') messageerror?: string;
  constructor() { }

  ngOnInit(): void {
  }

}
