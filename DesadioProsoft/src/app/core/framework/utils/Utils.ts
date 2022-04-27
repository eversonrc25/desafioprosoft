 
import { Router } from '@angular/router';
import { Observable } from 'rxjs';

export default class Utils  {


  static setObserver<T>(value: any) {
    return new Observable<T>(observer => {
      observer.next(value);
      observer.complete();
    });
  }

  static redirectTo(router:Router){
    let uri:string = router.url
    //window.location.reload()
    router.navigateByUrl('/', {skipLocationChange: true}).then(()=>
    router.navigate([uri]));

 }
}
