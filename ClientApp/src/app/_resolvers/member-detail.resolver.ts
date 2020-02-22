import {Injectable} from "@angular/core";
import {Resolve, ActivatedRouteSnapshot, Router} from "@angular/router";
import {User} from "../_models/User";
import { Observable, of } from 'rxjs';
import { UserService } from '../_services/user.service';
import { AlertifyService } from '../_services/alertify.service';
import { catchError } from 'rxjs/operators';

@Injectable()
export class MemberDetailResolver implements Resolve<User>{
  constructor(private userService:UserService, private alertiy:AlertifyService, private router:Router) {
  }
  resolve(route: ActivatedRouteSnapshot): Observable<User> {
   return this.userService.getUser(+route.params['id'])
      .pipe(
        catchError(err => {
          this.alertiy.error(err);
          this.router.navigate(['/members']);
          return of(null)
        })
      )
  }

}
