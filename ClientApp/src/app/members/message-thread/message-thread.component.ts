import {Component, Input, OnInit} from '@angular/core';
import {UserService} from "../../_services/user.service";
import {AuthService} from "../../_services/auth.service";
import {AlertifyService} from "../../_services/alertify.service";
import {Message} from "../../_models/message";
import { tap } from 'rxjs/operators';

@Component({
  selector: 'app-message-thread',
  templateUrl: './message-thread.component.html',
  styleUrls: ['./message-thread.component.css']
})
export class MessageThreadComponent implements OnInit {
@Input() recipientId:number;
messages:Message[];
newMessage:any = {};
  constructor(private userService:UserService, private authService:AuthService,
              private alertify:AlertifyService) { }

  ngOnInit() {
    this.getMessagesThread();
  }
  getMessagesThread(){
    this.userService.getMessageThread(this.authService.decodedToken.nameid, this.recipientId)
    .pipe(
      tap(messages=>{
        for (let i = 0; i < messages.length; i++) {
          const element = messages[i]
          if(element.isRead === false && element.recipientId === +this.authService.decodedToken.nameid){
           this.userService.markMessageAsRead(element.id,this.authService.decodedToken.nameid);
          }
          
        }
      })
    )
      .subscribe(messageThread =>{
        this.messages = messageThread;
      },err=>{
        this.alertify.error(err.error);
      })
  }
  sendMessage(){
    this.newMessage.recipientId = this.recipientId;
    this.userService.sendMessage(this.authService.decodedToken.nameid, this.newMessage)
    .subscribe((message:Message)=>{
      this.messages.push(message);
      this.newMessage.content = "";
      this.alertify.success("message sent");
    }, err =>{
      this.alertify.error(err.error);
    })
  }

}
