import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'formatRut',
  standalone: true,
})
export class FormatRutPipe implements PipeTransform {
  transform(value: string): string {
    if (!value) return '';

    // Ensure the input is a string
    let rut = value.toString();

    // Remove all non-numeric characters except for the last character (verification digit)
    rut = rut.replace(/[^0-9kK]/g, '');

    // Separate the RUT into body and verification digit
    const body = rut.slice(0, -1);
    const verificationDigit = rut.slice(-1);

    // Add thousands separators to the body
    let formattedBody = '';
    for (let i = body.length - 1, count = 0; i >= 0; i--, count++) {
      formattedBody = body[i] + formattedBody;
      if (count % 3 === 2 && i > 0) {
        formattedBody = '.' + formattedBody;
      }
    }
    return `${formattedBody}-${verificationDigit}`;
  }
}
