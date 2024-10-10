import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'formatPhone',
  standalone: true,
})
export class FormatPhonePipe implements PipeTransform {
  transform(phoneNumber: string): string {
    if (!phoneNumber) {
      return '';
    }

    // Remove any non-numeric characters
    let cleaned = phoneNumber.replace(/\D/g, '');

    // Format the number as +56 9 XXXX XXXX if it's a valid length
    if (cleaned.length === 9) {
      return `+56 9 ${cleaned.slice(1, 5)} ${cleaned.slice(5)}`;
    }

    // Return the original if the number doesn't match the expected length
    return phoneNumber;
  }
}
