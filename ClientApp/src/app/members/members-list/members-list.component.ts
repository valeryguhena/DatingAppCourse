import { Component, OnInit } from '@angular/core';
import { User } from '../../_models/User';
import { UserService } from '../../_services/user.service';
import { AlertifyService } from '../../_services/alertify.service';
import { ActivatedRoute } from '@angular/router';
import { Pagination, PaginatedResult } from 'src/app/_models/Pagination';

@Component({
  selector: 'app-members-list',
  templateUrl: './members-list.component.html',
  styleUrls: ['./members-list.component.css']
})
export class MembersListComponent implements OnInit {

  users:User[];
  user = JSON.parse(localStorage.getItem('user'));
  genderList = [{value:'female', display:'Females'}, {value:'male', display:'Males'}]
  userParams:any={};
  pagination:Pagination;

  constructor(private UserService:UserService, private alertify:AlertifyService,
              private route:ActivatedRoute) { }

  ngOnInit() {
   this.route.data.subscribe(data=>{
     this.users = data['users'].result;
     this.pagination = data['users'].pagination
   });

   this.userParams.gender = this.user.gender === 'female' ? 'male' : 'female';
   this.userParams.minAge = 18;
   this.userParams.maxAge = 99;
  }
pageChanged(event:any):void{
  this.pagination.currentPage = event.page;
  this.loadUsers();
}

resetFilters(){
  this.userParams.gender = this.user.gender === 'female' ? 'male' : 'female';
   this.userParams.minAge = 18;
   this.userParams.maxAge = 99;
   this.loadUsers();
}

  loadUsers(){
    this.UserService.getUsers(this.pagination.currentPage, this.pagination.itemsPerPage, this.userParams).subscribe((result:PaginatedResult<User[]>)=>{
      this.users = result.result;
      this.pagination = result.pagination;
    }, error =>{
       this.alertify.error('Failed to load users')
    })
  }
}
