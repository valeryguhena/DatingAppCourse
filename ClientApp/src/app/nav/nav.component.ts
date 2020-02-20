import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

model:any = {};
  constructor(public authService:AuthService, private alertify:AlertifyService, private route:Router) { }

  ngOnInit() {
  }
 login(){
   this.authService.login(this.model).subscribe(()=>{
   this.alertify.success('logged in successfully');
   }, ()=>{
     this.alertify.error('login failed');
     
   }, ()=>{
     this.route.navigate(['/members'])
   })   
 }

 loggedIn(){
  return this.authService.loggedIn();
 }
 logout(){
   localStorage.removeItem('token');
   this.route.navigate(['/'])
  this.alertify.message("You are logged out !")
 }
}