import { BaseHttpClientServiceService } from './../../../../Services/Shared/base-http-client-service.service';
/* eslint-disable @angular-eslint/no-empty-lifecycle-method */
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { CommonModule } from '@angular/common';
import { CUSTOM_ELEMENTS_SCHEMA, Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ConfirmationService, Message } from 'primeng/api';
import { MessagesModule } from 'primeng/messages';
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { Instrument } from 'src/Models/instrument.Model';
import { InstrumentService } from 'src/Services/Instrument.service';
import { HttpClientModule } from '@angular/common/http';
import { BaseResponse } from 'src/Models/response-models/base-response';


@Component({
    selector: 'app-add-instrument',
    standalone: true,
    imports: [CommonModule, SharedModule, ConfirmDialogModule, MessagesModule, HttpClientModule],
    providers: [ConfirmationService, InstrumentService, BaseHttpClientServiceService],
    schemas: [CUSTOM_ELEMENTS_SCHEMA],
    templateUrl: './add-instrument.component.html',
    styleUrls: ['./add-instrument.component.css']
})
export class AddInstrumentComponent implements OnInit {

    instrumentFormGroup: FormGroup;
    msgs: Message[] = [];
    isAddInstrument: boolean = true;
    tempFileName?: string;
    tempFile?: string;
    instrumentModel: Instrument = new Instrument();

    constructor(private confirmationService: ConfirmationService, private instrumentService: InstrumentService) {

        this.instrumentFormGroup = new FormGroup(
            {
                serialNo: new FormControl(),
                make: new FormControl(),
                model: new FormControl(),
                calibratedOn: new FormControl(),
                calibratedDueOn: new FormControl(),
                certificate: new FormControl(),
            }
        );
        this.addFormControlValidators();
    }

    ngOnInit() { }

    async onSubmit() {
        console.log("Form submitted")
        this.instrumentModel = this.instrumentFormGroup.value;
        this.instrumentModel.certificateName = this.tempFileName;
        this.instrumentModel.CertificateFile = this.tempFile;
        console.log(this.instrumentModel)
        this.confirmDialog();
    }

    onClear() { this.instrumentFormGroup.reset() }

    confirmDialog() {
        this.confirmationService.confirm({
            message: 'Are you sure that you want to save instrument?',
            header: 'Confirmation',
            icon: 'pi pi-exclamation-triangle',
            acceptButtonStyleClass: 'btn btn-success m-2',
            rejectButtonStyleClass: 'btn btn-danger',
            accept: () => {
                this.postInstrumentToAPIServer()
            },
            reject: () => {
                this.msgs = [{ severity: 'error', summary: 'Failed', detail: 'Instrument not added', life: 3000 }];
            }
        });
    }

    postInstrumentToAPIServer() {
        this.instrumentService.postInstrument(this.instrumentModel).subscribe({
            next: (response: BaseResponse<number>) => {
                this.msgs = [{ severity: 'success', summary: 'Success', detail: 'Instrument created', life: 3000 }];
            },
            error: (e) => {
                this.msgs = [{ severity: 'error', summary: 'Failed', detail: e.error.message, life: 3000 }];
            },
            complete: () => {
                console.log('Complete tenant List');
            },
        });
    }
    async uploadFile(event: any) {
        if (event.files.length > 0 && event.files[0].size < 5120000) {
            this.tempFileName = event.files[0].name;
            console.log(event.files[0])
            console.log(this.tempFileName)
            this.tempFile = await this.fileToBase64(event.files[0]);
            console.log(this.tempFile)
        }
    }

    addFormControlValidators() {
        this.instrumentFormGroup.controls['serialNo'].addValidators([Validators.required])
    }

    toggle() { this.isAddInstrument = !this.isAddInstrument; }

    base64ToFile(base64String: string, filename: string, mimeType: string): File {
        const byteCharacters = atob(base64String);
        const byteNumbers = new Array(byteCharacters.length);

        for (let i = 0; i < byteCharacters.length; i++) {
            byteNumbers[i] = byteCharacters.charCodeAt(i);
        }

        const byteArray = new Uint8Array(byteNumbers);
        const blob = new Blob([byteArray], { type: mimeType });

        return new File([blob], filename, { type: mimeType });
    }

    fileToBase64(file: File): Promise<string> {
        return new Promise<string>((resolve, reject) => {
            const reader = new FileReader();

            reader.onload = () => {
                if (typeof reader.result === 'string') {
                    resolve(reader.result);
                } else {
                    reject(new Error('Failed to convert file to base64'));
                }
            };

            reader.onerror = (error) => {
                reject(error);
            };

            reader.readAsDataURL(file);
        });
    }
}