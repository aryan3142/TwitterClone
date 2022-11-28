import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { UserComponent } from './user/user.component';
import { TweetComponent } from './tweet/tweet.component';

import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RegistrationComponent } from './user/registration/registration.component';
import { LoginComponent } from './user/login/login.component';
import { UserService } from './shared/user.service';
import { TweetService } from './shared/tweet.service';
import { AuthInterceptor } from './auth/auth.interceptor';
import { EditProfileComponent } from './user/edit-profile/edit-profile.component';
import { MyTweetsComponent } from './tweet/my-tweets/my-tweets.component';
import { PostTweetComponent } from './tweet/post-tweet/post-tweet.component';
import { ResetPasswordComponent } from './user/reset-password/reset-password.component';
import { EditTweetComponent } from './tweet/edit-tweet/edit-tweet.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    UserComponent,
    TweetComponent,
    RegistrationComponent,
    LoginComponent,
    EditProfileComponent,
    MyTweetsComponent,
    PostTweetComponent,
    ResetPasswordComponent,
    EditTweetComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule
  ],
  providers: [UserService, {
    provide: HTTP_INTERCEPTORS,
    useClass: AuthInterceptor,
    multi: true
  },TweetService],
  bootstrap: [AppComponent]
})
export class AppModule {

 }
