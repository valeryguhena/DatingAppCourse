import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { BsDatepickerConfig } from 'ngx-bootstrap';
import { User } from '../_models/User';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Output() cancelRegister= new EventEmitter();
  registerForm: FormGroup;
  
  bsConfig : Partial<BsDatepickerConfig> ;


  user: User;
  constructor(private authService:AuthService, private alertify: AlertifyService,
    private router:Router, private fb:FormBuilder) { }

  ngOnInit() {
    this.bsConfig = {
      containerClass:'theme-blue'
    }
    this.createFormValidation();
  }
  passwordMatchValidator(fGroup:FormGroup){
  return fGroup.get("password").value === fGroup.get("confirmPassword").value ? null : {"mismatch":true}
  }
  createFormValidation(){
    this.registerForm = this.fb.group({
      gender: ['male'],
      username: ['', Validators.required],
      knownAs: ['', Validators.required],
      dateOfBirth: [null, Validators.required],
      city: ['', Validators.required],
      country: ['', Validators.required],
    password:['', [Validators.required, Validators.minLength(4)]],
    confirmPassword:['', [Validators.required, Validators.minLength(4)]],
    },{validator:this.passwordMatchValidator});
  }
  register(){
    if(this.registerForm.valid){
      this.user = Object.assign({}, this.registerForm.value)
      this.authService.register(this.user).subscribe(()=>{
        this.alertify.success('registered successfully');
       }, error=>{
         this.alertify.error(error);
       }, ()=>{
         this.authService.login(this.user).subscribe(()=>{
          this.router.navigate(['/members'])
         });
       });
    }
  }
  cancel(){
     
   this.cancelRegister.emit();    
  }

}
