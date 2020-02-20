import { Injectable } from '@angular/core';
declare let alertify:any;
@Injectable({
  providedIn: 'root'
})
export class AlertifyService {

  constructor() { }
  confirm(message:string, callBack:()=>void){
    alertify.confirm(message, (e)=>{
      if(e){
        return true
      }
      else{}
    });
  }

  success(message){
    alertify.success(message)
  }
  error(message){
    alertify.error(message)
  }
  warning(message){
    alertify.warning(message)
  }
  message(message){
    alertify.message(message)
  }
}
