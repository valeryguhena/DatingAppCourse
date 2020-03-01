import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { MembersListComponent } from './members/members-list/members-list.component';
import { ListsComponent } from './lists/lists.component';
import { MessagesComponent } from './messages/messages.component';
import { AuthGuard } from './_guards/auth.guard';
import {MemberDetailComponent} from "./members/member-detail/member-detail.component";
import {MemberEditComponent} from "./members/member-edit/member-edit.component";
import { MemberListResolver } from './_resolvers/member-list.resolver';
import { MemberDetailResolver } from './_resolvers/member-detail.resolver';
import { MemberEditResolver } from './_resolvers/member-edit.resolver';
import { PreventUnsaveChangesGuard } from './_guards/prevent-unsave-changes.guard';
import { ListsResolver } from './_resolvers/lists.resolver';
import {MessagesResolver} from "./_resolvers/messages.resolver";



const routes: Routes = [
  {path:'', component:HomeComponent},
  {
    path:'',
    runGuardsAndResolvers:'always',
    canActivate:[AuthGuard],
    children:[
      {path:'members', component:MembersListComponent, resolve:{users:MemberListResolver}},
      {path:'members/:id', component:MemberDetailComponent,resolve:{user:MemberDetailResolver} },
      {path:'member/edit', component:MemberEditComponent,resolve:{user:MemberEditResolver},
        canDeactivate:[PreventUnsaveChangesGuard]},
      {path:'lists', component:ListsComponent, resolve:{users: ListsResolver}},
      {path:'messages', component:MessagesComponent, resolve:{messages:MessagesResolver}},
    ]
  },

  {path:'**', redirectTo:'', pathMatch: 'full'},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
