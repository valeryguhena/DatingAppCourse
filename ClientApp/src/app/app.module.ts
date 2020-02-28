import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import {BsDropdownModule, TabsModule, BsDatepickerModule, PaginationModule, ButtonsModule} from 'ngx-bootstrap';
import {HttpClientModule} from '@angular/common/http';
import { JwtModule } from '@auth0/angular-jwt';
import { NgxGalleryModule } from 'ngx-gallery';
import {TimeAgoPipe} from 'time-ago-pipe';


import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ValueComponent } from './value/value.component';
import { NavComponent } from './nav/nav.component';
import { AuthService } from './_services/auth.service';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import { AlertifyService } from './_services/alertify.service';
import { MembersListComponent } from './members/members-list/members-list.component';
import { ListsComponent } from './lists/lists.component';
import { MessagesComponent } from './messages/messages.component';
import { AuthGuard } from './_guards/auth.guard';
import { PreventUnsaveChangesGuard } from './_guards/prevent-unsave-changes.guard';
import { UserService } from './_services/user.service';
import { MemberCardComponent } from './members/member-card/member-card.component';
import {MemberDetailComponent} from "./members/member-detail/member-detail.component";
import { MemberListResolver } from './_resolvers/member-list.resolver';
import { MemberEditResolver } from './_resolvers/member-edit.resolver';
import { MemberDetailResolver } from './_resolvers/member-detail.resolver';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { PhotoEditorComponent } from './members/photo-editor/photo-editor.component';
import { FileUploadModule } from 'ng2-file-upload';
import { ErrorInterceptor } from './_services/error.interceptor';
import { ListsResolver } from './_resolvers/lists.resolver';

export function tokenGetter(){
   return localStorage.getItem('token');
}
@NgModule({
   declarations: [
      AppComponent,
      ValueComponent,
      NavComponent,
      HomeComponent,
      RegisterComponent,
      MembersListComponent,
      ListsComponent,
      MessagesComponent,
      MemberCardComponent,
      MemberDetailComponent,
      MemberEditComponent,   TimeAgoPipe,
      PhotoEditorComponent
   ],
   imports: [
      BrowserModule,
      AppRoutingModule,
      HttpClientModule,
      FormsModule,
      BrowserAnimationsModule,
      NgxGalleryModule,
      ReactiveFormsModule,
      FileUploadModule,
      ButtonsModule.forRoot(),
      PaginationModule.forRoot(),
      BsDatepickerModule.forRoot(),
      TabsModule.forRoot(),
      BsDropdownModule.forRoot(),
      JwtModule.forRoot({
        config:{
           tokenGetter:tokenGetter,
           whitelistedDomains: ['localhost:5000'],
           blacklistedRoutes:['localhost:5000/api/auth']
        }
      })
   ],
   providers: [
      AuthService,
      AlertifyService,
      AuthGuard,
      UserService,
      MemberListResolver,
      MemberDetailResolver,
      MemberEditResolver,
      PreventUnsaveChangesGuard,
      ErrorInterceptor,
      ListsResolver

   ],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
