import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Photo } from 'src/app/_models/photo';
import { FileUploader } from 'ng2-file-upload';
import { environment } from 'src/environments/environment';
import { AuthService } from 'src/app/_services/auth.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css']
})
export class PhotoEditorComponent implements OnInit {
@Input() photos:Photo[];
@Output() ChangeUserMainPhoto = new EventEmitter<string>();

 uploader:FileUploader;
 hasBaseDropZoneOver:boolean;
 hasAnotherDropZoneOver:boolean;
 baseUrl = environment.apiUrl;
 currentMain:Photo;
  constructor(private AuthService:AuthService, private alertify: AlertifyService, private userService: UserService) { }

  ngOnInit() {
    this.initializeUploader();
  }

  public fileOverBase(e:any):void {
    this.hasBaseDropZoneOver = e;
  }
  initializeUploader(){
    this.uploader = new FileUploader({
     url : `${this.baseUrl}users/${this.AuthService.decodedToken.nameid}/photos`,
      authToken: `Bearer ${localStorage.getItem('token')}`,
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload:true,
      autoUpload:false,
      maxFileSize:10*1024*1024
    });
    this.uploader.onAfterAddingFile = (file)=>{file.withCredentials = false};
    this.uploader.onSuccessItem = (item, response, status, header)=>{
      if(response){
        const pho: Photo = JSON.parse(response);
        const photo = {
          id : pho.id,
          url: pho.url,
          description: pho.description,
          dateAdded:pho.dateAdded,
          isMain:pho.isMain
        };
        this.photos.push(photo)
      }
    };
  }

  setMainPhoto(photo:Photo){
    this.userService.setMainPhoto(this.AuthService.decodedToken.nameid, photo.id)
    .subscribe(()=>{
      var mainPhoto = this.photos.find(x=>x.isMain);
      mainPhoto.isMain = false;
      photo.isMain = true;
      this.AuthService.ChangeUserPhotoUrl(photo.url)
      this.AuthService.currentUser.photoUrl = photo.url;
      localStorage.setItem('user', JSON.stringify(this.AuthService.currentUser))
      this.alertify.success("Photo set as main successfully");
    }, error=>{
      this.alertify.error(error);
    })
  }

  deletePhoto(id:number){
    this.alertify.confirm("Are you sure you want to delete this photo?", ()=>{
      this.userService.deletePhoto(this.AuthService.decodedToken.nameid, id)
    .subscribe(()=>{
      this.photos.splice(this.photos.findIndex(p=>p.id == id), 1);
      this.alertify.success("Image deleted");
    }, err=>{
      this.alertify.error("Could no delete this photo");
    })
    })
  }
}
