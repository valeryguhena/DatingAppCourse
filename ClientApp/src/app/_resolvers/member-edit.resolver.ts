import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, Router } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';
import { AuthService } from '../_services/auth.service';
import { User } from '../_models/User';
import { Observable, of } from 'rxjs';
import { UserService } from '../_services/user.service';
import { catchError } from 'rxjs/operators';

@Injectable()
export class MemberEditResolver implements Resolve<User>{
  constructor(private authService:AuthService, private alertify:AlertifyService,
              private userService:UserService, private router:Router) {
  }
  resolve(): Observable<User>  {
    return this.userService.getUser(this.authService.decodedToken.nameid)
      .pipe(catchError(err => {
        this.alertify.error(err);
        this.router.navigate(['/member']);
        return of(null);
        })
      );
  }

}
