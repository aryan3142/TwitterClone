import { EventEmitter, Injectable, Output, ViewChild } from '@angular/core';
import { Form, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HttpHeaders,HttpClient, HttpErrorResponse } from '@angular/common/http';

import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  @ViewChild('logoInput', {  
    static: true  
}) logoInput : any;

imageUrl: string =''; 

  constructor(private fb: FormBuilder, private http: HttpClient) { }

  formModel = this.fb.group({
    FirstName: ['', Validators.required],
    LastName: ['', Validators.required],
    Gender: ['', Validators.required],
    DateOfBirth: ['', Validators.required],
    EmailId: ['', [Validators.required, Validators.email]],
    Passwords: this.fb.group({
      Password: ['', Validators.required],
      ConfirmPassword: ['', Validators.required]
    },{validator: this.comparePasswords })
  });

  response: any = {dbPath: ''};
  selectedFile: File | undefined;
  formData: any = {};

  uploadFinished = (event: any) => { 
    this.selectedFile = <File>event.target.files[0]; 
    var reader = new FileReader();  
        reader.onload = (event: any) => {  
            this.imageUrl = event.target.result;  
        }  
        reader.readAsDataURL(this.selectedFile);
  }

  comparePasswords(fb : FormGroup){
    let confirmPassCtrl = fb.get('ConfirmPassword');
    if(confirmPassCtrl?.errors == null || 'passwordMismatch' in confirmPassCtrl?.errors){
      if(fb.get('Password')?.value != confirmPassCtrl?.value){
        confirmPassCtrl?.setErrors({passwordMismatch: true});
      }
      else{
        confirmPassCtrl?.setErrors(null);
      }
    }
  }

  uploadFile(){
    this.formData = new FormData();
    this.formData.append('image', this.selectedFile, this.selectedFile?.name);
    //formData.append('ImageUpload', this.logoInput.nativeElement.files[0]);

    
    //return formData;
    /*this.http.post(environment.API_URL +'/register/upload', formData, {reportProgress: true, observe: 'events'})
      .subscribe({
        next: (event:any) => {
        /*if (event.type === HttpEventType.UploadProgress)
          this.progress = Math.round(100 * event.loaded / event.total);
        else if (event.type === HttpEventType.Response) {
          this.message = 'Upload success.';
          this.onUploadFinished.emit(event.body);
        }
        this.message = 'Upload success.';
        this.onUploadFinished.emit(event.body);
      },
      error: (err: HttpErrorResponse) => console.log(err)
    });*/
  }

  register(){
    //this.uploadFile();
    
    var body = {
      FirstName : this.formModel.value.FirstName,
      LastName : this.formModel.value.LastName,
      EmailId : this.formModel.value.EmailId,
      Password : this.formModel.value.Passwords.Password,
      DateOfBirth : this.formModel.value.DateOfBirth,
      Gender : this.formModel.value.Gender  
    };
    
    const httpOptions = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
                          responseType: 'text' as 'json'};

    //return this.http.post<any>(environment.API_URL + '/tweets/register', {body: body, File:this.formData }, httpOptions);
    return this.http.post(environment.API_URL + '/register', body, httpOptions);
  }

  login(formData:any){
    const httpOptions = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
                          responseType: 'text' as 'json'};

    return this.http.post(environment.API_URL + `/login?username=${formData.Username}&password=${formData.Password}`,httpOptions);
  }

  editProfile(form: any){
    var body = {
      FirstName : form.FirstName,
      LastName : form.LastName,
      EmailId : form.EmailId,
      DateOfBirth : form.DateOfBirth,
      Gender : form.Gender  
    };

    return this.http.post(environment.API_URL + '/editUser', body);
  }

  getUserProfile() {
    //var tokenHeader = new HttpHeaders({Authorization: 'Bearer ' + localStorage.getItem('token')});

    return this.http.get(environment.API_URL + '/getLoggedInUser');
  }

  searchByUsername(username: string){
    return this.http.get(environment.API_URL + '/user/search/' + username);
  }

  resetPassword(username: string ,formData:any){
    const httpOptions = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
                          responseType: 'text' as 'json'};

    return this.http.put<any>(environment.API_URL + `/${username}/forgot?oldPassword=${formData.OldPassword}&newPassword=${formData.NewPassword}`, {} ,httpOptions);
  }

  public createImgPath = (serverPath: string) => {
     //var user = this.getUserProfile();
     //user = this.response.dbPath;
    //return `https://localhost:5001/${serverPath}`; 
    //return environment.API_URL + `/${serverPath}`;
  }

  getAllUsers(){
    return this.http.get<any>(environment.API_URL + '/users/all');
  }
}
