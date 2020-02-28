import { Component, OnInit } from '@angular/core';
import { UserService } from '../_services/user.service';
import { ActivatedRoute } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';
import { User } from '../_models/User';
import { Pagination, PaginatedResult } from '../_models/Pagination';

@Component({
  selector: 'app-lists',
  templateUrl: './lists.component.html',
  styleUrls: ['./lists.component.css']
})
export class ListsComponent implements OnInit {

  users:User[];
  pagination:Pagination; 
  likeParams = 'likers'
  constructor(private userService:UserService,
     private route:ActivatedRoute, private alertify:AlertifyService) { }

  ngOnInit() {
    this.route.data.subscribe(data=>{
     this.users = data['users'].result;
     this.pagination = data['users'].pagination;
    })
  }
  pageChanged(event:any):void{
    this.pagination.currentPage = event.page;
    this.loadUsers();
  }
    
    loadUsers(){
      this.userService.getUsers(this.pagination.currentPage, this.pagination.itemsPerPage, null, this.likeParams ).subscribe((result:PaginatedResult<User[]>)=>{
        this.users = result.result;
        this.pagination = result.pagination;
      }, error =>{
         this.alertify.error('Failed to load users')
      })
    }

}
