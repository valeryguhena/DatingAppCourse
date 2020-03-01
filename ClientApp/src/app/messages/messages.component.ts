import { Component, OnInit } from '@angular/core';
import {ActivatedRoute} from "@angular/router";
import {AuthService} from "../_services/auth.service";
import {AlertifyService} from "../_services/alertify.service";
import {Message} from "../_models/message";
import {PaginatedResult, Pagination} from "../_models/Pagination";
import {UserService} from "../_services/user.service";


@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css']
})
export class MessagesComponent implements OnInit {
  messages:Message[];
  pagination:Pagination;
  messageContainer = 'Unread';

  constructor(private route:ActivatedRoute, private authService:AuthService
              , private alertify:AlertifyService, private userService:UserService) { }

  ngOnInit() {
    this.route.data.subscribe(data=>{
      this.messages = data['messages'].result;
      this.pagination = data['messages'].pagination;
    });
  }
 loadMessages(){
   const currentUserId = +this.authService.decodedToken.nameid;
    this.userService.getMessages(this.authService.decodedToken.nameid,
      this.pagination.currentPage, this.pagination.itemsPerPage, this.messageContainer)
      .subscribe((res:PaginatedResult<Message[]>)=>{
        this.messages = res.result;
        this.pagination = res.pagination;
      }, err=>{
        this.alertify.error(err.error);
      })
   }
   deleteMessage(id: number){
     this.alertify.confirm("Are you sure you want to delete this messages",()=>{
       this.userService.deleteMessage(id, this.authService.decodedToken.nameid)
       .subscribe(()=>{
         this.messages.splice(this.messages.findIndex(i=>i.id == id), 1);
         this.alertify.success("Message deleted successfully")
       }, err=>{
         this.alertify.error(err.error);
       });
     })
   }

   pageChanged(event:any){
    this.pagination.currentPage = event.page;
    this.loadMessages();
 }
}
