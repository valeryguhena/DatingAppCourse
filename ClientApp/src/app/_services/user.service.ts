import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../_models/User';
import { environment } from 'src/environments/environment';

// const httpOptions ={
//    headers: new HttpHeaders({
//   'Authorization':'Bearer '+ localStorage.getItem('token')
//   })
// }

@Injectable({
  providedIn: 'root'
})
export class UserService {
   baseUrl = environment.apiUrl + 'users';

constructor(private http:HttpClient) { }

getUsers():Observable<User[]>{
  return this.http.get<User[]>(this.baseUrl);
}

getUser(id:number):Observable<User>{
  return this.http.get<User>(`${this.baseUrl}/${id}`);
}

}
