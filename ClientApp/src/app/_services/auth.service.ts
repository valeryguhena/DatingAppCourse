import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import{map} from 'rxjs/operators'
import{JwtHelperService} from '@auth0/angular-jwt'
import { User } from '../_models/User';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
baseUrl = 'http://localhost:5000/api/auth/';
jwtHelper = new JwtHelperService();
decodedToken:any;
currentUser:User;
photoUrl = new BehaviorSubject<string>('../../assets/user.png');
currentPhotoUrl = this.photoUrl.asObservable();

  constructor(private http: HttpClient) { }

  ChangeUserPhotoUrl(photoUrl:string){
    this.photoUrl.next(photoUrl);

  }

  login(userToLogin:User){
    return this.http.post(this.baseUrl + 'login', userToLogin)
    .pipe(
      map((response:any)=>{
        const user = response;
        localStorage.setItem('token', user.token);
        localStorage.setItem('user', JSON.stringify(user.user));
        this.decodedToken = this.jwtHelper.decodeToken(user.token);
        this.currentUser = user.user;
        this.ChangeUserPhotoUrl(this.currentUser.photoUrl);
      })
    )
  }
  register(user:User){
   return this.http.post(this.baseUrl+'register', user);
  }

  loggedIn(){
    const token = localStorage.getItem('token');
    return !this.jwtHelper.isTokenExpired(token);
  }
}
