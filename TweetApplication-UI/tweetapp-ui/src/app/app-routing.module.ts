import { Component, NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { HomeComponent } from './home/home.component';
import { TweetComponent } from './tweet/tweet.component';
import { RegistrationComponent } from './user/registration/registration.component';
import { UserComponent } from './user/user.component';
import { LoginComponent } from './user/login/login.component';
import { AuthGuard } from './auth/auth.guard';
import { EditProfileComponent } from './user/edit-profile/edit-profile.component';
import { MyTweetsComponent } from './tweet/my-tweets/my-tweets.component';
import { PostTweetComponent } from './tweet/post-tweet/post-tweet.component';
import { ResetPasswordComponent } from './user/reset-password/reset-password.component';
import { EditTweetComponent } from './tweet/edit-tweet/edit-tweet.component';

const routes: Routes = [
  { path: '', redirectTo: 'user/login', pathMatch: 'full'},
  { path: 'home', component: HomeComponent, canActivate:[AuthGuard]},
  { path: 'tweet', component: TweetComponent ,
    children: [
      { path: 'my-tweets', component: MyTweetsComponent },
      { path: 'post-tweet', component: PostTweetComponent },
      { path: 'edit-tweet', component: EditTweetComponent }
    ]
  },
  { path: 'user', component: UserComponent ,
  children:[
    { path: 'registration', component: RegistrationComponent },
    { path: 'login', component: LoginComponent },
    { path: 'edit-profile', component: EditProfileComponent},
    { path: 'reset-password', component: ResetPasswordComponent },
  ]}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
