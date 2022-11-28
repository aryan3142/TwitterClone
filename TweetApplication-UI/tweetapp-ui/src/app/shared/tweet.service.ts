import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class TweetService {
  formModel = this.fb.group({
    Message: ['', Validators.required],
  });

  constructor(private fb: FormBuilder, private http: HttpClient) { }

  postNewTweet(username:any){
    var body = {
      tweetMessage : this.formModel.value.Message, 
    }
    
    const httpOptions = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
                          responseType: 'text' as 'json'};
    return this.http.post<any>(environment.API_URL + `/${username}/add`, body, httpOptions);
  }

  getAllTweets(){
    return this.http.get<any>(environment.API_URL + '/GetAllTweets');
  }

  getUserTweets(username: string){
    return this.http.get<any>(environment.API_URL + `/${username}/GetUserTweet`);
  }

  likeOrDisLikeTweet(tweetId: string, username:string){
    const httpOptions = { headers: new HttpHeaders({ 'Content-Type': 'application/json' })};

    return this.http.put(environment.API_URL + `/${username}/like/${tweetId}`, {}, httpOptions);
    // this.http.put(environment.API_URL + '/' +username + '/like/' + tweetId, null).subscribe((response) => {
    //         this.getUserTweets(username);
    //     });
  }

  reply(tweetId: string, username:string, message:any){
    var body = {
      Message : message.value
    }
    const httpOptions = { headers: new HttpHeaders({ 'Content-Type': 'application/json' })};

    return this.http.post(environment.API_URL + `/${username}/reply/${tweetId}`, `'${message.replyText}'`, httpOptions);
  }

  deleteTweet(tweetId: string, username: string){
    const httpOptions = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
                          responseType: 'text' as 'json'};
    return this.http.delete(environment.API_URL + `/${username}/delete/${tweetId}`, httpOptions);
  }

  editTweet(tweetId: string, username: string, message: any){
    const httpOptions = { headers: new HttpHeaders({ 'Content-Type': 'application/json' })};
    return this.http.put(environment.API_URL + `/${username}/update/${tweetId}`, `'${message}'`,httpOptions);
  }

}
