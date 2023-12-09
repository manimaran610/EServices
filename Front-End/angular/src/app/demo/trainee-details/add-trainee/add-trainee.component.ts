import { TraineeService } from './../../../../Services/trainee.service';
import { FileProcessingService } from './../../../../Services/Shared/file-processing.service';
/* eslint-disable @angular-eslint/no-empty-lifecycle-method */
import { CommonModule } from '@angular/common';
import { CUSTOM_ELEMENTS_SCHEMA, Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MessageService } from 'primeng/api';
import { MessagesModule } from 'primeng/messages';
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { HttpClientModule } from '@angular/common/http';
import { ToastModule } from 'primeng/toast';
import { Router } from '@angular/router';
import { BaseHttpClientServiceService } from 'src/Services/Shared/base-http-client-service.service';
import { Trainee } from 'src/Models/trainee.Model';
import { BaseResponse } from 'src/Models/response-models/base-response';

@Component({
  selector: 'app-add-trainee',
  standalone: true,
  imports: [CommonModule, SharedModule,  MessagesModule, HttpClientModule, ToastModule],
  providers: [ TraineeService,BaseHttpClientServiceService, MessageService, FileProcessingService],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  templateUrl: './add-trainee.component.html',
  styleUrls: ['./add-trainee.component.css']
})
export class AddTraineeComponent implements OnInit {
  isSaveLoading: boolean = false;
  isSaved: boolean = false;
  traineeFormGroup: FormGroup;

  tempFileName?: string;
  tempFile?: string;
  traineeModel: Trainee = new Trainee();

  constructor(
    private traineeService: TraineeService,
    private messageService: MessageService,
    private fileProcessingService: FileProcessingService,
    public router: Router
  ) {
    this.traineeFormGroup = new FormGroup({
      name: new FormControl(),
      certificate: new FormControl()
    });
    this.addFormControlValidators();
  }

  ngOnInit() {}
  navigateToUrl = (url: string | undefined) => this.router.navigateByUrl(url!);


  async onSubmit() {
    if (this.traineeFormGroup.valid) {
        console.log("Form submitted")
            this.traineeModel = this.traineeFormGroup.value;
            this.traineeModel.certificateName = this.tempFileName;
            this.traineeModel.certificateFile = this.tempFile;
            this.postTraineeToAPIServer();
            console.log(this.traineeModel)
    }else {
        this.messageService.add({ key: 'tc', severity: 'warn', summary: 'Warning', detail: "Instrument details Invalid", life: 4000 });
    }
}
onClear() { this.traineeFormGroup.reset() }


postTraineeToAPIServer() {
  this.isSaveLoading =true;
  this.traineeService.postTrainee(this.traineeModel).subscribe({
      next: (response: BaseResponse<number>) => {
          if (response.succeeded) this.messageService.add({ key: 'tc', severity: 'success', summary: 'Success', detail: 'Trainee details saved', life: 4000 }); 
          this.isSaved=true;      
      },
      error: (e) => {
          console.log(e)
          this.messageService.add({ key: 'tc', severity: 'error', summary: 'Failed',
          detail: e.status ==0? 'Server connection error': e.error.Message !== undefined ? e.error.Message : e.error.title, life: 4000 });
          this.isSaveLoading =false;

      },
      complete: () => {
          this.isSaveLoading =false;
          // this.router.navigateByUrl("/Home-page")
      },
  });
}
  async uploadFile(event: any) {

    if (event.files.length > 0 && event.files[0].size < 5120000) {
        this.tempFileName = event.files[0].name;
      if(event.files[0].name.split('?')[0].split('.').pop() !== 'pdf') {
        this.traineeFormGroup.controls['certificate'].setErrors({min:true});
      //  this.messageService.add({ key: 'tc', severity: 'warn', summary: 'Warning', detail: "Please choose PDF Files", life: 4000 });
      }
        this.tempFile = await this.fileProcessingService.fileToBase64(event.files[0]);
       
    }
    else if (event.files.length > 0 && event.files[0].size > 5120000) {
        this.traineeFormGroup.controls['certificate'].setErrors({max:true} )
    }
    else {
        this.tempFile = undefined;
        this.tempFileName = undefined;
    }
}

addFormControlValidators() {
    this.traineeFormGroup.controls['name'].addValidators([Validators.required])
    this.traineeFormGroup.controls['certificate'].addValidators([Validators.required])

}
}
