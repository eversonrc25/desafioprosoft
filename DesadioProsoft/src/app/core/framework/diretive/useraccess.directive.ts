import { Directive, OnInit, Input } from '@angular/core';
import { TemplateRef, ViewContainerRef } from '@angular/core';
import { map } from 'rxjs/operators';
//import { AuthService } from '../services/auth.service';
import { LoginService} from '../auth/LoginService'

@Directive({ selector: '[appUser]'})
export class UserDirective implements OnInit {

     role: any;
    @Input()
  set appUser(context: any) {
    this.role = context;
  }


    constructor(
        private templateRef: TemplateRef<any>,
        private loginService: LoginService,
        private viewContainer: ViewContainerRef
    ) { }

    ngOnInit() {
    
        
        const hasAccess =  this.loginService.getRules().find( element => element == this.role );
        
        if (hasAccess) {
            this.viewContainer.createEmbeddedView(this.templateRef);
        } else {
            this.viewContainer.clear();
        }
    }
}