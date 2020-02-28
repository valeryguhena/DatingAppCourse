import { Injectable } from '@angular/core';
import { User } from '../_models/User';
import { Resolve, ActivatedRouteSnapshot, Router } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';
import { UserService } from '../_services/user.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()
export class ListsResolver implements Resolve<User[]>{
  constructor(private userService:UserService, private router:Router, private alertify:AlertifyService) {
  }
  pageNumber =1
  pageSize = 5
  likeParams = "likers";
  resolve(route: ActivatedRouteSnapshot): Observable<User[]> {
    return this.userService.getUsers(this.pageNumber, this.pageSize, null, this.likeParams)
      .pipe(
        catchError(err=>{
          this.alertify.error('Cannot load users');
          this.router.navigate(['/home']);
          return of(null)
        })
      );
  }

}
