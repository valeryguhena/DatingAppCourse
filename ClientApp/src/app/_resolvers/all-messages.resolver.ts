import {ActivatedRouteSnapshot, Resolve, Router} from "@angular/router";
import {Message} from "../_models/message";
import {UserService} from "../_services/user.service";
import {AuthService} from "../_services/auth.service";
import {AlertifyService} from "../_services/alertify.service";
import {Observable, of} from "rxjs";
import {catchError} from "rxjs/operators";

export class AllMessagesResolver implements Resolve<Message[]>{
  pageNumber =1;
  pageSize = 5;
  messageContainer = 'Unread';

  constructor(private userService:UserService, private authService:AuthService,
              private alertify:AlertifyService, private router:Router) {  }
  resolve(route: ActivatedRouteSnapshot): Observable<Message[]>  {
    return this.userService.getMessages(this.authService.decodedToken.nameid, this.pageNumber,
      this.pageSize, this.messageContainer)
      .pipe(catchError(err=>{
        this.alertify.error("Could not load messages");
        this.router.navigate(['/home']);
        return of(null);
      }));
  }

}
