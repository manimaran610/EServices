import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class FileProcessingService {

constructor() { }

base64ToFile(base64String: string, filename: string,mimeType:string): File {

 const byteCharacters=base64String.includes(',') ? atob(base64String.split(',')[1]):atob(base64String);
 
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

downloadFile(file: File) {
  const blob = new Blob([file], { type: file.type });
  const url = window.URL.createObjectURL(blob);
  const a = document.createElement('a');
  a.href = url;
  a.download = file.name;
  a.click();
  window.URL.revokeObjectURL(url);
}

}
